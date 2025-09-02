using UnityEngine;

public class WizardBlockAttackScript : AttackScript
{
    public override void PerformAttack()
    {
        character.SetIsBlocking(1);
        character.SetStopAttacking(1);
    }
}
