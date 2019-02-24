using UnityEngine;
using Rewired;

public class FlareGun : MonoBehaviour
{
    [SerializeField] string playerControlling;

    [SerializeField] Camera aimCamera;

    [SerializeField] GameObject bulletPrefab;

    [SerializeField] GameObject aimReticle;

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
    }

    void Update()
    {
        TestDelay();
        AimThroughCamera();

        if(rewiredPlayer.GetButtonDown("Shoot") && !delayStarted) //If the button has been pressed and the delay hasn't been initated
        {
            StartDelayTimer();
            ShootGun();
            AudioManager.instance.PlayOneShot("Flare", 1f);
        }    
    }

    void ShootGun() //Fires a bullet prefab from the bulletSpawn point
    {
        GameObject bullet = Instantiate(bulletPrefab, gameObject.transform.position, bulletPrefab.transform.rotation);

        // TODO make this allow the player to free shoot
        //if (Vector3.Dot(bullet.transform.forward, aimReticleTransform.position) > 0f)
        //{
                    
        //}

        bullet.transform.LookAt(aimReticleTransform.position);
        bullet.transform.Rotate(Vector3.left * -90f);
    }

    void StartDelayTimer()
    {
        if(!delayStarted)
        {
            currentDelayTime = Time.time + delay; 
            delayStarted = true;
        }
    }

    void AimThroughCamera()
    {
        Ray ray = aimCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;
        Physics.Raycast(ray, out hit); 

        aimReticleTransform.position = hit.point;
        aimReticleTransform.rotation = aimReticleRotStart;
    }

    void TestDelay()
    {
        if(delayStarted && Time.time > currentDelayTime)
        {
            delayStarted = false;
        }
    }   
}