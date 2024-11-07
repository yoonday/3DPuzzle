using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartButton : MonoBehaviour
{
    private void Start()
    {
        SceneManager.sceneLoaded -= CharacterSpawn;
        SceneManager.sceneLoaded += CharacterSpawn;
    }

    public void OnStage1()
    {
        GameManager.Instance.stageNum = 0;      
        SceneManager.LoadScene("Stage");
        DataManager.Instance.data.isComplete[1] = false;
        DataManager.Instance.data.isComplete[2] = false;
        DataManager.Instance.data.isComplete[3] = false;

        //CharacterManager.Instance.Player.transform.position = DataManager.Instance.data.respawnPoint[0];
        //DataManager.Instance.LoadCheckPoint();
    }

    public void OnStage2()
    {
        GameManager.Instance.stageNum = 1;
        DataManager.Instance.data.isComplete[2] = false;
        DataManager.Instance.data.isComplete[3] = false;
        SceneManager.LoadScene("Stage");
    }

    public void OnStage3()
    {
        GameManager.Instance.stageNum = 2;
        DataManager.Instance.data.isComplete[3] = false;
        SceneManager.LoadScene("Stage");
    }

    void CharacterSpawn(Scene scene, LoadSceneMode mode)
    {
        if(scene.name == "Stage")
            CharacterManager.Instance.Player.transform.position = DataManager.Instance.data.respawnPoint[GameManager.Instance.stageNum];
    }
}
