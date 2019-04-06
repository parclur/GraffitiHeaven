using UnityEngine;

public interface ITriggerable
{
    void TriggerEvent();
}

public class TriggeredEvent : MonoBehaviour
{
    [SerializeField] private bool overridesCamera = false;

    private TriggeredEventCameraOverride overrideBehavior;

    private void Start()
    {
        if (overridesCamera)
            overrideBehavior = GetComponent<TriggeredEventCameraOverride>();
    }

    public void Trigger()
    {
        gameObject.SendMessage("TriggerEvent");

        if (overridesCamera)
            overrideBehavior.OverrideCamera();
    }
}