using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnKey : MonoBehaviour
{
    public GameObject keyInOppositeRoom;
    public bool keySpawned = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // If the key in the other room has been spawned, won't spawn another key
        if(keyInOppositeRoom.activeSelf)
        {
            keySpawned = true;
        }
    }

    public void TriggerEvent()
    {
        if (keySpawned == false)
        {
            // Spawns key in opposite room
            keyInOppositeRoom.SetActive(true);
            keySpawned = true;
        }
    }
}
