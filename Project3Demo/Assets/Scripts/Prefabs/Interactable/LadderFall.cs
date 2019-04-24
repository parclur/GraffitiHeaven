using UnityEngine;

public class LadderFall : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Flare")
        {
            gameObject.GetComponent<Rigidbody>().useGravity = true;

            AudioManager.instance.PlayOneShot("MetalHit2", 1f, 0.5f);
        }
    }
}