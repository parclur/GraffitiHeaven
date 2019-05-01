using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class EndEventTriggerScript : MonoBehaviour
{
    PlayableDirector PD;
    private void Awake()
    {
        PD = GetComponent<PlayableDirector>();
    }
    private void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag == "Diver")
        {
            PD.Play();
            Debug.Log("Fired");
        }
    }
}
