using UnityEngine;

public class PushBox : MonoBehaviour
{
    [SerializeField] private Vector2 axis;

    [SerializeField] private float pushSpeed;

    private Rigidbody boxBody;

    private void Start()
    {
        boxBody = transform.parent.GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Diver")
        {
            boxBody.velocity = new Vector3(axis.x, 0f, axis.y) * pushSpeed;

            AudioManager.instance.PlayOneShot("MetalHit1", 1f);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Diver")
        {
            boxBody.velocity = Vector3.zero;
        }
    }
}