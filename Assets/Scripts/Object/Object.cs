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
        _rigidbody.isKinematic = true;
        isInteracting = true;

        originalScale = transform.localScale.x;
        originalDistance = Vector3.Distance(transform.localPosition, cameraTransform.position);
    }
    public void EndInteract()
    {
        _rigidbody.isKinematic = false;
        isInteracting = false;
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

            // 중앙을 기준으로 커지므로, 크기 변화에 따라 높이를 보정
            float heightAdjustment = (transform.localScale.y * originalScale - originalScale) / 2;

            RaycastHit floorHit;
            if (Physics.Raycast(transform.position + Vector3.up * 0.1f, Vector3.down, out floorHit, Mathf.Infinity, groundMask))
            {
                transform.position = new Vector3(transform.position.x, floorHit.point.y + heightAdjustment + offsetFactor, transform.position.z);
            }
        }
    }
    
}
