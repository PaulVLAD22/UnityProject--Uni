using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrolAreaEnemyState : AbstractEnemyState
{
    public PatrolAreaEnemyState(AbstractEnemyController enemyController) : base(enemyController) {}

    public override void DoAction() 
    {
        this.enemyController.PatrolArea();
    }

    public override void CheckStateChange() 
    {
        if (this.enemyController.PlayerInAttackRange()) {
            Debug.Log("Changed state from patrol area to attack player");
            this.enemyController.ChangeState(new AttackPlayerEnemyState(this.enemyController));
        } else {
            if (this.enemyController.PlayerInSightRange() && this.enemyController.PlayerInFieldOfView()) {
                Debug.Log("Changed state from patrol area to chase player");
                this.enemyController.ChangeState(new ChasePlayerEnemyState(this.enemyController));
            }
        }
    }
}
