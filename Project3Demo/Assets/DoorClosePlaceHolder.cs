using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorClosePlaceHolder : MonoBehaviour, ITriggerable
{

    [SerializeField] private GameObject door;
    [SerializeField] private float closedMoveTime;
    [SerializeField] private float delay;

    private Vector3 startingPos;
    private Vector3 startingRot;


    // Start is called before the first frame update
    void Start()
    {
        startingRot = door.transform.eulerAngles;
        startingPos = door.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TriggerEvent(){
        if (door.GetComponent<HeavyDoor>().doorOpen == true){
            door.GetComponent<HeavyDoor>().doorOpen = false;
            door.GetComponent<HeavyDoor>().stopOpening = true;
            StartCoroutine(CloseDoor());
        }
    }

    private IEnumerator CloseDoor(){

        yield return new WaitForSeconds(delay);
        Debug.Log("Go");
        float newElapsedTime = 0.0f;
        Vector3 newStartingPos = door.transform.position;
        Vector3 newStartingRot = door.transform.eulerAngles;
        while(newElapsedTime < closedMoveTime){
                door.transform.position = Vector3.Lerp(newStartingPos, startingPos, (newElapsedTime / closedMoveTime));
                door.transform.eulerAngles = new Vector3(
                    Mathf.LerpAngle(newStartingRot.x, startingRot.x, (newElapsedTime / closedMoveTime)), 
                    Mathf.LerpAngle(newStartingRot.y, startingRot.y, (newElapsedTime / closedMoveTime)),
                    Mathf.LerpAngle(newStartingRot.z, startingRot.z, (newElapsedTime / closedMoveTime)));
                newElapsedTime += Time.deltaTime;
                    yield return new WaitForEndOfFrame();
                    }
                    door.GetComponent<HeavyDoor>().stopOpening = false;

    }

}
