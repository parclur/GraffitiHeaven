using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialLampreyEvent : MonoBehaviour
{
    public GameObject door;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    // When the player enters the triggerzone, the door starts to open
    public void TriggerEvent()
    {
        if (!door.GetComponent<HeavyDoor>().doorOpen)
        {
            // Open door
            door.GetComponent<HeavyDoor>().Activate();
        }
    }
}
