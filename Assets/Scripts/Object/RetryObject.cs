using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RetryObject : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        Object[] objects = GameObject.FindObjectsOfType<Object>();

        if (collision.gameObject.CompareTag("Player"))
        {
            DataManager.Instance.LoadCheckPoint();
        }

        if (collision.gameObject.CompareTag("Item"))
        {
            foreach (var obj in objects)
            {
                obj.Initialize();
            }
        }
    }
}
