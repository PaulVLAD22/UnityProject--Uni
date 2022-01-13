using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public PlayerStat Health = new PlayerStat(100f, 1000f);

    public PlayerStat BaseSpeed = new PlayerStat(6f, 10f);
    public PlayerStat SprintSpeed = new PlayerStat(12f, 20f);
    public PlayerStat JumpHeight = new PlayerStat(2f, 10f);
    public PlayerStat MaxHealth = new PlayerStat(100f, 1000f);
    public PlayerStat DamageReduction = new PlayerStat(0f, 99f);
    public int KillCount = 0;

    private HighscoreManager highscoreManager = new HighscoreManager();

    public void TakeDamage(float damage)
    {
        this.Health.AddValue(new StatModifier
        {
            Value = CalculateReducedDamage(damage) * -1,
            Type = StatModType.Flat
        });
        CheckHealth();
    }


    public void TakeCustomDamage(StatModifier statMod)
    {
        statMod.Value = CalculateReducedDamage(statMod.Value);
        this.Health.AddValue(statMod);
        CheckHealth();
    }

    public void CheckHealth()
    {
        if (this.Health.CustomValue <= 0)
        {
            EndGame();
        }else if(this.Health.CustomValue >= this.MaxHealth.Value)
        {
            this.Health.CustomValue = this.MaxHealth.Value;
        }
        Debug.Log($"Player Health - {this.Health.CustomValue}");
    }

    public float CalculateReducedDamage(float damage)
    {
        return damage * (1 - DamageReduction.Value / 100);
    }

    private void EndGame()
    {
        Debug.Log("Game Ended");
        highscoreManager.AddScore(KillCount);
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        //SceneManager.LoadScene(0);
    }

    GameObject Canvas;
    GameObject NumberOfKills;
    public ProgressBar Pb;

    private void Start()
    {
        Canvas = GameObject.Find("Canvas");
        NumberOfKills = Canvas.transform.GetChild(2).gameObject;
        highscoreManager = new HighscoreManager();
        highscoreManager.LoadScores();
    }

    private void Update()
    {
        Pb.BarValue = CalculateHealth();
        Pb.barValuev2 = Health.CustomValue + " / " + MaxHealth.Value;
    }

    public void IncrementKillCount()
    {
        KillCount += 1;
        NumberOfKills.GetComponent<TextMeshProUGUI>().SetText(": "+ KillCount);
        Debug.Log($"Current kill count:{KillCount}");
    }

    private float CalculateHealth()
    {
        return Health.CustomValue / MaxHealth.Value * 100;
    }
}
