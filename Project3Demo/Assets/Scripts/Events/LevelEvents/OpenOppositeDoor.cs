using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenOppositeDoor : MonoBehaviour
{
    public GameObject oppositeDoor;
    public GameObject oppositeDoor2;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // When the player enters the room filled with lamprey eggs, the door for the opposite room opens
    public void TriggerEvent()
    {
        if(!oppositeDoor.GetComponent<HeavyDoor>().doorOpen)
        {
            // Open door
            oppositeDoor.GetComponent<HeavyDoor>().Activate();
            oppositeDoor2.GetComponent<HeavyDoor>().Activate();
        }
    }
}
