using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeProxEvent : MonoBehaviour, ITriggerable
{
    [SerializeField] private PlayerROVMovement controller;
    [SerializeField] private GameObject diver;
    [SerializeField] private GameObject drone;


    private Transform droneStartPos;
    [SerializeField] private Transform droneTargetPos;
    float travelDistance;
    
    [SerializeField] private float wait;
    float startTime;
    [SerializeField] private float lerpTime;

    bool movingCam = false;

    [SerializeField] private Transform diverSnapPosition;
    [SerializeField] private GameObject[] loadTargets;
    [SerializeField] private GameObject[] loadLamprey;

    [SerializeField] private bool toggleDiverControl;
    [SerializeField] private bool toggleDroneControl;
    [SerializeField] private bool triggerLamprey;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(movingCam == true){
            float dist = (Time.time - startTime) * lerpTime;
            float frac = dist/travelDistance;
            drone.transform.position = Vector3.Lerp(drone.transform.position, droneTargetPos.position, frac);
            controller.enabled = false;
            if((Vector3.Distance(drone.transform.position, droneTargetPos.position) <= 0.01f)){
                movingCam = false;
                Debug.Log("euhhh?");
                //controller.enabled = true;
                if (loadTargets[0] != null){
                    LoadEach();
                }

                if(diverSnapPosition != null){
                    SnapDiver();
                }
                if(toggleDiverControl == true){
                    DiverControl();
                }
                if(toggleDroneControl == true){
                    DroneControl();
                }
                if(triggerLamprey == true){
                    Lamprey();
                }

                Destroy(gameObject);
            }
        }
    }

     public void TriggerEvent(){
         //diver.GetComponent<PlayerDiverMovement>().climbing = true;
         

        StartCoroutine(MoveDrone(wait));
        

         //lerpTime = travelDistance/lerpTime * Time.deltaTime;


         //drone.transform.position = Vector3.Lerp(drone.transform.position, droneTargetPos.position, lerpTime);

     }

     private void LoadEach(){
         foreach(GameObject i in loadTargets){
             i.SetActive(true);
         }
     }

    private void DiverControl(){
         
     }
    private void DroneControl(){
         
     }
     private void SnapDiver(){
         diver.GetComponent<CharacterController>().enabled = false;
         diver.transform.position = diverSnapPosition.position;
         diver.transform.rotation = diverSnapPosition.rotation;
     }
     private void Lamprey(){
         foreach(GameObject i in loadLamprey){
             i.SetActive(true);
         }
     }

     private IEnumerator MoveDrone(float waitTime){
         yield return new WaitForSeconds(waitTime);
         controller.enabled = false;
         droneStartPos = drone.transform;
         startTime = Time.time;
         travelDistance = Vector3.Distance(droneStartPos.position, droneTargetPos.position);
         movingCam = true;
     }
}
