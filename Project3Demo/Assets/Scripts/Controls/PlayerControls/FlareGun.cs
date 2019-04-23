using UnityEngine;
using Rewired;

public class FlareGun : MonoBehaviour
{
    [SerializeField] private float reloadTime;

    [SerializeField] public int flareCount;

    [SerializeField] private GameObject aimArea;

    private BoxCollider aimAreaCollider;

    private GameObject flarePrefab;

    private Camera aimCamera;

    private Player fireController;

    private bool canFire = true;

    private float currentReloadTime = 0f;

    private void Start()
    {
        aimAreaCollider = aimArea.GetComponent<BoxCollider>();

        // Go get the flare 
        flarePrefab = Resources.Load<GameObject>("Prefabs/Items/Flare");

        // Find the ROV player to aim through
        aimCamera = GameObject.Find("PlayerROV").transform.GetChild(0).gameObject.GetComponent<Camera>();

        // Get input from the Diver player
        fireController = ReInput.players.GetPlayer("Diver");
    }

    private void Update()
    {
        if (fireController.GetButtonDown("Shoot"))
        {
            if (canFire)
            {
                if (flareCount > 0)
                {
                    FireFlare(); // Create Flare instance

                    AudioManager.instance.PlayOneShot("Flare", 1f);

                    canFire = false; // Disable firing
                }
                else
                {
                    AudioManager.instance.PlayOneShot("EmptyFlareGunClick");
                }
            }
        }
   
        if (currentReloadTime > reloadTime) // Reset firing ability
        {
            canFire = true;

            currentReloadTime = 0f;
        }
        else // Otherwise count until reload time is up
        {
            currentReloadTime += Time.deltaTime;
        }
    }

    private void FireFlare() 
    {
        Ray ray = aimCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;
        Physics.Raycast(ray, out hit);

        GameObject flare = Instantiate(flarePrefab, gameObject.transform.position, gameObject.transform.rotation);
        
        if(aimAreaCollider.bounds.Contains(hit.point)) //Checks the collider to see if the point is within bounds
        {
            flare.transform.LookAt(hit.point); //If it is, rotate towards the point that was hit by the raycast
        }

        flare.GetComponent<Flare>().Ignite(); //Activates things within the flare script
        flareCount--;
    }

    public void AddFlares(int count)
    {
        flareCount += count;
    }
}