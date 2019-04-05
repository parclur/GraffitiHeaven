using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControllerCollisions : MonoBehaviour
{
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider.tag == "Key")
        {
            GetComponent<PlayerDiverMovement>().addKey(hit.gameObject);
            hit.gameObject.SetActive(false);
        }
        if(hit.collider.tag == "Lamprey")
        {
            SceneLoader.instance.LoseGame();
            
        }
        if(hit.collider.tag == "AnglerFish")
        {
            GetComponent<PlayerDiverMovement>().ApplySlow();
        }
        if(hit.collider.tag == "LoadZone")
        {
            SceneLoader.instance.NextScene();
        }
    }
}
