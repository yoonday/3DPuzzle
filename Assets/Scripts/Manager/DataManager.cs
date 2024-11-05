using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System.IO;
using System.ComponentModel;
using UnityEngine.SceneManagement;

public class DataManager : MonoBehaviour
{
    static GameObject container;

    static DataManager _instance;

    public static DataManager Instance
    {
         get
         {
            if (!_instance)
            {
                container = new GameObject();
                container.name = "DataManager";
                _instance = container.AddComponent(typeof(DataManager)) as DataManager;
                DontDestroyOnLoad(container);
            }
            return _instance;
         }
    }

    private void Awake()
    {
        LoadData();
    }

    string GameDataFileName = "GameData.json";

    public Data data = new Data();

    public void LoadData()
    {
        string filepath = Application.persistentDataPath + "/" + GameDataFileName;

        if (File.Exists(filepath))
        {
            string FromJsonData = File.ReadAllText(filepath);
            data = JsonUtility.FromJson<Data>(FromJsonData);
            print(filepath);
        }
    }

    public void LoadStage(int stageNum)
    {
        if (data.isComplete[stageNum])
        {
            SceneManager.LoadScene("TestScene_Seo");
            CharacterManager.Instance.Player.transform.position = data.respawnPoint[stageNum];
        }
    }

    public void LoadCheckPoint()
    {          
        int lastCompletedStage = -1;

        for (int i = data.isComplete.Length - 1; i >= 0; i--)
        {
            if (data.isComplete[i])
            {
                lastCompletedStage = i;
                break;
            }
        }

        if (lastCompletedStage != -1)
        {
            SceneManager.LoadScene("TestScene_Seo");
            CharacterManager.Instance.Player.transform.position = data.respawnPoint[lastCompletedStage];
        }     
    }

    public void SaveGameData()
    {
        string ToJsonData = JsonUtility.ToJson(data, true);
        string filePath = Application.persistentDataPath + "/" + GameDataFileName;

        File.WriteAllText(filePath, ToJsonData);
        print(filePath);       
    }

    private void OnApplicationQuit()
    {
        SaveGameData();
    }
}
