using UnityEngine;

public class BasicAttack : AttackScript
{
    // Vector beetween the Enemy and the Player
    private Vector2 distanceToEnemy;

    // Boolean indicating if the Player is playing a Dashing animation
    private bool isDashing = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        distanceToEnemy.x = opponent.transform.position.x - transform.position.x;
    }

    private void FixedUpdate()
    {
        //distanceToEnemy.x = Mathf.Abs(opponent.transform.position.x - transform.position.x);
        character.rigidBody.linearVelocityX = (isDashing ? character.moveSpeed : 0f) * (character.spriteRenderer.flipX ? -1f : 1f);
    }

    // Setter for isDashing
    public void SetIsDashing(int inputValue)
    {
        isDashing = inputValue != 0;
    }

    // Setter for flipX of the Player Sprite Renderer
    public void SetSpriteRendererFlipX(int inputValue)
    {
        character.spriteRenderer.flipX = inputValue != 0;
    }

    public override void PerformAttack()
    {
        character.animator.SetTrigger(animatorTriggerName);
        character.animator.SetBool("IsAttacking", true);
        character.SetStopAttacking(0);
    }

    public void ApplyDamage()
    {
        opponent.TakeDamage(damage);
        character.animator.SetBool("IsAttacking", false);
        character.animator.SetTrigger("TriggerDashBack");
        character.SetStopAttacking(1);
    }
}