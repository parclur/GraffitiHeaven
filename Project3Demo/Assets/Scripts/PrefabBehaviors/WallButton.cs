using UnityEngine;
using Rewired;

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
            //if (Input.GetKeyDown(KeyCode.E))
            {
                //AudioManager.instance.PlayOneShot("ButtonPushed", 1f, 0f);
                connectsTo.SendMessage("Activate");
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
}