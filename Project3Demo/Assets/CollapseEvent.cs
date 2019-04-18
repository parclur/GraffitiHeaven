using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollapseEvent : MonoBehaviour, ITriggerable
{
    [SerializeField] private GameObject floor;
    [SerializeField] private float collapseTime;

    public void TriggerEvent()
    {
        StartCoroutine(Collapse());
    }

    private IEnumerator Collapse()
    {
        float time = 0f;

        Vector3 startPos = floor.transform.position;
        Vector3 endPos = startPos + (Vector3.down * 10f);

        while (time < collapseTime)
        {
            floor.transform.position = Vector3.Lerp(startPos, endPos, (time / collapseTime));
            time += Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }
    }
}
