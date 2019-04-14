using UnityEngine;

public class PushBox : MonoBehaviour
{
    [SerializeField] private Vector2 axis;

    [SerializeField] private float pushSpeed;

    private BoxFall boxFall;

    private void Start()
    {
        boxFall = transform.parent.GetComponent<BoxFall>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Diver" && !boxFall.dontPush)
        {
            boxFall.rb.velocity = new Vector3(axis.x * pushSpeed, boxFall.rb.velocity.y, axis.y * pushSpeed);

            AudioManager.instance.PlayOneShot("MetalHit1", 1f);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Diver" && !boxFall.dontPush)
        {
            boxFall.rb.velocity = Vector3.zero;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Diver" && !boxFall.dontPush)
        {
            boxFall.rb.velocity = new Vector3(axis.x * pushSpeed, boxFall.rb.velocity.y, axis.y * pushSpeed);
        }
    }
}