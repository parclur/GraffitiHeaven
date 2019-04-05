using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using UnityEngine.SceneManagement;

public class NextSceneOnKey : MonoBehaviour {

    Player diver;
    Player drone;
    [SerializeField] string nextScene;

    void Start(){
        diver = ReInput.players.GetPlayer("Diver");
        drone = ReInput.players.GetPlayer("Drone");
    }

    void Update(){
        if(diver.GetButton("Interact") || drone.GetButton("Interact")){
            SceneManager.LoadScene(nextScene);
        }
    }
}
