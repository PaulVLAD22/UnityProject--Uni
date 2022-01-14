using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooterController :AbstractEnemyController
{
    public AudioSource attackSound;
    public float timeOfAnimation;
    public ParticleSystem muzzleFlash;

    protected void CheckOrientationAndDoDamage()
    {
        muzzleFlash.Play();
        attackSound.Play(0);
        if (PlayerInFieldOfView()) {
            GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().TakeDamage(damage);
        }
    }

    protected override void AttackAction()
    {
        Invoke(nameof(CheckOrientationAndDoDamage), timeOfAnimation);
    }
}
