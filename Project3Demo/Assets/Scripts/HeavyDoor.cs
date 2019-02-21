using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using Rewired;

public class HeavyDoor : MonoBehaviour
{
    [SerializeField] private Vector3 openPos;

    [SerializeField] private float moveTime;

    [SerializeField] private float keyDistance;

    public bool testForAll;

    private Vector3 startingPos;

    GameObject player;

    public List<GameObject> requiredKeys;

    bool isDoorOpen;

    Player rewiredPlayer;

    private void Start()
    {
        startingPos = gameObject.transform.position;
        openPos = startingPos + openPos;
        player = GameObject.FindGameObjectWithTag("Diver");
        rewiredPlayer = ReInput.players.GetPlayer("Diver");
    }

    public void Activate()
    {
        AudioManager.instance.PlayOneShot("MetalDoorOpen");
        StartCoroutine(MoveDoor());
    }

    private void Update()
    {
        KeyActivate();
    }

    public void KeyActivate(){
        if(Vector3.Distance(player.transform.position, transform.position) < keyDistance && !isDoorOpen){
            if(rewiredPlayer.GetButton("Interact")){
                bool doorOpen;
                if(testForAll){
                    doorOpen = testAllrequiredKeys(player.GetComponent<PlayerDiverMovement>().getKeyList());
                }
                else {
                    doorOpen = testSingleKey(player.GetComponent<PlayerDiverMovement>().getKeyList());
                }
                if(doorOpen){
                    AudioManager.instance.PlayOneShot("MetalDoorOpen");
                    StartCoroutine(MoveDoor());
                }
            }
        }
    }

    bool testSingleKey(List<GameObject> playerKeys) //Tests to see if the player has a single one of the required requiredKeys
    {
        foreach(GameObject testKey in playerKeys) //Passed in requiredKeys
        {
            foreach(GameObject requiredkey in requiredKeys) //requiredKeys assigned to door
            {
                if (testKey == requiredkey) //If there is a match open the door
                    return true;
            }
        }
        return false;

    }
    
    bool testAllrequiredKeys(List<GameObject> playerKeys) //Test to see if the player has ALL the required Keys
    {
        bool testBool = false;

        foreach (GameObject requiredkey in requiredKeys) //Passed in requiredKeys
        {
            foreach (GameObject testKey in playerKeys) //requiredKeys assigned to door
            {
                if(requiredkey == testKey)
                {
                    testBool = true;
                    break;
                }
            }
            if (testBool == false)
                return false;
            else
                testBool = false;
        }
        
        return true;
    }

    private IEnumerator MoveDoor()
    {
        isDoorOpen = true;
        float elapsedTime = 0.0f;
        while (elapsedTime < moveTime)
        {
            gameObject.transform.position = Vector3.Lerp(startingPos, openPos, (elapsedTime / moveTime));
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }
}