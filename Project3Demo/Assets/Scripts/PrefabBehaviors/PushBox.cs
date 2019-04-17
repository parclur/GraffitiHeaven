using UnityEngine;

public class PushBox : MonoBehaviour
{
    [SerializeField] private Vector2 axis;

    [SerializeField] private float pushSpeed;

    private BoxFall boxFall;

    Rigidbody rb;

    private void Start()
    {
        rb = transform.parent.GetComponent<Rigidbody>();
        boxFall = transform.parent.GetComponent<BoxFall>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Diver")
        {
            PlayerDiverMovement diverMovement = other.GetComponent<PlayerDiverMovement>();
            if(boxFall.dontPush) diverMovement.pullBox = null;
            else {
                diverMovement.InitBox(axis);
                diverMovement.pullBox = rb;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Diver" && !boxFall.dontPush)
        {
            PlayerDiverMovement diverMovement = other.GetComponent<PlayerDiverMovement>();
            if(!diverMovement.pulling){
                diverMovement.pullBox = null;
            }
            boxFall.rb.velocity = Vector3.zero;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Diver")
        {
            PlayerDiverMovement diverMovement = other.GetComponent<PlayerDiverMovement>();
            if(boxFall.dontPush) diverMovement.pullBox = null;
            else if(diverMovement.pullBox == null){
                diverMovement.InitBox(axis);
                diverMovement.pullBox = rb;
            }
        }
    }
}