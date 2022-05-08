using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooterController :AbstractEnemyController
{
    public const int FORCE = 9000; // over 9000!

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
        tempBullet.GetComponent<Rigidbody>().AddForce(tempBullet.transform.forward * FORCE);
    }

    public override void AttackAction()
    {
        Invoke(nameof(CheckOrientationAndDoDamage), timeOfAnimation);
    }
}
