using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class CanvasScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public CharacterScript player;

    public CharacterScript enemy;

    public HeartPrefabScript heartPrefab;

    public HeartPrefabScript emptyHeart;

    public Image pauseImage;

    public Image gameOverImage;

    public TMP_Text healText;

    // The Text on the Game Over Screen
    public TMP_Text gameOverText;

    public Button[] nextLevelButtons;

    public Button[] playerActionButtons;

    private HeartPrefabScript[] playerHeartInstances;

    private HeartPrefabScript[] enemyHeartInstances;

    void Start()
    {
        playerHeartInstances = new HeartPrefabScript[player.maxLifePoints];
        enemyHeartInstances = new HeartPrefabScript[enemy.maxLifePoints];
        SetupHeartIntances(player, -810f, 410f);
        SetupHeartIntances(enemy, 310f, 410f);
        UpdateHeartInstances(player.tag);
        UpdateHeartInstances(enemy.tag);
        gameOverImage.gameObject.SetActive(false);
        pauseImage.gameObject.SetActive(false);
        foreach (Button button in playerActionButtons)
        {
            button.gameObject.SetActive(false);
        }

        UpdateHealText(player.numberOfHeals.ToString());
    }

    void SetupHeartIntances(CharacterScript character, float xStartingPoint, float yStartingPoint)
    {
        if(character.tag == "Player")
        {
            for (int index = 0; index < character.maxLifePoints; index++)
            {
                playerHeartInstances[index] = Instantiate(heartPrefab);
                playerHeartInstances[index].transform.SetParent(transform);
                playerHeartInstances[index].transform.localPosition = new Vector3(xStartingPoint + 100f * (index % 5), yStartingPoint - 100f * (index / 5), 0f);
            }
        }
        else
        {
            for (int index = 0; index < character.maxLifePoints; index++)
            {
                enemyHeartInstances[index] = Instantiate(heartPrefab);
                enemyHeartInstances[index].transform.SetParent(transform);
                enemyHeartInstances[index].transform.localPosition = new Vector3(xStartingPoint + 100f * (index % 5), yStartingPoint - 100f * (index / 5), 0f);
            }
        }
    }

    public void UpdateHeartInstances(string tag)
    {
        if(tag == "Player")
        {
            for(int index = player.maxLifePoints - 1; index > player.currentLifePoints - 1; index--)
            {
                ReplaceHeart(emptyHeart, playerHeartInstances, index);
            }
            for(int index = player.currentLifePoints - 1; index >= 0 ; index--)
            {
                ReplaceHeart(heartPrefab, playerHeartInstances, index);
            }
        }
        else
        {
            for (int index = enemy.maxLifePoints - 1; index > enemy.currentLifePoints - 1; index--)
            {
                ReplaceHeart(emptyHeart, enemyHeartInstances, index);
            }
            for (int index = enemy.currentLifePoints - 1; index >= 0; index--)
            {
                ReplaceHeart(heartPrefab, enemyHeartInstances, index);
            }
        }
    }

    public void UpdateHealText(string inputString)
    {
        healText.text = inputString;
    }

    public void UpdateGameOverText(string inputString)
    {
        gameOverText.text = inputString;
    }

    private void ReplaceHeart(HeartPrefabScript heartType, HeartPrefabScript[] heartArray, int index)
    {
        HeartPrefabScript newHeart;
        newHeart = Instantiate(heartType);
        newHeart.transform.SetParent(transform);
        newHeart.transform.localPosition = heartArray[index].transform.localPosition;
        newHeart.transform.localScale = Vector3.one;
        Destroy(heartArray[index].gameObject);
        heartArray[index] = newHeart;
    }
}
