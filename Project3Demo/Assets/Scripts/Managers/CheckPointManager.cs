using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointManager : MonoBehaviour
{
    [HideInInspector] public static CheckPointManager instance;

    GameObject activeCheckpoint; //The active checkpoint in the level
    CheckpointScript activeCheckpointScript;

    [SerializeField] GameObject startingCheckpoint; //The first checkpoint in the level
    [SerializeField] GameObject diver;
    [SerializeField] GameObject drone;

    private void Awake()
    {
        DontDestroyOnLoad(this); //Sets the manager to not destroy on load
        instance = this;
    }

    private void OnLevelWasLoaded(int level)
    {
        Debug.Log("Activating!");
        SpawnAtActiveCheckpoint();
    }

    private void Start()
    {
        SetActiveCheckpoint(startingCheckpoint);
        //SpawnAtActiveCheckpoint();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            SpawnAtActiveCheckpoint();
        }
    }

    public void SpawnAtActiveCheckpoint() //Will call the spawn players function on the active checkpoint
    {
        Debug.Log("Movign player to checkpoint: " + activeCheckpoint.name + " at position: " + activeCheckpoint.transform.position);
        Debug.Log("Diver current transform: " + diver.transform.position + "; Drone current transform: " + drone.transform.position);
        activeCheckpointScript.SpawnPlayers(diver, drone);
    }

    public void SetActiveCheckpoint(GameObject checkpoint) //Sets the active checkpoint
    {
        Debug.Log("Setting active checkpoint to checkpoing: " + checkpoint.name);
        activeCheckpoint = checkpoint;
        Debug.Log("Extracting script from checkpoint...");
        activeCheckpointScript = activeCheckpoint.GetComponent<CheckpointScript>();
        Debug.Log("Script: " + activeCheckpointScript.name + " extracted.");
    }
}
