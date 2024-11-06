using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartButton : MonoBehaviour
{
    public void OnStage1()
    {
        GameManager.Instance.StageNum = 1;      
        SceneManager.LoadScene("TestScene_Seo");
        //DataManager.Instance.LoadData();
        //DataManager.Instance.data.isComplete[1] = false;
        //DataManager.Instance.data.isComplete[2] = false;
        //DataManager.Instance.data.isComplete[3] = false;
        
        ////CharacterManager.Instance.Player.transform.position = DataManager.Instance.data.respawnPoint[0];
        //DataManager.Instance.LoadCheckPoint();
    }
}
