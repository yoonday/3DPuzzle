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

    [SerializeField] private bool isCompareTag = false;
    [SerializeField] private string objectTag;
    private void Start()
    {
        buttonLinkedObject = linkedObject.GetComponent<IButtonLinkedObject>();     
    }
    //private void OnTriggerEnter(Collider other)
    //{
    //    if(IsLayerMatched(objectLayer, other.gameObject.layer))
    //    {
    //        if (isCompareTag && other.CompareTag(objectTag))
    //        {
    //            buttonLinkedObject.OnButton();
    //        }

    //        else if (!isCompareTag)
    //        {
    //            buttonLinkedObject.OnButton();
    //        }
    //    }
    //}

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
            if (isCompareTag && other.CompareTag(objectTag))
            {
                buttonLinkedObject.OnButton();
                Debug.Log("if");
            }

            else if (!isCompareTag)
            {
                buttonLinkedObject.OnButton();
                Debug.Log("else");
            }
        }
    }

    private bool IsLayerMatched(int layerMask, int objectLayer)
    {
        return layerMask == (layerMask | (1 << objectLayer));
    }
}