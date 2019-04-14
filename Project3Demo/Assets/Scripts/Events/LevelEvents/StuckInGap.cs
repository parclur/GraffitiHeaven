using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StuckInGap : MonoBehaviour
{
    public Transform lamprey;
    public GameObject lampreySpawnPoint;

    public void TriggerEvent()
    {
        // Spawn lamprey to kill them and then restarts at checkpoint
        Instantiate(lamprey, lampreySpawnPoint.transform.position, Quaternion.identity);
    }
}