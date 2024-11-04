using UnityEngine;

public interface IButtonLinkedObject
{
    public void OnButton();
    public void EndButton();
}
public class ButtonObject : MonoBehaviour
{
    [SerializeField] private LayerMask objectLayer;
    [SerializeField] private IButtonLinkedObject linkedObject;


    private void OnTriggerEnter(Collider other)
    {
        if(IsLayerMatched(objectLayer, other.gameObject.layer))
        {
            linkedObject.OnButton();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (IsLayerMatched(objectLayer, other.gameObject.layer))
        {
            linkedObject.EndButton();
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (IsLayerMatched(objectLayer, other.gameObject.layer))
        {
            linkedObject.OnButton();
        }
    }

    private bool IsLayerMatched(int layerMask, int objectLayer)
    {
        return layerMask == (layerMask | (1 << objectLayer));
    }
}