using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCondition : MonoBehaviour
{
    Collider _collider;
    Data data;

    private void Start()
    {
        _collider = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            data.isComplete[0] = true;
            data.respawnPoint[0] = gameObject.transform.position;
        }
    }
}
