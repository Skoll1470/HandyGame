using UnityEngine;

public abstract class AttackScript : MonoBehaviour
{
    public CharacterScript character;

    public CharacterScript opponent;

    public int damage;

    public int numberOfTurns = 1;

    public string animatorTriggerName;

    public abstract void PerformAttack();
}