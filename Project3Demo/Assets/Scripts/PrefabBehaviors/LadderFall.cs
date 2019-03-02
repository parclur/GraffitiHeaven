using UnityEngine;

public class LadderFall : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Flare")
        {
            gameObject.GetComponent<Rigidbody>().useGravity = true;

            // needs better sound
            //AudioManager.instance.PlayOneShot("MetalCrash", 1f, 0.5f);
        }
    }
}