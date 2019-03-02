using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using Rewired;

public class HeavyDoor : MonoBehaviour {

    [SerializeField] private Vector3 openPos;

    //This is in euler angles - Set to either -90 or 90 for best results. If you want a fully opening door try something around 175 (or -175)
    [SerializeField] private Vector3 openRot;

    [SerializeField] private float moveTime;

    [SerializeField] private float closedMoveTime;

    [SerializeField] private float flareStunTime;

    [SerializeField] private float keyDistance;

    [SerializeField] private bool automaticDoor;

    [SerializeField] private bool flareClosesDoor;

    [SerializeField] private bool flareStunsDoor;

    public bool testForAll;

    private Vector3 startingPos;

    private Vector3 startingRot;

    private GameObject player;

    public List<GameObject> requiredKeys;

    private bool isDoorOpen;

    private Player rewiredPlayer;

    public bool stopOpening;

    private bool flareHit;

    private bool inFlareCoroutine;

    private void Start()
    {
        startingRot = gameObject.transform.eulerAngles;
        startingPos = gameObject.transform.position;
        openPos = startingPos + openPos;
        openRot = startingRot + openRot;
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
        if(automaticDoor){
            if(!isDoorOpen){
                Activate();
            }
            if(flareHit && !inFlareCoroutine){
                StartCoroutine(FlareStun());
            }
        }
        else {
            KeyActivate();
        }
    }

    public void KeyActivate()
    {

        // NOTE: This is really inefficient at large scale, perhaps use a trigger zone?
        if (Vector3.Distance(player.transform.position, transform.position) < keyDistance && !isDoorOpen)
        {
            if (rewiredPlayer.GetButton("Interact"))
            {
                bool doorOpen;
                if (testForAll)
                {
                    doorOpen = TestAllRequiredKeys(player.GetComponent<PlayerDiverMovement>().getKeyList());
                }
                else
                {
                    doorOpen = TestSingleKey(player.GetComponent<PlayerDiverMovement>().getKeyList());
                }

                if (doorOpen)
                {              
                    AudioManager.instance.PlayOneShot("MetalDoorOpen");
                    StartCoroutine(MoveDoor());
                }
            }
        }
    }

    bool TestSingleKey(List<GameObject> playerKeys) //Tests to see if the player has a single one of the required requiredKeys
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
    
    bool TestAllRequiredKeys(List<GameObject> playerKeys) //Test to see if the player has ALL the required Keys
    {
        bool testBool = false;

        foreach (GameObject requiredkey in requiredKeys) //Passed in requiredKeys
        {
            foreach (GameObject testKey in playerKeys) //requiredKeys assigned to door
            {
                if (requiredkey == testKey)
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
            if(flareHit){
                yield return new WaitForEndOfFrame();
            }
            if(automaticDoor && stopOpening && !flareStunsDoor && !flareClosesDoor){
                yield return new WaitForEndOfFrame();
            }
            else if(automaticDoor && flareHit && flareClosesDoor){
                //Play the sound byte of the door closing here
                float newElapsedTime = 0.0f;
                Vector3 newStartingPos = transform.position;
                Vector3 newStartingRot = transform.eulerAngles;
                while(newElapsedTime < closedMoveTime){
                    gameObject.transform.position = Vector3.Lerp(newStartingPos, startingPos, (newElapsedTime / closedMoveTime));
                    transform.eulerAngles = new Vector3(
                        Mathf.LerpAngle(newStartingRot.x, startingRot.x, (elapsedTime / closedMoveTime)), 
                        Mathf.LerpAngle(newStartingRot.y, startingRot.y, (elapsedTime / closedMoveTime)),
                        Mathf.LerpAngle(newStartingRot.z, startingRot.z, (elapsedTime / closedMoveTime)));
                    newElapsedTime += Time.deltaTime;
                    yield return new WaitForEndOfFrame();
                }
                elapsedTime = moveTime + 1;
            }
            else {
                gameObject.transform.position = Vector3.Lerp(startingPos, openPos, (elapsedTime / moveTime));
                transform.eulerAngles = new Vector3(
                    Mathf.LerpAngle(startingRot.x, openRot.x, (elapsedTime / moveTime)), 
                    Mathf.LerpAngle(startingRot.y, openRot.y, (elapsedTime / moveTime)),
                    Mathf.LerpAngle(startingRot.z, openRot.z, (elapsedTime / moveTime)));
                elapsedTime += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
            
        }
    }

    private IEnumerator FlareStun(){
        inFlareCoroutine = true;
        float elapsedTime = 0.0f;
        while (elapsedTime < flareStunTime)
        {
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        inFlareCoroutine = false;
    }

    void OnCollisionEnter(Collision col){
        if(flareStunsDoor || flareClosesDoor){
            flareHit = true;
        }
    }
}