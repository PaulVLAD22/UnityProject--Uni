using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Item : MonoBehaviour 
{
    public GameObject pickupEffect;
    string Name;
    public StatModifier mod;
    public PlayerStatsEnum stat;

    void OnTriggerEnter (Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Pickup(other);
        }
    }

    public void Pickup(Collider player)
    {
        Instantiate(pickupEffect, transform.position, transform.rotation);

        Equip(player.GetComponent<Player>());

        Destroy(gameObject);
    }

    public void Equip(Player p)
    {
        switch (stat) {
            case PlayerStatsEnum.BaseSpeed:
                p.BaseSpeed.AddModifier(mod);
                break;
            case PlayerStatsEnum.SprintSpeed:
                p.SprintSpeed.AddModifier(mod);
                break;
            case PlayerStatsEnum.JumpHeight:
                p.JumpHeight.AddModifier(mod);
                break;
            case PlayerStatsEnum.Health:
                p.Health.AddModifier(mod);
                break;
            case PlayerStatsEnum.MaxHealth:
                p.MaxHealth.AddModifier(mod);
                break;
            default:
                break;
        }

    }

    //public void Unequip(Player p)
    //{
    //    p.SprintSpeed.RemoveModifier(mod);
    //    p.SprintSpeed.AddModifier(mod);
    //}
}

public enum PlayerStatsEnum
{
        BaseSpeed, 
        SprintSpeed,
        JumpHeight,
        Health,
        MaxHealth
}
