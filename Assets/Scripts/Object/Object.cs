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

    [SerializeField] private float maxScale = 15f;

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
            transform.position = hit.point - offsetFactor * targetScale.x * cameraTransform.forward;

            float currentDistance = Vector3.Distance(cameraTransform.position, transform.position);

            float s = currentDistance / originalDistance;
            targetScale.x = targetScale.y = targetScale.z = s;
            transform.localScale = targetScale * originalScale;

            if (Physics.CheckBox(transform.position, transform.localScale * 0.5f, Quaternion.identity, groundMask))
            {
                // CheckBox 충돌 후 새로 Raycast로 법선을 얻어 충돌 보정
                if (Physics.Raycast(transform.position, Vector3.down, out hit, Mathf.Infinity, groundMask))
                {
                    Vector3 correctionDirection = hit.normal; // 새로운 충돌 법선
                    transform.position += correctionDirection * offsetFactor;
                }
            }


        }
    }
}
