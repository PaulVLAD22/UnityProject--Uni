using System.Collections;
using System.Collections.Generic;
using TMPro;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public HighscoreManager highscoreManager;
    private GameObject leaderboard;

    // Start is called before the first frame update
    void Start()
    {
        //highscoreManager = HighscoreManager.instance;
        highscoreManager = new HighscoreManager();
        //leaderboard = Canvas.transform.GetChild(2).transform.GetChild(2).transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).gameObject;
        //leaderboard = GameObject.FindGameObjectsWithTag("Leaderboard")[0];
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
        Debug.Log("QUIT");
        Application.Quit();
    }

    public void LoadLeaderboard()
    {
        
        leaderboard = GameObject.FindGameObjectsWithTag("Leaderboard")[0];
        var scores = String.Join("\n", highscoreManager.LoadScores());
        leaderboard.GetComponent<TextMeshProUGUI>().SetText(scores);
    }
}
