using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSwapAimNormal : MonoBehaviour {

    private CharacterAim_Base characterAimBase;
    private PlayerAim playerAim;

    private Player_Base playerBase;
    private PlayerPunch playerPunch;

    private void Awake() {
        characterAimBase = GetComponent<CharacterAim_Base>();
        playerAim = GetComponent<PlayerAim>();
        playerBase = GetComponent<Player_Base>();
        playerPunch = GetComponent<PlayerPunch>();

        playerBase.enabled = false;
        playerPunch.enabled = false;
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.T)) {
            //characterAimBase.SetVObjectEnabled(false);
            characterAimBase.enabled = false;
            playerAim.enabled = false;
            playerBase.enabled = true;
            playerPunch.enabled = true;
            //playerBase.RefreshBodySkeletonMesh();
        }
        
        if (Input.GetKeyDown(KeyCode.G)) {
            playerBase.enabled = false;
            playerPunch.enabled = false;
            //characterAimBase.SetVObjectEnabled(true);
            characterAimBase.enabled = true;
            //characterAimBase.RefreshBodySkeletonMesh();
            playerAim.enabled = true;
        }
    }

}