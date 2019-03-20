using UnityEngine;
using Rewired;

public class LadderClimb : MonoBehaviour
{
    [SerializeField] private float upwardRate;

    Player rewiredPlayer;

    private void Start()
    {
        rewiredPlayer = ReInput.players.GetPlayer("Diver");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Diver")
        {
            other.gameObject.GetComponent<PlayerDiverMovement>().isGrounded = false;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Diver" && rewiredPlayer.GetButton("Interact"))
        {
            other.gameObject.GetComponent<CharacterController>().Move(Vector3.up * upwardRate);
        }
    }
}