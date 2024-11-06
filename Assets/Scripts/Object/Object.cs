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
    private Collider _collider;

    // »óÈ£ÀÛ¿ë ÇÏ´Â µ¿¾È »çÀÌÁî¸¦ ¹Ù²Ü ‹š ¾²´Â º¯¼ö
    private Vector3 initialPosition;
    public float offsetFactor;
    private float originalScale;
    private float originalDistance;
    private Vector3 targetScale;
    private Transform cameraTransform;
    private Rigidbody _rigidbody;

    // »óÈ£ÀÛ¿ë ÇÏ´Â µ¿¾È ÇÃ·¹ÀÌ¾î°¡ ¹°Ã¼¸¦ È¸Àü½ÃÅ°·Á ÇÒ ¶§ ¾²´Â º¯¼ö
    private bool RotZ = false;
    private bool RotY = false;
    [SerializeField] private float rotationSpeed = 30f;
    
    // »óÈ£ÀÛ¿ë ÇÏ´Â µ¿¾È °íÁ¤µÈ°Í Ã³·³ º¸ÀÌ°Ô ¹æÇâÀ» ¹Ù²Ù´Â µ¥ ¾²´Â º¯¼ö
    private Vector3 initialPlayerRot;
    private Vector3 initialRot;
    [SerializeField] private float maxScale = 15f;

    private void Start()
    {
        cameraTransform = Camera.main.transform;
        _rigidbody = GetComponent<Rigidbody>();
        _collider = GetComponent<Collider>();
    }

    private void Update()
    {
        if (isInteracting)
        {
            Resize();
            fixAngle();
            if (RotY)
            {
                initialRot.y += rotationSpeed * Time.deltaTime;
            }
            if (RotZ)
            {
                initialRot.z += rotationSpeed * Time.deltaTime;
            }
        }

        
    }
    public void OnInteract()
    {
        _rigidbody.isKinematic = true;
        isInteracting = true;
        _collider.enabled = false;

        initialPlayerRot = CharacterManager.Instance.Player.transform.eulerAngles;
        initialRot = transform.eulerAngles;

        originalScale = transform.localScale.x;
        originalDistance = Vector3.Distance(transform.localPosition, cameraTransform.position);
    }
    public void EndInteract()
    {
        _rigidbody.isKinematic = false;
        isInteracting = false;
        _collider.enabled = true;
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
                // CheckBox ï¿½æµ¹ ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ Raycastï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ ï¿½æµ¹ ï¿½ï¿½ï¿½ï¿½
                if (Physics.Raycast(transform.position, Vector3.down, out hit, Mathf.Infinity, groundMask))
                {
                   Vector3 correctionDirection = hit.normal; // ï¿½ï¿½ï¿½Î¿ï¿½ ï¿½æµ¹ ï¿½ï¿½ï¿½ï¿½
                    transform.position += correctionDirection * offsetFactor;
                }

            }


        }
    }

    private void fixAngle()
    {
        transform.eulerAngles = initialRot + CharacterManager.Instance.Player.transform.eulerAngles - initialPlayerRot; 
    }

    public void RotateY()
    {
        RotY = true;
    }

    public void RotateZ()
    {
        RotZ = true;
    }

    public void EndRotateY()
    {
        RotY = false;
    }
    public void EndRotateZ()
    {
        RotZ = false;
    }
}
