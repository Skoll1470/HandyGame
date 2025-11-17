using UnityEngine;

public class BasicAttack : AttackScript
{
    // Vector beetween the Enemy and the Player
    private Vector2 distanceToEnemy;

    // Boolean indicating if the Player is playing a Dashing animation
    private bool isDashing = false;

    private bool isDashingBack = false;

    private float startingDistanceToEnemy = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UpdateDistanceToEnemy();
        startingDistanceToEnemy = distanceToEnemy.x;
        Debug.Log(startingDistanceToEnemy);
    }

    private void FixedUpdate()
    {
        //character.rigidBody.linearVelocityX = (isDashing ? character.moveSpeed : 0f) * (character.spriteRenderer.flipX ? -1f : 1f);
        UpdateDistanceToEnemy();

        if (distanceToEnemy.x <= 0.7f && isDashing && !isDashingBack)
        {
            character.animator.SetTrigger(animatorTriggerName);
            isDashing = false;
        }
        else if (distanceToEnemy.x >= startingDistanceToEnemy && isDashing && isDashingBack)
        {
            isDashing = false;
            isDashingBack = false;
        }
        character.rigidBody.linearVelocityX = (isDashing ? character.moveSpeed : 0f) * (isDashingBack ? -1f : 1f);
        character.animator.SetBool("IsMoving", isDashing);
        character.spriteRenderer.flipX = isDashingBack;
    }

    void UpdateDistanceToEnemy()
    {
        distanceToEnemy.x = Mathf.Abs(opponent.transform.position.x - character.transform.position.x);
    }

    // Setter for isDashing
    public void SetIsDashing(int inputValue)
    {
        isDashing = inputValue != 0;
    }

    public void SetIsDashingBack(int inputValue)
    {
        isDashingBack = inputValue != 0;
    }

    // Setter for flipX of the Player Sprite Renderer
    public void SetSpriteRendererFlipX(int inputValue)
    {
        character.spriteRenderer.flipX = inputValue != 0;
    }

    public override void PerformAttack()
    {
        //character.animator.SetTrigger(animatorTriggerName);
        //character.animator.SetBool("IsAttacking", true);
        isDashing = true;
        character.SetStopAttacking(0);
    }

    public void ApplyDamage()
    {
        opponent.TakeDamage(damage);
        character.SetStopAttacking(1);
    }
}