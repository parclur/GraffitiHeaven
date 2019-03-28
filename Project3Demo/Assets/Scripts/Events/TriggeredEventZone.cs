using UnityEngine;
using System.Collections.Generic;

public class TriggeredEventZone : MonoBehaviour
{
    [SerializeField] private List<string> checkTags;

    [SerializeField] private List<TriggeredEvent> triggeredEvents;

    [SerializeField] private bool canTriggerMultipleTimes = false;

    private void OnTriggerEnter(Collider other)
    {
        if (checkTags.Contains(other.gameObject.tag))
        {
            foreach (TriggeredEvent e in triggeredEvents)
            {
                e.Trigger();
            }

            if (!canTriggerMultipleTimes)
            {
                Destroy(gameObject);
            }
        }
    }
}