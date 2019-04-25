using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelectManager : MonoBehaviour
{
    [HideInInspector] public static LevelSelectManager instance;

    [SerializeField] string level1StartingCheckpoint;
    [SerializeField] string level2StartingCheckpoint;
    [SerializeField] string level3StartingCheckpoint;
    [SerializeField] string level4StartingCheckpoint;
    [SerializeField] string level5StartingCheckpoint;

    string checkpointKey = "ActiveCheckpoint";

    public void Awake()
    {
        instance = this;
    }

    public void NewGame()
    {
        PlayerPrefs.SetString(checkpointKey, level1StartingCheckpoint); //Loads the first level
        SceneLoader.instance.NextScene();
    }

    public void LoadGame()
    {
        //no need to set a player pref here as it will just load from the last one
        SceneLoader.instance.LoadGame();
    }

    public void LoadLevel1()
    {
        PlayerPrefs.SetString(checkpointKey, level1StartingCheckpoint);
        SceneLoader.instance.LoadGame();
    }

    public void LoadLevel2()
    {
        PlayerPrefs.SetString(checkpointKey, level2StartingCheckpoint);
        SceneLoader.instance.LoadGame();
    }

    public void LoadLevel3()
    {
        PlayerPrefs.SetString(checkpointKey, level3StartingCheckpoint);
        SceneLoader.instance.LoadGame();
    }

    public void LoadLevel4()
    {
        PlayerPrefs.SetString(checkpointKey, level4StartingCheckpoint);
        SceneLoader.instance.LoadGame();
    }

    public void LoadLevel5()
    {
        PlayerPrefs.SetString(checkpointKey, level5StartingCheckpoint);
        SceneLoader.instance.LoadGame();
    }


}
