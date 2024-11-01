using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sample : MonoBehaviour
{
    private void Start()
    {
        DataManager.Instance.LoadGameData();
        Test(3);
    }

    private void OnApplicationQuit()
    {
        DataManager.Instance.SaveGameData();
    }

    public void Test(int test)
    {
        DataManager.Instance.data.isComplete[test]= true;
        DataManager.Instance.SaveGameData();
    }
}
