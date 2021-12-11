using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooter2Controller : AbstractEnemyController
{
    protected override void AttackPlayer()
    {
        // Make sure enemy doesn't move
        agent.SetDestination(transform.position);
        Debug.Log("Attacking player");
        transform.LookAt(player);

        if (!alreadyAttacked)
        {
            // Attack code here
            Rigidbody rb = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * 32f, ForceMode.Impulse);
            //GameObject.FindGameObjectWithTag("Player").GetComponent<ShootingAi>().TakeDamage(damage);
            // End of attack code

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }
}
