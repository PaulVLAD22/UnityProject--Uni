using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingAi : MonoBehaviour
{
    int health = 1000;
    public void TakeDamage(int damage)
    {
        health -= damage;
        if(health<= 0 ) Destroy(gameObject);
    }
}
