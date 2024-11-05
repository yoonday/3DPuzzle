using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sample : MonoBehaviour
{
    private void Start()
    {
        DataManager.Instance.LoadGameData();
    }

    private void OnApplicationQuit()
    {
        DataManager.Instance.SaveGameData();
    }

    public void Test(int test)
    {
        DataManager.Instance.SaveGameData();
    }

    public void Restart()
    {
        DataManager.Instance.LoadGameData();
        Debug.Log("´­¸²");
    }
}
