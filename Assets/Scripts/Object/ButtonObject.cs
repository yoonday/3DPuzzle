using UnityEngine;

public interface IButtonLinkedObject
{
    public void OnButton();
    public void EndButton();
}
public class ButtonObject : MonoBehaviour
{
    [SerializeField] private LayerMask objectLayer;
    [SerializeField] private GameObject linkedObject;
    private IButtonLinkedObject buttonLinkedObject;
    private void Start()
    {
        buttonLinkedObject = linkedObject.GetComponent<IButtonLinkedObject>();     
    }
    private void OnTriggerEnter(Collider other)
    {
        if(IsLayerMatched(objectLayer, other.gameObject.layer))
        {
            buttonLinkedObject.OnButton();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (IsLayerMatched(objectLayer, other.gameObject.layer))
        {
            buttonLinkedObject.EndButton();
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (IsLayerMatched(objectLayer, other.gameObject.layer))
        {
            buttonLinkedObject.OnButton();
        }
    }

    private bool IsLayerMatched(int layerMask, int objectLayer)
    {
        return layerMask == (layerMask | (1 << objectLayer));
    }
}