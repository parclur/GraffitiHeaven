using UnityEngine;
using Rewired;
using System.Collections;
using System.Collections.Generic;

public class WallButton : MonoBehaviour
{
    [SerializeField] private GameObject connectsTo;

    private Player rewiredPlayer;

    private bool isInRange = false;

    private void Start()
    {
        rewiredPlayer = ReInput.players.GetPlayer("Drone");
    }

    private void Update()
    {
        if (isInRange)
        {
            if (rewiredPlayer.GetButtonDown("Interact"))
            {
                connectsTo.SendMessage("Activate");
                StartCoroutine(PushButton());
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Drone")
        {
            isInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Drone")
        {
            isInRange = false;
        }
    }

    IEnumerator PushButton(){
        transform.localScale = new Vector3(transform.localScale.x / 2, transform.localScale.y / 2, transform.localScale.z / 2);
        yield return new WaitForSeconds(1f);
        transform.localScale = new Vector3(transform.localScale.x * 2, transform.localScale.y * 2, transform.localScale.z * 2);
    }
}