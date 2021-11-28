
using UnityEngine;

public class EnemyShooterController : AbstractEnemyController
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
            // Rigidbody rb = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<Rigidbody>();
            // rb.AddForce(transform.forward * 32f, ForceMode.Impulse);
            // End of attack code

            Debug.Log("Attacking player...");
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }
}
