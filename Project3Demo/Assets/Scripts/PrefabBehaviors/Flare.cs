using UnityEngine;

public class Flare : MonoBehaviour
{
    [SerializeField] private float speed; //Speed at wich the flare is shot at

    [SerializeField] private float gravitySpeedThreshold = 1; //The threshold that the velocity needs to be under in order for gravity to be enabled

    [SerializeField] private float dragAfterGravity = 1.0f; //The drag that is applyed after the gravity has been enabled

    [SerializeField] private float delayBeforeGravityEnabled = 2; //Delays before the gravity is enabled

    private float timeBeforeDelay = 0; //Time variable for the timer

    bool timerEnabled = false; //If the timer is enabled

    bool gravityEnabled = false; //If gravity has been enabled

    private AudioSource flareAudio;

    private Rigidbody rb;

    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }

    private void Update()
    {
        
        if (timerEnabled)
            TestGravityTimer();
        else if(!gravityEnabled)
            CheckVelocity();
    }

    public void Ignite()
    {
        flareAudio = AudioManager.instance.AddAudio("BurningFlare", 1f, 0.75f, true, gameObject);     
        gameObject.GetComponent<Rigidbody>().AddForce(gameObject.transform.forward * speed);
    }


    private void CheckVelocity() //Checks to see if hte velocity is below the threshold
    {
        if (rb.velocity.magnitude <= gravitySpeedThreshold) //if the velocity vector is 0
        {
            timerEnabled = true;
            timeBeforeDelay = Time.time + delayBeforeGravityEnabled;
            //Debug.Log("Timer enabled!");
        }
    }

    private void EnableGravity() //Enables gravity
    {
        gravityEnabled = true; //Avoids redunent checks
        rb.useGravity = true;
        rb.drag = dragAfterGravity;
    }

    private void TestGravityTimer() //Checks the timer, to enable set timerEnabled = ture and timerBeforeDelay = Time.time + delayBeforeGravityEnabled
    {
        if(timerEnabled && timeBeforeDelay <= Time.time)
        {
            EnableGravity(); //Enables gravity
            timerEnabled = false; //Disables timer
        }
    }
}