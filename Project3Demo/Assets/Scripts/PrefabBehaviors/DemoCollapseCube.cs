using UnityEngine;
using System.Collections;

public class DemoCollapseCube : MonoBehaviour, ITriggerable
{
    [SerializeField] private float collapseTime;

    public void TriggerEvent()
    {
        StartCoroutine(Collapse());
    }

    private IEnumerator Collapse()
    {
        float time = 0f;

        Vector3 startPos = gameObject.transform.position;
        Vector3 endPos = startPos + (Vector3.down * 4f);

        while (time < collapseTime)
        {
            gameObject.transform.position = Vector3.Lerp(startPos, endPos, (time / collapseTime));
            time += Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }
    }
}