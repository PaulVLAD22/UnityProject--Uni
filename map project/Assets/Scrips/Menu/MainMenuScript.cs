using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class MainMenuScript : MonoBehaviour
{

    private HighscoreManager highscoreManager;
    private GameObject leaderboard;
    // Start is called before the first frame update
    void Start()
    {
        highscoreManager = HighscoreManager.instance;
        //leaderboard = Canvas.transform.GetChild(2).transform.GetChild(2).transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).gameObject;
        leaderboard = GameObject.FindGameObjectsWithTag("Leaderboard")[0];
    }

    // Update is called once per frame
    void Update()
    {
        var scores = String.Join("\n", highscoreManager.leaderboard.list);
        leaderboard.GetComponent<TextMeshProUGUI>().SetText(scores);
    }

    public void DisplayLeaderboard()
    {
        var scores = String.Join("\n", highscoreManager.leaderboard.list);
        leaderboard.GetComponent<TextMeshProUGUI>().SetText(scores);
    }
}
