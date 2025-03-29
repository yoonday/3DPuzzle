using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed; // 이동 속도
    public float jumpPower;
    private Vector2 curMovementInput;
    public LayerMask groundLayerMask; // 바닥 레이어

    [Header("Look")]
    public Transform cameraContainer; // 카메라 컨테이너
    public float minXLook; // 카메라 회전 최소값
    public float maxXLook; // 카메라 회전 최대값
    private float camCurXRot; // 카메라의 현재 x축 회전 값
    public float lookSensitivity; // 회전 민감도
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
        Cursor.lockState = CursorLockMode.Locked;  // 커서를 게임 뷰의 중앙에 고정
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

    void Move() // 이동
    {
       
        Vector3 dir = transform.forward * curMovementInput.y + transform.right * curMovementInput.x;
        dir *= moveSpeed; 
        dir.y = _rigidbody.velocity.y; 

        _rigidbody.velocity = dir; 

    }

    void CameraLook() // 카메라 회전(위아래 움직임)
    {
        camCurXRot += mouseDelta.y * lookSensitivity;
        camCurXRot = Mathf.Clamp(camCurXRot, minXLook, maxXLook); // 범위 설정 
        cameraContainer.localEulerAngles = new Vector3(-camCurXRot, 0, 0); // 카메라의 위/아래 회전 처리. 

        transform.eulerAngles += new Vector3(0, mouseDelta.x * lookSensitivity, 0); // 캐릭터의 좌/우 회전                                                                                  
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
            if (Physics.Raycast(rays[i], 0.1f, groundLayerMask)) // groundLayer에 해당하는 것만 검출
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
