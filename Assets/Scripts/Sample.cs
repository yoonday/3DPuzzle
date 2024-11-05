using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sample : MonoBehaviour
{
    DataManager dataManager;
    private void Start()
    {
        DataManager.Instance.LoadStage(0);
    }

    public void Test(int test)
    {
        DataManager.Instance.LoadStage(test);
    }

    public void Restart()
    {
        Debug.Log("´­¸²");
    }
}
