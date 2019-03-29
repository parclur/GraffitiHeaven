using UnityEngine;

public class Flare : MonoBehaviour
{
    [SerializeField] private float speed;

    [SerializeField] private float lifetime;

    private AudioSource flareAudio;

    public void Ignite()
    {
        flareAudio = AudioManager.instance.AddAudio("BurningFlare", 1f, 0.75f, true, gameObject);     
        gameObject.GetComponent<Rigidbody>().AddRelativeForce(gameObject.transform.forward * speed);

        Destroy(flareAudio, lifetime);
        Destroy(gameObject, lifetime);
    }
}