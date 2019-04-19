using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.SceneManagement;

public class CheckPointManager : MonoBehaviour
{
    [HideInInspector] public static CheckPointManager instance;

    GameObject activeCheckpoint; //The active checkpoint in the level
    CheckpointScript activeCheckpointScript;
    string activeCheckpointName;
    string checkpointKey = "ActiveCheckpoint";


    [SerializeField] GameObject diver; //NEED TO FIND DIVER AND DRONE
    [SerializeField] GameObject drone;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        Debug.Log("Moving to checkpoint from start");
        SetActiveCheckpoint(PlayerPrefs.GetString(checkpointKey)); //Gets the active checkpoint from playerprefs and sets it

        SpawnAtActiveCheckpoint();

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
        for (int i = 0; i < 4; i++)
            activeCheckpointScript.SpawnPlayers(diver, drone);
    }

    public void SetActiveCheckpoint(string checkpointName) //Sets the active checkpoint
    {
        GameObject activeCheckpoint = GameObject.Find(checkpointName); //Finds the active checkpoint
        SetActiveCheckpoint(activeCheckpoint);

    }

    public void SetActiveCheckpoint(GameObject checkpoint) //Sets the active checkpoint
    {
        //Debug.Log("Setting active checkpoint to checkpoint: " + checkpoint.name);

        activeCheckpoint = checkpoint; //Sets the active checkpoing to the passed checkpoint
        //Debug.Log("Extracting script from checkpoint...");

        activeCheckpointScript = activeCheckpoint.GetComponent<CheckpointScript>(); //Gets the script of the active checkpoint
        //Debug.Log("Script: " + activeCheckpointScript.name + " extracted.");

        activeCheckpointName = activeCheckpoint.name; //Gets the name of the active checkpoint
        //Debug.Log("Name: " + activeCheckpointName);

        PlayerPrefs.SetString(checkpointKey, activeCheckpointName); //Sends the name to player prefs

    }


}
