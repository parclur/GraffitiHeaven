using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorButton : MonoBehaviour
{
    [SerializeField] private GameObject connectsTo;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Diver")
        {
            StartCoroutine(ShiftDown());
            connectsTo.SendMessage("Activate");
        }
    }

    private IEnumerator ShiftDown()
    {
        Vector3 startPos = gameObject.transform.position;
        float timeElapsed = 0f;
        while (timeElapsed < 1.0f)
        {
            gameObject.transform.position = Vector3.Lerp(startPos, startPos + Vector3.down, (timeElapsed / 1f));
            timeElapsed += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }
}