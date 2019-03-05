using UnityEngine;

public class Flare : MonoBehaviour
{
    [SerializeField] private float speed;
    
    private AudioSource flareAudio;

    public void Ignite(Vector3 forward)
    {
        flareAudio = AudioManager.instance.AddAudio("BurningFlare", 1f, 0.75f, true, gameObject);
        gameObject.GetComponent<Rigidbody>().AddRelativeForce(forward * speed);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(flareAudio, 2f);
        Destroy(gameObject, 2f);
    }
}