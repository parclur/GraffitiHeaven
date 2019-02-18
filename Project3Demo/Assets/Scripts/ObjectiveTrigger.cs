using UnityEngine;
using Rewired;

public class ObjectiveTrigger : MonoBehaviour
{
    [SerializeField] private string playerThatCanInteract;

    Player rewiredPlayer;

    private void Start()
    {
        rewiredPlayer = ReInput.players.GetPlayer(playerThatCanInteract);
    }

    void OnTriggerStay(Collider other)
    {
        if (other.transform.tag == "Diver")
        {
            if (rewiredPlayer.GetButton("Interact"))
            {
                SceneLoader.instance.WinGame();
            }
        }
    }
}