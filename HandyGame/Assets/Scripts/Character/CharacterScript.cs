using System.Collections.Generic;
using UnityEngine;

public class CharacterScript : MonoBehaviour
{
    // The maximum amount of Life Points
    public int maxLifePoints;

    // The current amount of Life Points
    public int currentLifePoints;

    // The Character movement speed
    public float moveSpeed;

    // The Number of Remaining Heals
    public int numberOfHeals;

    // The Animator
    public Animator animator;

    // The RigidBody2D
    public Rigidbody2D rigidBody;

    // The SpriteRenderer
    public SpriteRenderer spriteRenderer;

    // The Audio Source
    public AudioSource audioSource;

    // The Canvas for the Heart visuals
    public CanvasScript canvas;

    // Array of Attacks used
    public List<AttackScript> attacks;

    // Array of Audio Clips used
    public List<AudioClip> audioClips;

    // Array of the Name references of each Audio Clips
    public List<string> audioClipNames;

    // Boolean indicating if the Character just Stopped Attacking
    private bool stopAttacking = false;

    // Boolean indicating if the Character is Dead
    private bool isDead = false;

    // Boolean indicating if the Character is Blocking
    private bool isBlocking = false;

    // Called when taking damage
    public void TakeDamage(int damage)
    {
        // If the Character is Dead, exit
        if (currentLifePoints <= 0) return;

        // If the Character is Blocking, play the Blocking Audio and exit
        if(isBlocking)
        {
            PlayAudioClipByName("AudioBlock");
            return;
        }

        // Setting the updated current Life Points, if it would be negative, set it to 0
        currentLifePoints = (currentLifePoints - damage) < 0 ? 0 : currentLifePoints - damage;

        // Calling the Animator Trigger to play the Hurt and Death Animaton
        animator.SetTrigger("TriggerHurt");
        animator.SetBool("IsDead", currentLifePoints == 0);
        // Updating the corresponding Hearts in the Canvas
        canvas.UpdateHeartInstances(tag);
    }

    public void Heal(int amount)
    {
        // If the Character has no Life Points or no more Heals, exit
        if( currentLifePoints <= 0 || numberOfHeals <= 0) return;

        // Setting the updated current Life Points, if the Character is at Max Life Points, doesn't change
        currentLifePoints = (currentLifePoints == maxLifePoints) ? maxLifePoints : (currentLifePoints + amount);

        // Playing the Heal Audio
        PlayAudioClipByName("AudioHeal");

        // Updating the Heart HUD
        canvas.UpdateHeartInstances(tag);

        // Decrementing the Number of Heals
        numberOfHeals--;
    }

    public void PlayAudioClipByName(string reference)
    {
        // Getting the Index of the Audio Clip
        int index = audioClipNames.IndexOf(reference);

        // If the Index of the Audio Clip was not Found, exit
        if(index == -1)
        {
            Debug.Log("Audio Clip Not Found");
            return;
        }

        // Replacing the Current Audio Clip by the one Found
        audioSource.clip = audioClips[index];

        // Randomly shifting the pitch to avoid repeatitivenesse
        audioSource.pitch = Random.Range(0.9f, 1.1f);

        // Playing the Audio Clip
        audioSource.Play();
    }

    // Setter for stopAttacking
    public void SetStopAttacking(int inputValue)
    {
        stopAttacking = inputValue != 0;
    }

    // Getter for stopAttacking
    public bool GetStopAttacking()
    {
        return stopAttacking;
    }

    // Method that Performs an Attack givien by its Index in the Attacks Array
    public void PerformAttackByIndex(int index)
    {
        attacks[index].PerformAttack();    
    }

    // Setter for IsDead Boolean
    public void SetIsDead(int inputValue)
    {
        isDead = inputValue != 0;
    }

    // Getter for IsDead Boolean
    public bool GetIsDead()
    {
        return isDead;
    }

    // Setter for IsBlocking Boolean
    public void SetIsBlocking(int inputValue)
    {
        isBlocking = inputValue != 0;
        animator.SetBool("IsBlocking", isBlocking);
    }

    // Getter for IsBlocking Boolean
    public bool GetIsBlocking()
    {
        return isBlocking;
    }
}
