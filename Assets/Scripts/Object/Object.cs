using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.InputSystem.HID;

public class Object : MonoBehaviour, IInteractable
{
    private bool isInteracting = false;
    public LayerMask ignoreTargetMask;
    public LayerMask groundMask;

    private Vector3 initialPosition;
    public float offsetFactor;
    private float originalScale;
    private float originalDistance;
    private Vector3 targetScale;
    private Transform cameraTransform;
    private Rigidbody _rigidbody;

    private void Start()
    {
        cameraTransform = Camera.main.transform;
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (isInteracting)
        {
            Resize();
        }
    }
    public void OnInteract()
    {
        if (isInteracting)
        {
            _rigidbody.isKinematic = false;
            isInteracting = false;
        }
        else // (!isInteracting)
        {
            _rigidbody.isKinematic = true;

            isInteracting = true;

            originalScale = transform.localScale.x;
            originalDistance = Vector3.Distance(transform.localPosition, cameraTransform.position);
        }

        
    }

    private void Resize()
    {
        RaycastHit hit;
        if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit, Mathf.Infinity, ignoreTargetMask))
        {
            transform.position = hit.point - offsetFactor * targetScale.x * transform.forward;

            float currentDistance = Vector3.Distance(cameraTransform.position, transform.position);

            float s = currentDistance / originalDistance;
            targetScale.x = targetScale.y = targetScale.z = s;
            transform.localScale = targetScale * originalScale;

            if (Physics.CheckBox(transform.position, transform.localScale * 0.5f, Quaternion.identity, groundMask))
            {
                // 충돌이 발생하면, 충돌 방향의 반대 방향으로 이동하여 겹침 방지
                Vector3 correctionDirection = hit.normal; 
                transform.position += correctionDirection * offsetFactor;
            }


        }
    }
}
