using UnityEngine;
using Rewired;

public class FlareGun : MonoBehaviour
{


    [SerializeField] string playerControlling;

    [SerializeField] Camera aimCamera;

    [SerializeField] GameObject bulletPrefab;

    [SerializeField] GameObject aimReticle;

    [SerializeField] GameObject flareSpawnpoint;

    [SerializeField] float bulletSpeed;

    [SerializeField] float delay;

    bool delayStarted;

    float currentDelayTime;

    Transform bulletSpawnpointTransform;

    Transform aimReticleTransform;

    Quaternion aimReticleRotStart;

    Player rewiredPlayer;

    void Start()
    {

        rewiredPlayer = ReInput.players.GetPlayer(playerControlling);

        aimReticleTransform = aimReticle.transform;

        aimReticleRotStart = aimReticleTransform.rotation;

        bulletSpawnpointTransform = gameObject.transform;

    }

    void Update()

    {

        TestDelay();

        AimThroughCamera();

        if (rewiredPlayer.GetButtonDown("Shoot") && !delayStarted) //If the button has been pressed and the delay hasn't been initated

        {

            StartDelayTimer();

            ShootGun();

            AudioManager.instance.PlayOneShot("Flare", 1f);

        }

    }

    void ShootGun() //Fires a bullet prefab from the bulletSpawn point

    {

        GameObject bullet = Instantiate(bulletPrefab, flareSpawnpoint.transform.position, Quaternion.identity);

        // TODO make this allow the player to free shoot

        //if (Vector3.Dot(bullet.transform.forward, aimReticleTransform.position) > 0f)

        //{



        //}

        //bullet.transform.LookAt(aimReticleTransform);

        bullet.GetComponent<Rigidbody>().AddForce(flareSpawnpoint.transform.forward * bulletSpeed);

        //Quaternion rotation = Quaternion.LookRotation(aimReticleTransform.position, Vector3.up);
        //bullet.transform.rotation = rotation;

        //bullet.transform.Rotate(Vector3.left * -90f);

    }

    void StartDelayTimer()

    {

        if (!delayStarted)

        {

            currentDelayTime = Time.time + delay;

            delayStarted = true;

        }

    }

    void AimThroughCamera()

    {

        Ray ray = aimCamera.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));

        RaycastHit hit;

        Physics.Raycast(ray, out hit);

        aimReticleTransform.position = hit.point;

        aimReticleTransform.rotation = aimReticleRotStart;

    }

    void TestDelay()

    {

        if (delayStarted && Time.time > currentDelayTime)

        {

            delayStarted = false;

        }

    }


    //[SerializeField] private float reloadTime;

    //[SerializeField] private int flareCount;

    //[SerializeField] private GameObject playerFOV;

    //[SerializeField] private GameObject flareGunSpawnPoint;

    //private GameObject flarePrefab;

    //private Camera aimCamera;

    //private Player fireController;

    //private bool canFire = true;

    //private float currentReloadTime = 0f;

    //private Transform flareGunTransform;

    //private Transform cameraTransform;

    //private Transform flareSpawnPointTransform;

    //private BoxCollider playerFOVBoxCollider;

    //private void Start()
    //{
    //    // Go get the flare 
    //    flarePrefab = Resources.Load<GameObject>("Prefabs/Flare");

    //    // Find the ROV player to aim through
    //    aimCamera = GameObject.Find("PlayerROV").transform.GetChild(0).gameObject.GetComponent<Camera>();

    //    // Get input from the Diver player
    //    fireController = ReInput.players.GetPlayer("Diver");

    //    playerFOVBoxCollider = playerFOV.GetComponent<BoxCollider>(); //Gets the box collider of the player's fov

    //    cameraTransform = aimCamera.GetComponent<Transform>();

    //    flareGunTransform = gameObject.GetComponent<Transform>();

    //    flareSpawnPointTransform = flareGunSpawnPoint.transform;
    //}

    //private void Update()
    //{
    //    if (fireController.GetButtonDown("Shoot"))
    //    {
    //        if (flareCount > 0)
    //        {
    //            if (canFire)
    //            {
    //                FireFlare(); // Create Flare instance

    //                AudioManager.instance.PlayOneShot("Flare", 1f);

    //                canFire = false; // Disable firing
    //            }
    //        }
    //        else
    //        {
    //            //AudioManager.instance.PlayOneShot("EmptyFlareGunClick");
    //        }
    //    }

    //    if (currentReloadTime > reloadTime) // Reset firing ability
    //    {
    //        canFire = true;

    //        currentReloadTime = 0f;
    //    }
    //    else // Otherwise count until reload time is up
    //    {
    //        currentReloadTime += Time.deltaTime;
    //    }
    //}

    //private void FireFlare() 
    //{
    //    GameObject flare = Instantiate(flarePrefab, flareSpawnPointTransform.position, flarePrefab.transform.rotation);

    //    // Only raycasts when firing, more effecient
    //    Ray ray = aimCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
    //    RaycastHit hit;
    //    Physics.Raycast(ray, out hit);

    //    //flare.transform.Rotate(Vector3.left * -90f);
    //    //if(playerFOVBoxCollider.bounds.Contains(hit.point)) //Checks to see if the point is within the diver's view (Collider)
    //    //{
    //    //    Debug.Log(hit.point);
    //    //    Vector3 aimPos = hit.point;
    //    //    flare.transform.LookAt(aimPos);

    //    //    

    //    //    Debug.Log("Drone is aiming for player");
    //    //}

    //    //Vector3 hitVec = (cameraTransform.position - hit.point).normalized;
    //    //Vector3 aimVec = flareGunTransform.right.normalized;

    //    //// If the camera is aiming toward the player's aim
    //    //if (Vector3.Dot(aimVec, hitVec) > 0.866)
    //    //{
    //    //    flare.transform.LookAt(hit.point);
    //    //    Debug.Log("WORKING YO");
    //    //}

    //    flare.GetComponent<Flare>().Ignite(flare.transform.forward);

    //    flareCount--;
    //}

    //public void AddFlares(int count)
    //{
    //    flareCount += count;
    //}
}