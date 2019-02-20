using System.Collections;
using UnityEngine;

public class HeavyDoor : MonoBehaviour
{
    [SerializeField] private Vector3 openPos;

    [SerializeField] private float moveTime;

    [SerializeField] private float keyDistance;

    private Vector3 startingPos;

    GameObject player;

    private void Start()
    {
        startingPos = gameObject.transform.position;
        openPos = startingPos + openPos;
        player = GameObject.FindGameObjectWithTag("Diver");
    }

    public void Activate()
    {
        AudioManager.instance.PlayOneShot("MetalDoorOpen");
        StartCoroutine(MoveDoor());
    }

    public void KeyActivate(){
        if(Vector3.Distance(player.transform.position, transform.position) > keyDistance){
            
        }
    }

    private IEnumerator MoveDoor()
    {
        float elapsedTime = 0.0f;
        while (elapsedTime < moveTime)
        {
            gameObject.transform.position = Vector3.Lerp(startingPos, openPos, (elapsedTime / moveTime));
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }        
    }
}