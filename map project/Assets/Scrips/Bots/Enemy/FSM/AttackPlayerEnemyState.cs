using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AttackPlayerEnemyState : AbstractEnemyState
{
    public AttackPlayerEnemyState(AbstractEnemyController enemyController) : base(enemyController) {}

    public override void DoAction() 
    {
        this.enemyController.AttackPlayer();
    }

    public override void CheckStateChange() 
    {
        if (!this.enemyController.PlayerInAttackRange()) {
            if (this.enemyController.PlayerInSightRange() && this.enemyController.PlayerInFieldOfView()) {
                Debug.Log("Changed state from attack player to chase player");
                this.enemyController.ChangeState(new ChasePlayerEnemyState(this.enemyController));
            } else {
                Debug.Log("Changed state from attack player to patrol area");
                this.enemyController.ChangeState(new PatrolAreaEnemyState(this.enemyController));
            }
        }
    }
}
