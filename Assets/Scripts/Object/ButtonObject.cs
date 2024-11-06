using System.Collections.Generic;
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
    [SerializeField] private int requireNumber = 1;

    [SerializeField] private bool isCompareTag = false;
    [SerializeField] private string objectTag;

    private List<GameObject> pushList;
    private void Start()
    {
        pushList = new List<GameObject>();
        buttonLinkedObject = linkedObject.GetComponent<IButtonLinkedObject>();     
    }
    private void OnTriggerEnter(Collider other)
    {
        if (IsLayerMatched(objectLayer, other.gameObject.layer))
        {
            if (!isCompareTag || isCompareTag && other.CompareTag(objectTag))
            {
                pushList.Add(other.gameObject);
                //buttonLinkedObject.OnButton();
            }
            if (pushList.Count >= requireNumber)
            {
                buttonLinkedObject.OnButton();
            }
            //else if (!isCompareTag)
            //{
            //    buttonLinkedObject.OnButton();
            //}
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (IsLayerMatched(objectLayer, other.gameObject.layer))
        {
            if (pushList.Contains(other.gameObject))
            {
                pushList.Remove(other.gameObject);
            }
            if (pushList.Count < requireNumber)
            {
                buttonLinkedObject.EndButton();
            }
            //buttonLinkedObject.EndButton();

        }
    }
    //private void OnTriggerStay(Collider other)
    //{
    //    if (IsLayerMatched(objectLayer, other.gameObject.layer))
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

    private bool IsLayerMatched(int layerMask, int objectLayer)
    {
        return layerMask == (layerMask | (1 << objectLayer));
    }
}