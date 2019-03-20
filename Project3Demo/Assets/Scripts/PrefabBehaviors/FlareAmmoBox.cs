using UnityEngine;
using Rewired;

public class FlareAmmoBox : MonoBehaviour
{
    [SerializeField] private int flareCount;

    private FlareGun playerFlareGun;

    private Player rewiredPlayer;

    private bool inRange = false;

    private void Start()
    {
        playerFlareGun = GameObject.Find("FlareGun").GetComponent<FlareGun>();
        rewiredPlayer = ReInput.players.GetPlayer("Diver");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Diver")
        {
            inRange = true;          
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Diver")
        {
            inRange = false;
        }
    }

    private void Update()
    {
        if (inRange && rewiredPlayer.GetButtonDown("Interact"))
        {
            //playerFlareGun.AddFlares(flareCount);
            Destroy(gameObject, 0.1f);

            //AudioManager.instance.PlayOneShot("FlareAmmoBoxPickup");
        }
    }
}