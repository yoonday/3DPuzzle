using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCondition : MonoBehaviour
{
    Collider collider;

    [SerializeField] int clearConditionNum;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            DataManager.Instance.data.isComplete[clearConditionNum] = true;
            DataManager.Instance.data.respawnPoint[clearConditionNum] = transform.position;
            Debug.Log(DataManager.Instance.data.respawnPoint[clearConditionNum]);
        }
    }
}
