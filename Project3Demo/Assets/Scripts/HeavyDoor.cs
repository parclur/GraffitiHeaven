using System.Collections;
using UnityEngine;

public class HeavyDoor : MonoBehaviour
{
    [SerializeField] private Vector3 openPos;

    [SerializeField] private float moveTime;

    private Vector3 startingPos;

    private void Start()
    {
        startingPos = gameObject.transform.position;
        openPos = startingPos + openPos;
    }

    public void Activate()
    {
        AudioManager.instance.PlayOneShot("MetalDoorOpen");
        StartCoroutine(MoveDoor());
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