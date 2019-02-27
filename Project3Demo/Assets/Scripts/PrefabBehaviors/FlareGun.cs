using UnityEngine;
using Rewired;

public class FlareGun : MonoBehaviour
{
    [SerializeField] private float reloadTime;

    private GameObject flarePrefab;

    private Camera aimCamera;

    private Player fireController;

    private Vector3 firingOffset;

    private bool canFire = true;

    private float currentReloadTime = 0f;

    private void Start()
    {
        // Go get the flare 
        flarePrefab = Resources.Load<GameObject>("Prefabs/Flare");

        // Find the ROV player to aim through
        aimCamera = GameObject.Find("PlayerROV").transform.GetChild(0).gameObject.GetComponent<Camera>();

        // Get input from the Diver player
        fireController = ReInput.players.GetPlayer("Diver");

        // Get the offset so as to not shoot ourselves in the foot
        firingOffset = gameObject.transform.position + Vector3.forward;
    }

    private void Update()
    {
        // Is there a flare available? Is the player firing?
        if (canFire && fireController.GetButtonDown("Shoot"))
        {
            FireFlare(); // Create Flare instance

            AudioManager.instance.PlayOneShot("Flare", 1f);

            canFire = false; // Disable firing
        }
        else if (currentReloadTime > reloadTime) // Reset firing ability
        {
            canFire = true;

            currentReloadTime = 0f;
        }
        else // Otherwise count until reload time is up
        {
            currentReloadTime += Time.deltaTime;
        }
    }

    private void FireFlare() //Fires a bullet prefab from the bulletSpawn point
    {
        // Only raycasts when firing, more effecient
        Ray ray = aimCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;
        Physics.Raycast(ray, out hit);

        // Create the flare
        GameObject flare = Instantiate(flarePrefab, gameObject.transform.position /*firingOffset*/, flarePrefab.transform.rotation);

        // Make the flare aim correctly
        flare.transform.LookAt(hit.collider.gameObject.transform.position);
        flare.transform.Rotate(Vector3.left * -90f);
    }
}