using UnityEngine;

public class EnemyBasicAttackScript : AttackScript 
{

    private Vector2 distanceToPlayer;

    private bool isMoving = false;

    private bool isWalkingBack = false;

    private float startingDistanceToPlayer = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UpdateDistanceToPlayer();
        startingDistanceToPlayer = distanceToPlayer.x;
    }

    private void FixedUpdate()
    {
        UpdateDistanceToPlayer();
        if(distanceToPlayer.x <= 2f && isMoving && !isWalkingBack)
        {
            character.animator.SetTrigger(animatorTriggerName);
            isMoving = false;
        }
        else if(distanceToPlayer.x >= startingDistanceToPlayer && isMoving && isWalkingBack)
        {
            isMoving = false;
            isWalkingBack = false;
        }
        character.rigidBody.linearVelocityX = (isMoving ? character.moveSpeed : 0f) * (isWalkingBack ? 1f : -1f);
        character.animator.SetBool("IsMoving", isMoving);
    }

    public override void PerformAttack()
    {
        isMoving = true;
    }

    public void ApplyDamage()
    {
        opponent.TakeDamage(damage);
    }
    void UpdateDistanceToPlayer()
    {
        distanceToPlayer.x = Mathf.Abs(opponent.transform.position.x - character.transform.position.x);
    }

    public void SetIsMoving(int inputValue)
    {
        isMoving = inputValue != 0;
    }

    public void SetIsWalkingBack(int inputValue)
    {
        isWalkingBack = inputValue != 0;
    }

    public void SetSpriteRendererFlipX(int inputValue)
    {
        character.spriteRenderer.flipX = inputValue != 0;
    }
}