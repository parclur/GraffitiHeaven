using UnityEngine;

public class Flare : MonoBehaviour
{
    [SerializeField] private float speed;

    [SerializeField] private float lifetime;

    [SerializeField] private Vector3 gravitySpeedThreshold = new Vector3(0, 0, 0); //The threshold that the velocity needs to be under in order for gravity to be enabled

    [SerializeField] private float dragAfterGravity = 1.0f; //The drag that is applyed after the gravity has been enabled

    bool gravityEnabled = false;

    private AudioSource flareAudio;

    private Rigidbody rb;

    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }

    private void Update()
    {
        CheckVelocity();
    }

    private void OnCollisionEnter(Collision collision)
    {
        //if (!gravityEnabled)
        //    EnableGravity();
    }

    public void Ignite()
    {
        flareAudio = AudioManager.instance.AddAudio("BurningFlare", 1f, 0.75f, true, gameObject);     
        gameObject.GetComponent<Rigidbody>().AddForce(gameObject.transform.forward * speed);

        //Destroy(flareAudio, lifetime);
        //Destroy(gameObject, lifetime);
    }


    private void CheckVelocity() //Checks to see if hte velocity is below the threshold
    {
        if(!gravityEnabled)
        {
            if (rb.velocity.x <= gravitySpeedThreshold.x && rb.velocity.y <= gravitySpeedThreshold.y && rb.velocity.z <= gravitySpeedThreshold.z) //if the velocity vector is 0
            {
                EnableGravity();
            }
        }

    }

    private void EnableGravity() //Enables gravity
    {
        //gravityEnabled = true; //Avoids redunent checks
        //rb.useGravity = true;
        //rb.drag = dragAfterGravity;
    }
}