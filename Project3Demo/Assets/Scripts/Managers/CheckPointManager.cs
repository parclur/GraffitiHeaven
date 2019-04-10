using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointManager : MonoBehaviour
{
    [HideInInspector] public static CheckPointManager instance;

    GameObject activeCheckpoint; //The active checkpoint in the level
    CheckpointScript activeCheckpointScript;
    string activeCheckpointName;


    [SerializeField] GameObject startingCheckpoint; //The first checkpoint in the level
    [SerializeField] GameObject diver;
    [SerializeField] GameObject drone;

    private void Awake()
    {
        DontDestroyOnLoad(this); //Sets the manager to not destroy on load
        instance = this;
        SetActiveCheckpoint(startingCheckpoint);
    }

    //private void OnLevelWasLoaded(int level)
    //{
    //    Debug.Log("Moving to checkpoint from load level");

    //    GameObject activeCheckpoint = GameObject.Find(activeCheckpointName); //Finds the active checkpoint
    //    SetActiveCheckpoint(activeCheckpoint);

    //    SpawnAtActiveCheckpoint();
    //}

    private void Start()
    {
        Debug.Log("Moving to checkpoint from start");
        GameObject activeCheckpoint = GameObject.Find(activeCheckpointName); //Finds the active checkpoint
        SetActiveCheckpoint(activeCheckpoint);

        SpawnAtActiveCheckpoint();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            SpawnAtActiveCheckpoint();
        }
    }

    public void SetActiveCheckpointToStartingCheckpoint()
    {
        SetActiveCheckpoint(startingCheckpoint);
    }

    public void SpawnAtActiveCheckpoint() //Will call the spawn players function on the active checkpoint
    {
        Debug.Log("Movign player to checkpoint: " + activeCheckpoint.name + " at position: " + activeCheckpoint.transform.position);
        Debug.Log("Diver current transform: " + diver.transform.position + "; Drone current transform: " + drone.transform.position);
        for (int i = 0; i < 4; i++)
            activeCheckpointScript.SpawnPlayers(diver, drone);
    }

    public void SetActiveCheckpoint(GameObject checkpoint) //Sets the active checkpoint
    {
        Debug.Log("Setting active checkpoint to checkpoing: " + checkpoint.name);
        activeCheckpoint = checkpoint;
        Debug.Log("Extracting script from checkpoint...");
        activeCheckpointScript = activeCheckpoint.GetComponent<CheckpointScript>();
        Debug.Log("Script: " + activeCheckpointScript.name + " extracted.");
        activeCheckpointName = activeCheckpoint.name;
        Debug.Log("Name: " + activeCheckpointName);

    }
}
