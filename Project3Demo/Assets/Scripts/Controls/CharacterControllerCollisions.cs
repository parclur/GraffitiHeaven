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
        if(hit.collider.tag == "Lamprey"){
            //Add something here
            Debug.Log("You died.");
        }
    }
}
