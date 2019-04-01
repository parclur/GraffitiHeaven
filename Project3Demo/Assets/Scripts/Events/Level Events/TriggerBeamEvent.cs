using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TriggerBeamEvent : MonoBehaviour, ITriggerable
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TriggerEvent(){
        Debug.Log("Event Triggered");
        foreach(Transform board in transform){
            if (board.gameObject.GetComponent<PlayableDirector>() != null){
                board.gameObject.GetComponent<PlayableDirector>().Play();
            }
        }
    }
}
