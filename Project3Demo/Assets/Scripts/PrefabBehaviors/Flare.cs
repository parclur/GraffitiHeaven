using UnityEngine;

public class Flare : MonoBehaviour
{
    [SerializeField] private float speed;
    
    private AudioSource flareAudio;

    private void Start()
    {
        flareAudio = AudioManager.instance.AddAudio("BurningFlare", 1f, 0.75f, true, gameObject);
        gameObject.GetComponent<Rigidbody>().AddRelativeForce(gameObject.transform.up * speed );
    }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject, 2f);
    }
}