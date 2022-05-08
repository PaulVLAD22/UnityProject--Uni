using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

abstract public class AbstractEnemyState
{
    protected AbstractEnemyController enemyController = null;

    protected AbstractEnemyState(AbstractEnemyController enemyController) 
    {
        this.enemyController = enemyController;
    }

    public abstract void DoAction();
    public abstract void CheckStateChange();
}
