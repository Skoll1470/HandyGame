using UnityEngine;

public class EnemySpellScript : AttackScript
{
    public SpellScript spell;

    private void FixedUpdate()
    {
        if(numberOfTurns <=0 && !spell.gameObject.activeInHierarchy)
        {
            numberOfTurns = 2;
            character.SetStopAttacking(1);
        }
    }

    public override void PerformAttack()
    {
        if(numberOfTurns >= 2)
        {
            character.animator.SetTrigger(animatorTriggerName);
            numberOfTurns--;
        }
        else
        {
            spell.gameObject.SetActive(true);
            numberOfTurns--;
        }
    }

    public void ApplyDamageSpell()
    {
        opponent.TakeDamage(damage);
    }
}