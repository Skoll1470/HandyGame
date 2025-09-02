using UnityEngine;

public class WizardChargedAttackScript : AttackScript
{
    private Vector2 distanceToPlayer;

    private bool isMoving = false;

    private bool isWalkingBack = false;

    private bool isAttackPerforming = false;

    private void Start()
    {
        if(numberOfTurns != 2)
        {
            Debug.Log("Wizard's Charged Attack does not have the correct number of turns");
        }
    }

    private void Update()
    {
        numberOfTurns = numberOfTurns <= 0 ? 2 : numberOfTurns;
    }

    private void FixedUpdate()
    {
        if(isAttackPerforming)
        {
            UpdateDistanceToPlayer();
            if (distanceToPlayer.x <= 4f && isMoving && !isWalkingBack)
            {
                character.animator.SetTrigger(animatorTriggerName);
                isMoving = false;
            }
            else if (distanceToPlayer.x >= 6.3f && isMoving && isWalkingBack)
            {
                isMoving = false;
                isWalkingBack = false;
                isAttackPerforming = false;
            }
            character.rigidBody.linearVelocityX = (isMoving ? character.moveSpeed : 0f) * (isWalkingBack ? 1f : -1f);
            character.animator.SetBool("IsMoving", isMoving);
        }
    }

    public override void PerformAttack()
    {
        if(numberOfTurns == 2)
        {
            character.animator.SetBool("IsPreparing", true);
            character.SetStopAttacking(1);
        }
        else
        {
            character.animator.SetBool("IsPreparing", false);
            isMoving = true;
            isAttackPerforming = true;
        }
        numberOfTurns--;
    }

    public void ApplyDamageChargedAttack()
    {
        opponent.TakeDamage(damage);
    }

    void UpdateDistanceToPlayer()
    {
        distanceToPlayer.x = Mathf.Abs(opponent.transform.position.x - character.transform.position.x);
    }

    public void SetIsMovingChargedAttack(int inputValue)
    {
        isMoving = inputValue != 0;
    }

    public void SetIsWalkingBackChargedAttack(int inputValue)
    {
        isWalkingBack = inputValue != 0;
    }

    public void SetSpriteRendererFlipX(int inputValue)
    {
        character.spriteRenderer.flipX = inputValue != 0;
    }
}