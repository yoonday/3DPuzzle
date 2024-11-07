using UnityEngine;

public class Pathway : MonoBehaviour
{
    [SerializeField] private LayerMask objectLayer;
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == objectLayer)
        {
            other.gameObject.GetComponent<IInteractable>().EndInteract();
            if (DataManager.Instance.originalStates.TryGetValue(other.gameObject, out ObjectState objState))
            {
                Debug.Log(objState.ToString());
                other.transform.position = objState.position;
                other.transform.localScale = objState.scale;
            }
        }
    }
}