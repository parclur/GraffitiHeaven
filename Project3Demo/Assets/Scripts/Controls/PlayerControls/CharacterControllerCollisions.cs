using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class CharacterControllerCollisions : MonoBehaviour
{
    Player player;

    void Start() {
        player = ReInput.players.GetPlayer("Diver");
    }


    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider.tag == "Key" && player.GetButton("Interact"))
        {
            GetComponent<PlayerDiverMovement>().addKey(hit.gameObject);
            hit.gameObject.SetActive(false);
        }
        if(hit.collider.tag == "LoadZone")
        {
            SceneLoader.instance.NextScene();
        }
    }
}
