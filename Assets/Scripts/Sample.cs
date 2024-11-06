using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sample : MonoBehaviour
{
    private void Start()
    {
        //DataManager.Instance.LoadStage(0);
    }

    public void Test(int test)
    {
        DataManager.Instance.LoadStage(test);
    }

    public void Restart()
    {
        Debug.Log("´­¸²");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            DataManager.Instance.LoadCheckPoint();
        }
    }
}
