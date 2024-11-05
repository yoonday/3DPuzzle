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
        Vector3[] directions = { cameraTransform.forward, cameraTransform.right, -cameraTransform.right, cameraTransform.up, -cameraTransform.up }; // 여러 방향으로 Ray 쏘기

        foreach (Vector3 direction in directions)
        {
            if (Physics.Raycast(cameraTransform.position, direction, out hit, Mathf.Infinity, ignoreTargetMask))
            {
                transform.position = hit.point - offsetFactor * targetScale.x * direction; // 충돌 위치에 맞춰 위치 조정

                float currentDistance = Vector3.Distance(cameraTransform.position, transform.position);
                float s = currentDistance / originalDistance;

                targetScale.x = targetScale.y = targetScale.z = s;
                transform.localScale = targetScale * originalScale;

                // 높이 보정 추가
                float heightAdjustment = transform.localScale.y / 2;
                RaycastHit floorHit;
                if (Physics.Raycast(transform.position, Vector3.down, out floorHit, Mathf.Infinity, groundMask))
                {
                    transform.position = new Vector3(transform.position.x, floorHit.point.y + heightAdjustment + offsetFactor, transform.position.z);
                }
                break; 
            }
        }
    }
    
}
