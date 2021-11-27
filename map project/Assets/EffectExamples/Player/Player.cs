using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerStat BaseSpeed = new PlayerStat(12f);
    public PlayerStat SprintSpeed = new PlayerStat(20f);
    public PlayerStat JumpHeight = new PlayerStat(3f);
    public PlayerStat Health = new PlayerStat(100f);
    public PlayerStat MaxHealth = new PlayerStat(100f);

}
