using UnityEngine;
using System.Collections;
using System.Collections.Generic; // Will be List<> based in the future

public class TriggeredEventCameraOverride : MonoBehaviour
{
    // Which Camera object is controlled for the duration?
    [SerializeField] private Camera overrideCamera;

    // Is there an existing controller for the Camera to be overrode?
    [SerializeField] private PlayerROVMovement controller;

    // How long will it take to look at the thing in question?
    [SerializeField] private float snapDuration;

    // How long will it look at the thing after snapping?
    [SerializeField] private float focusDuration;

    // What is being looked at?
    [SerializeField] private Transform focalPoint;

    // Should the Camera be overridden even if there isn't line-of-sight to the focal point?
    [SerializeField] bool checkLineOfSight = true;

    public void OverrideCamera()
    {
        if (checkLineOfSight) // Can the camera actually see the object in question?
        {
            Ray ray = new Ray(overrideCamera.transform.position, (overrideCamera.transform.position - focalPoint.position).normalized);
            RaycastHit hit;
            Physics.Raycast(ray, out hit);

            if (hit.collider.gameObject != focalPoint.gameObject)
            {
                return;
            }
        }

        StartCoroutine(SlerpToFocalPoint());
    }

    private IEnumerator SlerpToFocalPoint()
    {     
        controller.enabled = false; // Disable the existing controller

        float time = 0f;

        Quaternion startRot = overrideCamera.transform.rotation;
        Quaternion endRot = focalPoint.rotation;

        while (time < snapDuration) // Slerp to the focal point
        {
            overrideCamera.transform.rotation = Quaternion.Slerp(startRot, endRot, (time / snapDuration));
            time += Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }

        StartCoroutine(WatchForDuration()); // Hang for duration
    }

    private IEnumerator WatchForDuration()
    {
        yield return new WaitForSeconds(focusDuration);

        controller.enabled = true; // Reenable control
    }
}