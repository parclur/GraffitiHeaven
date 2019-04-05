using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggeredAudioQueue : MonoBehaviour
{
    [SerializeField] private List<string> checkTags;

    [SerializeField] private string queue;

    [SerializeField] private float volume = 0.5f;

    [SerializeField] private float delay = 0f;

    [SerializeField] private bool canTriggerMultipleTimes = false;

    private void OnTriggerEnter(Collider other)
    {
        if (checkTags.Contains(other.gameObject.tag))
        {
            AudioManager.instance.PlayOneShot(queue, volume, delay);

            if (!canTriggerMultipleTimes)
            {
                Destroy(gameObject);
            }
        }
    }
}