using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using UnityEngine.SceneManagement;

public class ClickThroughPapers : MonoBehaviour {

    [SerializeField] GameObject[] papers;
    int paperIndex;

    Player rewiredDiver;
    Player rewiredDrone;

    void Start(){
        rewiredDiver = ReInput.players.GetPlayer("Diver");
        rewiredDrone = ReInput.players.GetPlayer("Drone");
    }

    void Update(){
        if(rewiredDiver.GetButtonDown("Interact") || rewiredDrone.GetButtonDown("Interact")){
            paperIndex++;
            StartCoroutine(UpdatePapers());
        }
    }

    IEnumerator UpdatePapers(){
        if(paperIndex >= papers.Length){
            SceneManager.LoadScene("master-level-dev");
        }
        for(int i = 0; i < papers.Length; i++){
            if(i > paperIndex) papers[i].SetActive(false);
            if(i == paperIndex){
                papers[i].SetActive(true);
                float time = 0;
                while(time < 1){
                    papers[i].transform.localScale = Vector3.Lerp(new Vector3(1, 1, 1), new Vector3(.71f, .71f, .71f), time);
                    time += Time.deltaTime;
                    yield return null;
                }
            }
        }
    }
}
