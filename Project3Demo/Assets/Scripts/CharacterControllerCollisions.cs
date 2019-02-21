using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControllerCollisions : MonoBehaviour {

    void OnControllerColliderHit(ControllerColliderHit hit){
        //Debug.Log(hit.gameObject.tag);
        if(hit.collider.tag == "Key"){
            Debug.Log("Player interactable with key: " + hit.gameObject.name);
            GetComponent<PlayerDiverMovement>().addKey(hit.gameObject);
            hit.gameObject.SetActive(false);
        }
    }
}
