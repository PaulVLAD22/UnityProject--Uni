using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooterController :AbstractEnemyController
{
    public GameObject projectile;

    protected override void AttackAction()
    {
        float delay = 2.0f;
        
        GameObject bullet = Instantiate(projectile, new Vector3(transform.position.x,transform.position.y,transform.position.z), Quaternion.identity);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * 88f, ForceMode.Impulse);
        rb.AddForce(transform.up * 8f, ForceMode.Impulse);
        Destroy(bullet, delay);

    }
}
