using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingAi : MonoBehaviour
{
    public int health;
    
    public void TakeDamage(int damage)
    {
        this.health -= damage;
        if(health<= 0 ) Destroy(gameObject);
    }
}
