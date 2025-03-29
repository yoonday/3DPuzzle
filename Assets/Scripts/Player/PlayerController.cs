using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed; // �̵� �ӵ�
    public float jumpPower;
    private Vector2 curMovementInput;
    public LayerMask groundLayerMask; // �ٴ� ���̾�

    [Header("Look")]
    public Transform cameraContainer; // ī�޶� �����̳�
    public float minXLook; // ī�޶� ȸ�� �ּҰ�
    public float maxXLook; // ī�޶� ȸ�� �ִ밪
    private float camCurXRot; // ī�޶��� ���� x�� ȸ�� ��
    public float lookSensitivity; // ȸ�� �ΰ���
    private Vector2 mouseDelta; 
    public bool canLook = true;

    public event Action optionToggle;
    private Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }


    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;  // Ŀ���� ���� ���� �߾ӿ� ����
    }

    private void Update()
    {
        //Debug.DrawRay(/*CharacterManager.Instance.Player.*/transform.position + new Vector3(0, 1, 0), transform.forward, Color.red);
    }

    void FixedUpdate()
    {
        Move();
    }

    private void LateUpdate()
    {
        if (canLook)
        {
            CameraLook();
        }
    }

    void Move() // �̵�
    {
       
        Vector3 dir = transform.forward * curMovementInput.y + transform.right * curMovementInput.x;
        dir *= moveSpeed; 
        dir.y = _rigidbody.velocity.y; 

        _rigidbody.velocity = dir; 

    }

    void CameraLook() // ī�޶� ȸ��(���Ʒ� ������)
    {
        camCurXRot += mouseDelta.y * lookSensitivity;
        camCurXRot = Mathf.Clamp(camCurXRot, minXLook, maxXLook); // ���� ���� 
        cameraContainer.localEulerAngles = new Vector3(-camCurXRot, 0, 0); // ī�޶��� ��/�Ʒ� ȸ�� ó��. 

        transform.eulerAngles += new Vector3(0, mouseDelta.x * lookSensitivity, 0); // ĳ������ ��/�� ȸ��                                                                                  
    }


    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed) 
        {
            curMovementInput = context.ReadValue<Vector2>();
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            curMovementInput = Vector2.zero;
        }
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        mouseDelta = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && IsGrounded()) 
        {
            _rigidbody.AddForce(Vector2.up * jumpPower, ForceMode.Impulse);
        }
    }

    bool IsGrounded()
    {
        Ray[] rays = new Ray[4]
        {
            new Ray(transform.position + (transform.forward * 0.2f) + (transform.up * 0.15f), Vector3.down),
            new Ray(transform.position + (-transform.forward * 0.2f) + (transform.up * 0.15f), Vector3.down), 
            new Ray(transform.position + (transform.right * 0.2f) + (transform.up * 0.15f), Vector3.down), 
            new Ray(transform.position + (-transform.forward * 0.2f) + (transform.up * 0.15f), Vector3.down) 
        };

        for (int i = 0; i < rays.Length; i++)
        {
            if (Physics.Raycast(rays[i], 0.1f, groundLayerMask)) // groundLayer�� �ش��ϴ� �͸� ����
            {
                return true;
            }
        }

        return false;
    }

    public void OnOptionInput(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Started)
        {
            
            optionToggle?.Invoke();
            ToggleCursor();
        }
    }

    public void ToggleCursor()
    {
        bool isLocked = Cursor.lockState == CursorLockMode.Locked;
        Cursor.lockState = isLocked ? CursorLockMode.None : CursorLockMode.Locked;
        canLook = !canLook;
    }
}
