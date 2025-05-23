using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public interface IInteractable
{
    public bool OnInteract();
    public void EndInteract();
}

public class Interaction : MonoBehaviour
{
    public float checkRate = 0.05f;
    private float lastCheckTime;
    public float maxCheckDistance;
    public LayerMask layerMask;

    public GameObject curInteractGameObject;
    private IInteractable curInteractable;

    //[SerializeField] private float rotationSpeed = 0.5f;
    //private bool isRotateZ = false;
    //private bool isRotateY = false;
    private bool interacting = false;
    private Camera camera;

    public LayerMask ignoreTargetMask;

    private AudioSource audioSource;

    void Start()
    {
        camera = Camera.main;
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {

        if (Time.time - lastCheckTime > checkRate && !interacting)
        {
            lastCheckTime = Time.time;

            Ray ray = camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, maxCheckDistance, layerMask))
            {
                if (hit.collider.gameObject != curInteractGameObject)
                {
                    curInteractGameObject = hit.collider.gameObject;
                    curInteractable = hit.collider.GetComponent<IInteractable>();
                }
            }
            else
            {
                curInteractGameObject = null;
                curInteractable = null;
            }
        }

        //if (isRotateY)
        //{
        //    curInteractGameObject.transform.Rotate(0, rotationSpeed, 0);
        //}
        //if(isRotateZ)
        //{
        //    curInteractGameObject.transform.Rotate(0, 0, rotationSpeed);
        //}
    }



    public void OnInteractInput(InputAction.CallbackContext context)
    {
        if (curInteractable == null)
        {
            return;
        }

        if (context.phase == InputActionPhase.Started && curInteractable != null && !interacting)
        {
            if (curInteractable.OnInteract())
            {
                interacting = true;
                audioSource.Play();
            }
        }
        else if (context.phase == InputActionPhase.Started && interacting)
        {
            curInteractable.EndInteract();
            interacting = false;
            curInteractGameObject = null;
            curInteractable = null;
        }

    }

    //public void OnRotateZInput(InputAction.CallbackContext context)
    //{
    //    if (context.phase == InputActionPhase.Started && interacting)
    //    {
    //        //isRotateZ = true;
    //        curInteractGameObject.GetComponent<Object>().RotateZ();
    //    }
    //    if (context.phase == InputActionPhase.Canceled)
    //    {
    //        if (curInteractGameObject.TryGetComponent<Object>(out Object obj)) obj.EndRotateZ();
    //        //isRotateZ = false;
    //    }
    //}

    public void OnRotateYInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed && interacting)
        {
            curInteractGameObject.GetComponent<Object>().RotateY();
        }

        if (context.phase == InputActionPhase.Canceled && interacting)
        {
            if (curInteractGameObject.TryGetComponent<Object>(out Object obj)) obj.EndRotateY();
            //isRotateY = false;
        }
    }

    public void OnRotateXInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed && interacting)
        {
            curInteractGameObject.GetComponent<Object>().RotateX();
        }

        if (context.phase == InputActionPhase.Canceled && interacting)
        {
            if (curInteractGameObject.TryGetComponent<Object>(out Object obj)) obj.EndRotateX();
            //isRotateY = false;
        }
    }
}
