using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooterController :AbstractEnemyController
{
    public AudioSource attackSound;
    public float timeOfAnimation;
    public ParticleSystem muzzleFlash;
    public GameObject bullet;

    protected void CheckOrientationAndDoDamage()
    {
        muzzleFlash.Play();
        attackSound.Play(0);
        Quaternion fireRotation = Quaternion.LookRotation(transform.forward);
        GameObject tempBullet = Instantiate(bullet, new Vector3(gameObject.transform.position.x,gameObject.transform.position.y+1,gameObject.transform.position.z), fireRotation);
        tempBullet.GetComponent<Rigidbody>().AddForce(tempBullet.transform.forward * 900);
         // if (PlayerInFieldOfView()) {
        //     GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().TakeDamage(damage);
        // }
    }

    protected override void AttackAction()
    {
        Invoke(nameof(CheckOrientationAndDoDamage), timeOfAnimation);
    }
}
