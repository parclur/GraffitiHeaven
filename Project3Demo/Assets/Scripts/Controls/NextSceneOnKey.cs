using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using UnityEngine.SceneManagement;

public class NextSceneOnKey : MonoBehaviour {

    Player player;
    [SerializeField] string nextScene;

    void Start(){
        ReInput.players.GetPlayer("Diver");
    }

    void Update(){
        if(player.GetButton("Interact")){
            SceneManager.LoadScene(nextScene);
        }
    }
}
