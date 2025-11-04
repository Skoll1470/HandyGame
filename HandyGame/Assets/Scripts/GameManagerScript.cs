using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class GameManagerScript : MonoBehaviour
{
    // The Player Character
    public CharacterScript player;

    // The Healing Effect
    public HealingFXScript healingFX;

    // The Enemy Character
    public CharacterScript enemy;

    // The Canvas Script
    public CanvasScript canvas;

    // The Dialog Box Script
    public DialogScript dialog;

    // The Background Music Audio Source
    public AudioSource backgroundMusic;

    // The Winning Theme Audio Clip
    public AudioClip winningClip;

    // The Losing Theme Audio Clip
    public AudioClip losingClip;

    // Boolean indicating if the Gamestate is Open
    private bool isUnoccupied = true;

    // Boolean indicating if the Player just attacked
    private bool playerAttacked = false;

    // Boolean indicating if this is the Start of the Turn
    private bool isStartOfTurn = true;

    private bool playerHealed = false;

    // Boolean indicating the current Enemey Attack index
    private int enemyAttackIndex;

    // Boolean indicating the remaining number of Turns to finish the Enemy Attack
    private int enemyAttackRemainingTurns = 0;

    // The Data Class used for the Json manipulations
    private DataClass dataClass;

    // The Auto Scrolling Current Time accumulation
    private float autoScrollingTime = 0.0f;

    // The Auto Scrolling Button Index
    private int autoScrollingIndex = 0;

    // The Data Class containing the Json informations
    private class DataClass
    {
        public bool autoScrolling = false;
        public int autoScrollingSpeed = 5;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Starting with the encounter dialog
        StartNewDialog("Un ennemi est apparu !");

        // Reading the Json to setup the variables
        ReadJson();
    }

    // Update is called once per frame
    void Update()
    {
        // Checking if Left Click or Space Button has been pressed while the Auto Scrolling is on
        if ((Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Space)) &&
            dataClass.autoScrolling &&
            isUnoccupied &&
            !dialog.gameObject.activeInHierarchy &&
            Time.timeScale > 0)
        {
            // Starting the Player Action corresponding to the Current Auto Scrolling Index
            canvas.playerActionButtons[autoScrollingIndex].onClick.Invoke();
        }

        // Checking if the Player just performed an action
        if (player.GetStopAttacking())
        {
            // Updating the Gamestate
            isUnoccupied = true;
            player.SetStopAttacking(0);
            playerAttacked = true;
            autoScrollingIndex = 0;
            autoScrollingTime = 0.0f;

            // Determining wich Dialog to show
            if(player.GetIsBlocking())
            {
                StartNewDialog("Vous vous préparez à bloquer une attaque.");
            }
            else if(playerHealed)
            {
                StartNewDialog("Vous vous êtes soigné de 2 dégâts !");
                playerHealed = false;
            }
            else if(enemy.GetIsBlocking())
            {
                StartNewDialog("Ennemi a bloqué l'attaque !");
            }
            else
            {
                StartNewDialog("Vous avez infligé " + player.attacks[0].damage + " dégâts !");
            }

            // Resetting the Enemy Blocking State
            enemy.SetIsBlocking(0);
        }

        // Checking if the Enemy just performed an action
        else if (enemy.GetStopAttacking())
        {
            // Updating the Gamestate
            isUnoccupied = true;
            enemy.SetStopAttacking(0);
            enemyAttackRemainingTurns--;

            // Determining wich Dialog to show
            if (player.currentLifePoints != 0)
            {
                if (enemyAttackRemainingTurns > 0)
                {
                    StartNewDialog("Ennemi prépare une attaque...");
                }
                else if(enemy.GetIsBlocking())
                {
                    StartNewDialog("Ennemi se prépare à bloquer une attaque...");
                }
                else if (player.GetIsBlocking())
                {
                    StartNewDialog("Vous avez bloqué l'attaque !");
                }
                else
                {
                    StartNewDialog("Vous avez subit " + enemy.attacks[enemyAttackIndex].damage + " dégâts !");
                }

                // Resetting the Player Blocking State
                player.SetIsBlocking(0);
            }
        }

        // Starting the Enemy Attack when the Player Attack finished
        if (!dialog.gameObject.activeInHierarchy && playerAttacked)
        {
            StartEnemyAttack();
            playerAttacked = false;
        }

        // Checking if one Character is Dead, then the game is over
        if (enemy.GetIsDead() || player.GetIsDead())
        {
            // Showing the Game Over Screen
            canvas.gameOverImage.gameObject.SetActive(true);

            // If it's the Player that is Dead, then the Player lost
            if(player.GetIsDead())
            {
                // Updatting the Game Over Text accordingly
                canvas.UpdateGameOverText("Vous avez perdu...");

                // Playing the Losing Audio Clip
                if (backgroundMusic.clip != losingClip)
                {
                    backgroundMusic.clip = losingClip;
                    backgroundMusic.Play();
                }

                // Deactivating the Next Level Button
                canvas.nextLevelButtons[0].gameObject.SetActive(false);
            }

            // Else, the Player won
            else
            {
                // Playing the Winning Audio Clip
                if (backgroundMusic.clip != winningClip)
                {
                    backgroundMusic.clip = winningClip;
                    backgroundMusic.Play();
                }

                // Deactivating the Play Again Button
                canvas.nextLevelButtons[1].gameObject.SetActive(false);
            }
        }
    }

    private void FixedUpdate()
    {
        // Variable that keep track of the number of active Player Action Buttons
        int numberOfActives = 0;

        // Determining if the Gamestate is at the Start of a Turn 
        isStartOfTurn = !dialog.gameObject.activeInHierarchy && isUnoccupied && player.currentLifePoints != 0 && enemy.currentLifePoints != 0;

        // Reading the Json to update the variables
        ReadJson();

        foreach (Button button in canvas.playerActionButtons)
        {
            // Showing or not the Player Action Buttons if it's the Start of the Turn
            button.gameObject.SetActive(isStartOfTurn);

            // If the Player is at Max Life Points, doesn't show the Heal Button
            if(button == canvas.playerActionButtons[2])
            {
                if(player.numberOfHeals <= 0 || player.currentLifePoints == player.maxLifePoints)
                {
                    button.gameObject.SetActive(false);
                }
            }

            // Updating the Number of Active Player Action Buttons variable
            numberOfActives = button.gameObject.activeInHierarchy ? numberOfActives + 1 : numberOfActives;

            // Resetting every Player Action Buttons' color
            button.image.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        }
        
        if(dataClass.autoScrolling && isStartOfTurn)
        {
            // Updating the Auto Scrolling Time 
            autoScrollingTime += Time.deltaTime;

            // If Auto Scrolling Time is at the Speed time, resetting the Auto Scrolling Time and Selecting the next Player Action Button
            if(autoScrollingTime >= dataClass.autoScrollingSpeed)
            {
                autoScrollingIndex = autoScrollingIndex == numberOfActives - 1 ? 0 : autoScrollingIndex + 1;
                autoScrollingTime = 0.0f;
            }
            canvas.playerActionButtons[autoScrollingIndex].Select();
            canvas.playerActionButtons[autoScrollingIndex].image.color = new Color(0.0f, 1.0f, 1.0f, 1.0f);
        }
    }

    // Method that starts the Player Attack
    public void StartPlayerAttack(int index)
    {
        if(isUnoccupied && !player.animator.GetBool("IsDead"))
        {
            isUnoccupied = false;
            player.PerformAttackByIndex(index);
        }
    }

    // Method that starts the Enemy Attack
    void StartEnemyAttack()
    {
        if(isUnoccupied && !enemy.animator.GetBool("IsDead"))
        {
            isUnoccupied = false;
            if(enemyAttackRemainingTurns <= 0)
            {
                enemyAttackIndex = Random.Range(0, enemy.attacks.Count);
                enemyAttackRemainingTurns = enemy.attacks[enemyAttackIndex].numberOfTurns;
            }
            enemy.PerformAttackByIndex(enemyAttackIndex);
        }   
    }

    // Method that Initialize the Player Variables for Blocking
    public void StartPlayerBlocking()
    {
        player.SetIsBlocking(1);
        player.SetStopAttacking(1);
    }

    // Method that starts the Player Heal
    public void StartPlayerHeal(int amount)
    {
        player.Heal(amount);
        player.SetStopAttacking(1);
        canvas.UpdateHealText(player.numberOfHeals.ToString());
        playerHealed = true;
        healingFX.gameObject.SetActive(true);
    }

    // Method that creates a new dialog containing the input string
    void StartNewDialog(string input)
    {
        dialog.SetLine(input);
        dialog.StartDialog();
    }

    private void ReadJson()
    {
        string json = File.ReadAllText(Application.persistentDataPath + "/Settings.json");
        dataClass = JsonUtility.FromJson<DataClass>(json);
    }
}