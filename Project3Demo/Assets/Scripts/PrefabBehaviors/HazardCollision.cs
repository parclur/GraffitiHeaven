using UnityEngine;

public class HazardCollision : MonoBehaviour
{
    // The diver to apply a slow to
    private PlayerDiverMovement player;

    // The behavior to inform if hit with a flare
    private HazardBehavior behavior;

    private void Start()
    {
        player = GameObject.Find("PlayerDiver").GetComponent<PlayerDiverMovement>();
        behavior = transform.parent.GetComponent<HazardBehavior>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Diver")
        {
            player.ApplySlow(); // If the diver, slow the diver and apply screen overlays
        }

        if (collision.transform.tag == "Flare")
        {
            behavior.ApplyCollision(); // If the flare, stun and retreat
        }
    }
}