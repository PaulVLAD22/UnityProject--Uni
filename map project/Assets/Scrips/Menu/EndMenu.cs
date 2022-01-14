using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EndMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        var playerScore  =  PlayerPrefs.GetInt("score");
        GetComponent<TextMeshProUGUI>().SetText("SCORE: "+ playerScore);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
