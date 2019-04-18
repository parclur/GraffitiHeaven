using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointScript : MonoBehaviour
{
    Transform diverSpawnPoint;
    Transform droneSpawnPoint;

    GameObject diverGameObject;
    GameObject droneGameObject;

    // Start is called before the first frame update
    private void Awake()
    {
        diverSpawnPoint = transform.Find("CheckpointDiverSpawn");
        Debug.Log(gameObject.name + " diver spawnpoint set to: " + diverSpawnPoint.position);
        droneSpawnPoint = transform.Find("CheckpointDroneSpawn");
        Debug.Log(gameObject.name + " drone spawnpoint set to: " + droneSpawnPoint.position);
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "diver")
        {
            CheckPointManager.instance.SetActiveCheckpoint(this.gameObject); //When the player enters the trigger set this as the active checkpoint
        }
    }


    public void SpawnPlayers(GameObject diver, GameObject drone)
    {
        diverGameObject = diver;
        droneGameObject = drone;
        diver.transform.position = diverSpawnPoint.position;
        drone.transform.position = droneSpawnPoint.position;

    }
}
