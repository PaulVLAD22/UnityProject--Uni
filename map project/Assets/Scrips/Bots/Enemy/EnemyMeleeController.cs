using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMeleeController : AbstractEnemyController
{
    protected override void AttackAction()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().TakeDamage(damage);
        // ii adauga Stefan delay ca e prea asa
    }
}
