using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RetryObject : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Item")  || collision.gameObject.CompareTag("Player")  )
        {
            DataManager.Instance.LoadCheckPoint();
        }
    }
}
