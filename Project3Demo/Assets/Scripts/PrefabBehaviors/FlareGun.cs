using UnityEngine;
using Rewired;

public class FlareGun : MonoBehaviour
{
    [SerializeField] private float reloadTime;

    [SerializeField] private int flareCount;

    private GameObject flarePrefab;

    private Camera aimCamera;

    private Player fireController;

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

        Debug.Log(hit.transform);
        GameObject flare = Instantiate(flarePrefab, gameObject.transform.position, gameObject.transform.rotation);
        
        flare.transform.LookAt(hit.point);

        //flare.transform.Rotate(Vector3.up * 90f);
        flare.GetComponent<Flare>().Ignite();
        flareCount--;
    }

    public void AddFlares(int count)
    {
        flareCount += count;
    }
}