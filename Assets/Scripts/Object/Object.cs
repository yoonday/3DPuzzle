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

    // 상호작용 하는 동안 사이즈를 바꿀 떄 쓰는 변수
    private Vector3 initialPosition;
    public float offsetFactor;
    private float originalScale;
    private float originalDistance;
    private Vector3 targetScale;
    private Transform cameraTransform;
    private Rigidbody _rigidbody;

    // 상호작용 하는 동안 플레이어가 물체를 회전시키려 할 때 쓰는 변수
    private bool RotZ = false;
    private bool RotY = false;
    private bool RotX = false;
    [SerializeField] private float rotationSpeed = 30f;
    
    // 상호작용 하는 동안 고정된것 처럼 보이게 방향을 바꾸는 데 쓰는 변수
    private Vector3 initialPlayerRot;
    private Vector3 initialRot;

    [SerializeField] private float maxScale = 30f;
    [SerializeField] private float minScale = 0.1f;

    [SerializeField]
    List<GameObject> _objects = new List<GameObject>();
    [SerializeField]
    List<GameObject> InteracableObjects = new List<GameObject>();


    int groundLayer;
    int offGroundLayer;
    int interactableLayer;
    public bool notControlTrigger = false;
    public bool includePlayer = false;

    private Transform parentTransform;
    private Vector3 initialPos;
    private Vector3 initialScale;
    private Vector3 initialRotation;

    private void Start()
    {
        cameraTransform = Camera.main.transform;
        _rigidbody = GetComponent<Rigidbody>();
        _collider = GetComponent<Collider>();
        groundLayer = LayerMask.NameToLayer("Ground");
        offGroundLayer = LayerMask.NameToLayer("OffGround");
        interactableLayer = LayerMask.NameToLayer("Interactable");

        parentTransform = transform.parent;
        initialPos = transform.localPosition;
        initialScale = transform.localScale;
        initialRotation = transform.eulerAngles;

    }

    private void Update()
    {
        if (isInteracting)
        {
            Resize();
            fixAngle();
            if(RotX)
            {
                initialRot.x += rotationSpeed * Time.deltaTime;
            }
            if (RotY)
            {
                initialRot.y += rotationSpeed * Time.deltaTime;
            }
            //if (RotZ)
            //{
            //    initialRot.z += rotationSpeed * Time.deltaTime;
            //}
        }

        
    }
    public bool OnInteract()
    {

        if (includePlayer) return false;

        CancelInvoke();
        _rigidbody.isKinematic = true;
        isInteracting = true;
        if (!notControlTrigger)
        {
            _collider.isTrigger = true;
        }

        initialPlayerRot = CharacterManager.Instance.Player.transform.eulerAngles;
        initialRot = transform.eulerAngles;

        originalScale = transform.localScale.x;
        originalDistance = Vector3.Distance(transform.position, cameraTransform.position); // 트러블슈팅 월드좌표

        return true;
    }
    public void EndInteract()
    {
        _rigidbody.isKinematic = false;
        isInteracting = false;
        if (!notControlTrigger)
        {
            _collider.isTrigger = false;
        }

        RotX = false;
        RotY = false;
        RotZ = false;

        Invoke("Initialize", 10f);
    }

    private void Resize()
    {
        RaycastHit hit;
        if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit, Mathf.Infinity, ignoreTargetMask))
        {
            
            transform.position = hit.point - offsetFactor * targetScale.x * cameraTransform.forward;
            //float factor = 0.1f;
            //do
            //{
            //    transform.position = hit.point - factor * targetScale.x * cameraTransform.forward;

            //    factor += 0.1f;

            ////    if (factor >= 2.0f) break;
            //} while (isCollide);
            //Debug.Log(factor);

            float currentDistance = Vector3.Distance(cameraTransform.position, transform.position);

            float s = currentDistance / originalDistance;
            targetScale.x = targetScale.y = targetScale.z = s;
            //s = Mathf.Clamp(s * originalScale, minScale, maxScale);
            transform.localScale = targetScale * originalScale;

            float scaleX = transform.localScale.x;
            if (scaleX < minScale)
            {
                transform.localScale = new Vector3(minScale, minScale, minScale);
            }
            else if (scaleX > maxScale)
            {
                transform.localScale = new Vector3(maxScale, maxScale, maxScale);
            }


            if (Physics.CheckBox(transform.position, transform.localScale * 0.5f, Quaternion.identity, groundMask))
            {
                // CheckBox 충돌 후 새로 Raycast로 법선을 얻어 충돌 보정
                if (Physics.Raycast(transform.position, Vector3.down, out hit, Mathf.Infinity, groundMask))
                {
                    Vector3 correctionDirection = hit.normal; // 새로운 충돌 법선
                    transform.position += correctionDirection * offsetFactor * targetScale.x * 0.01f;
                }
            }


        }
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if(IsLayerMatched(other.gameObject.layer,groundMask))
    //    {
    //        collideList.Add(other.gameObject);
    //    }
    //    if (collideList.Count > 0) isCollide = true;
    //}
    //private void OnTriggerExit(Collider other)
    //{
    //    if (IsLayerMatched(other.gameObject.layer, groundMask))
    //    {
    //        if (collideList.Contains(other.gameObject))
    //        {
    //            collideList.Remove(other.gameObject);
    //        }
    //    }
    //    if (collideList.Count <= 0) isCollide = false;
    //}
    private void fixAngle()
    {
        transform.eulerAngles = initialRot + CharacterManager.Instance.Player.transform.eulerAngles - initialPlayerRot; 
    }

    public void RotateX()
    {
        RotX = true;
    }
    public void RotateY()
    {
        RotY = true;
    }

    //public void RotateZ()
    //{
    //    RotZ = true;
    //}
    public void EndRotateX()
    {
        RotX = false;
    }
    public void EndRotateY()
    {
        RotY = false;
    }
    //public void EndRotateZ()
    //{
    //    RotZ = false;
    //}


    private bool IsLayerMatched(int layerMask, int objectLayer)
    {
        return layerMask == (layerMask | (1 << objectLayer));
    }

    private void OnTriggerEnter(Collider other)
    {
        Player player = other.GetComponent<Player>();
        if (!player) return;
        includePlayer = true;

        

        EndInteract();
        for (int i = 0; i < _objects.Count; i++)
        {
            _objects[i].layer = groundLayer;
        }
        for (int i = 0; i < InteracableObjects.Count; i++)
        {
            InteracableObjects[i].layer = interactableLayer;
        }

    }

    private void OnTriggerExit(Collider other)
    {
        Player player = other.GetComponent<Player>();
        if (!player) return;
        includePlayer = false;


        for (int i = 0; i < _objects.Count; i++)
        {
            _objects[i].layer = offGroundLayer;
        }
        for (int i = 0; i < InteracableObjects.Count; i++)
        {
            InteracableObjects[i].layer = offGroundLayer;
        }
    }

    public void Initialize()
    {
        if(parentTransform != null && parentTransform != transform.parent)
        {
            transform.parent = parentTransform;
        }
        transform.localPosition = initialPos;
        transform.localScale = initialScale;
        transform.eulerAngles = initialRotation;
    }
}
