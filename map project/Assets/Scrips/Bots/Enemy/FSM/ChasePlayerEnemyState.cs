using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChasePlayerEnemyState : AbstractEnemyState
{
    public ChasePlayerEnemyState(AbstractEnemyController enemyController) : base(enemyController) {}

    public override void DoAction() 
    {
        this.enemyController.ChasePlayer();
    }

    public override void CheckStateChange() 
    {
        if (this.enemyController.PlayerInAttackRange()) {
            this.enemyController.ChangeState(new AttackPlayerEnemyState(this.enemyController));
            Debug.Log("Changed state from chase player to attack player");
        } else {
            if (!this.enemyController.PlayerInSightRange() || !this.enemyController.PlayerInFieldOfView()) {
                this.enemyController.ChangeState(new PatrolAreaEnemyState(this.enemyController));
                Debug.Log("Changed state from chase player to patrol area");
            }
        }
    }
}
