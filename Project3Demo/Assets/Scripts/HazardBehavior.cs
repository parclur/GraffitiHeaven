using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HazardBehavior : MonoBehaviour
{
    [SerializeField] private float timeToAttack = 2f;

    [SerializeField] private float timeToRetreat = 2f;

    [SerializeField] private Transform dormantTransform;

    [SerializeField] private Transform attackTransform;

    [SerializeField] private GameObject arm;

    Transform armTransform;

    CapsuleCollider armCollider;

    MeshRenderer armRender;

    private GameObject target = null;

    private bool isAttacking = false;

    private float currentTime = 0f;

    private void Start()
    {
        armTransform = arm.transform;
        armRender = arm.GetComponent<MeshRenderer>();
        armCollider = arm.GetComponent<CapsuleCollider>();
    }

    private void Update()
    {
        if (isAttacking)
        {
            armRender.enabled = true;
            armCollider.enabled = true;

            if (currentTime < timeToAttack)
            {           

                armTransform.position = Vector3.Lerp(dormantTransform.position, attackTransform.position, (currentTime / timeToAttack));
                currentTime += Time.deltaTime;
            }
            else
            {
                armTransform.position = attackTransform.position;
                armTransform.LookAt(target.transform);
                armTransform.Rotate(new Vector3(90f, 0f, 0f));
            }
        }
        else
        {
            if (currentTime < timeToRetreat)
            {
                armTransform.position = Vector3.Lerp(attackTransform.position, dormantTransform.position, (currentTime / timeToRetreat));
                armTransform.rotation = Quaternion.Slerp(armTransform.rotation, dormantTransform.rotation, (currentTime / timeToRetreat));
                currentTime += Time.deltaTime;
            }
            else
            {
                armRender.enabled = false;
                armCollider.enabled = false;
                armTransform.position = dormantTransform.position;
                armTransform.rotation = dormantTransform.rotation;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Diver" || other.gameObject.tag == "Drone")
        {
            target = other.gameObject;
            isAttacking = true;

            currentTime = 0f;

            AudioManager.instance.PlayOneShot("Monster1e", 1f);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Diver" || other.gameObject.tag == "Drone")
        {
            target = null;
            isAttacking = false;

            currentTime = 0f;

            //AudioManager.instance.PlayOneShot("Monster6e", 0.5f);
        }
    }

    public void ApplyCollision()
    {
        isAttacking = false;
    }
}