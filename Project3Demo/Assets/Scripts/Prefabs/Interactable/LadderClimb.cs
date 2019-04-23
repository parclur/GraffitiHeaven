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
        if (other.gameObject.tag == "Diver")
        {
            PlayerDiverMovement move;
            move = other.gameObject.GetComponent<PlayerDiverMovement>();

            if(rewiredPlayer.GetButton("Interact") || move.climbing){
                move.cc.Move(Vector3.up * upwardRate);
                move.climbing = true;
            }
        }
    }

    private void OnTriggerExit(Collider other){
        if (other.gameObject.tag == "Diver"){
            PlayerDiverMovement move;
            move = other.gameObject.GetComponent<PlayerDiverMovement>();

            if(move.climbing){
                move.cc.Move(move.transform.forward * upwardRate * 3);
                move.climbing = false;
            }
        }
    }
}