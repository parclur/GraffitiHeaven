using UnityEngine;

public class LadderClimb : MonoBehaviour
{
    [SerializeField] private float upwardRate;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Diver")
        {
            gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Diver")
        {
            other.gameObject.GetComponent<Rigidbody>().AddForce(Vector3.up * upwardRate);
        }
    }
}