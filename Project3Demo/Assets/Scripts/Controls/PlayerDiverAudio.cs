using UnityEngine;

public class PlayerDiverAudio : MonoBehaviour
{
    [SerializeField] private GameObject robot;

    private AudioSource robotAudio;

    private AudioSource footstepAudio;

    private CharacterController cc;

    private Rigidbody rb;

    void Start()
    {
        AudioManager.instance.AddAudio("Ambience1", 1f, 0f, true);
        AudioManager.instance.AddAudio("SlowBreathing", 0.05f, 0f, true);

        robotAudio = AudioManager.instance.AddAudio("ElectricMotor2", 0.25f, 0f, true);
        footstepAudio = AudioManager.instance.AddAudio("Footsteps", 1f, 0f, true);
        cc = gameObject.GetComponent<CharacterController>();
        rb = gameObject.GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (robot.GetComponent<Rigidbody>().velocity.magnitude > 0f || robot.GetComponent<Rigidbody>().angularVelocity.magnitude > 0f)
        {
            float distanceToRobot = Vector3.Distance(gameObject.transform.position, robot.transform.position);

            robotAudio.volume = 1f / distanceToRobot;
        }
        else
        {
            robotAudio.volume = 0f;
        }

        if ((cc && cc.velocity.magnitude > 0.5f) || (!cc && rb.velocity.magnitude > 0.5f))
        {
            footstepAudio.volume = 1f;
        }
        else
        {
            footstepAudio.volume = 0f;
        }
    }
}