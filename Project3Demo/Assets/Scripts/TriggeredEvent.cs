using UnityEngine;

public class TriggeredEvent : MonoBehaviour
{
    private TriggeredEventCameraOverride overrideBehavior;

    private void Start()
    {
        overrideBehavior = GetComponent<TriggeredEventCameraOverride>();
    }

    public void Trigger()
    {
        gameObject.SendMessage("TriggerEvent");
        overrideBehavior.OverrideCamera();
    }
}