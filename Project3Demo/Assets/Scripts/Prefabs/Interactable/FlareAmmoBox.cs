using UnityEngine;
using Rewired;

public class FlareAmmoBox : MonoBehaviour
{
    [SerializeField] private int flareCount;

    private FlareGun playerFlareGun;

    private Player rewiredDiver;
    private Player rewiredDrone;

    private bool inRange = false;

    private void Start()
    {
        playerFlareGun = GameObject.Find("FlareGunSpawn").GetComponent<FlareGun>();
        rewiredDiver = ReInput.players.GetPlayer("Diver");
        rewiredDrone = ReInput.players.GetPlayer("Drone");

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Diver" || other.gameObject.tag == "Drone")
        {
            inRange = true;          
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Diver" || other.gameObject.tag == "Drone")
        {
            inRange = false;
        }
    }

    private void Update()
    {
        if (inRange && rewiredDiver.GetButtonDown("Interact") || inRange && rewiredDrone.GetButtonDown("Interact"))
        {
            playerFlareGun.AddFlares(flareCount);
            Destroy(gameObject, 0.1f);

            AudioManager.instance.PlayOneShot("FlareAmmoBoxPickup");
        }
    }
}