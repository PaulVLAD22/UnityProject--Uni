using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMeleeController : AbstractEnemyController
{
    public AudioSource attackSound;
    public float timeOfAnimation;

    protected void CheckCollisionAndDoDamage()
    {
        attackSound.Play(0);
        if (PlayerInAttackRange()) {
            GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().TakeDamage(damage);
        }
    }

    public override void AttackAction()
    {
        Invoke(nameof(CheckCollisionAndDoDamage), timeOfAnimation);
    }
}
