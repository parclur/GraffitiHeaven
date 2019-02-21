using UnityEngine;

public class LadderClimb : MonoBehaviour
{
    [SerializeField] private float upwardRate;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Diver")
        {
            gameObject.GetComponent<PlayerDiverMovement>().isGrounded = false;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Diver")
        {
            other.gameObject.GetComponent<CharacterController>().Move(Vector3.up * upwardRate);
        }
    }
}