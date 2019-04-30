using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LampreyEvent : MonoBehaviour, ITriggerable
{
    [SerializeField] private GameObject[] Lamprey;
    [SerializeField] private float spawnTime;

    [SerializeField] private bool openLampreyDoor;
    [SerializeField] private bool deleteLamprey;
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
        if (openLampreyDoor == true){
            StartCoroutine(SpawnLamprey());
            door.GetComponent<HeavyDoor>().Activate();
            AudioManager.instance.PlayOneShot("MetalDoorUnlocked");
            AudioManager.instance.PlayOneShot("MetalDoorOpen", 0.5f, 1.25f);
        }
    }
    

    private IEnumerator SpawnLamprey(){
        if(spawnTime != -1){
            yield return new WaitForSeconds(spawnTime);
            foreach(GameObject i in Lamprey){
                if (i.activeSelf == false){
                    i.SetActive(true);
                } else if(deleteLamprey){
                    Destroy(i);
                }
            }
        }
        
    }
}
