﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LampreyEvent : MonoBehaviour, ITriggerable
{
    [SerializeField] private GameObject[] Lamprey;
    [SerializeField] private float spawnTime;

    [SerializeField] private bool openLampreyDoor;
    [SerializeField] private GameObject door;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TriggerEvent(){
        
        if (openLampreyDoor == true) {
            StartCoroutine(SpawnLamprey());
            door.GetComponent<HeavyDoor>().Activate();
        }
    }
    

    private IEnumerator SpawnLamprey(){
        yield return new WaitForSeconds(spawnTime);
        foreach(GameObject i in Lamprey){
            if (i.activeSelf == false){
                i.SetActive(true);
            } else {
                Destroy(i);
            }
        }
    }
}