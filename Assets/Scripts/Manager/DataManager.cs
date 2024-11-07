using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System.IO;
using System.ComponentModel;
using UnityEngine.SceneManagement;
using System.Linq;

[System.Serializable]
public struct ObjectState
{
    public Vector3 position;
    public Vector3 scale;

    public ObjectState(Vector3 position, Vector3 scale)
    {
        this.position = position;
        this.scale = scale;
    }
}

public class DataManager : MonoBehaviour
{
    static GameObject container;
    static DataManager _instance;
    
    public Dictionary<GameObject, ObjectState> originalStates = new Dictionary<GameObject, ObjectState>();

    string GameDataFileName = "GameData.json";
    public Data data = new Data();

    public static DataManager Instance
    {
        get
        {
            if (!_instance)
            {
                container = new GameObject();
                container.name = "DataManager";
                _instance = container.AddComponent(typeof(DataManager)) as DataManager;
            }
            return _instance;
        }
    }

    //public static DataManager Instance
    //{
    //    get
    //    {
    //        if (_instance == null)
    //        {
    //            _instance = new GameObject(nameof(DataManager)).AddComponent<DataManager>();
    //        }
    //        return _instance;
    //    }
    //}

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if (_instance != this)
            {
                Destroy(gameObject);
            }
        }
        
        LoadData();
    }

    public void LoadData()
    {
        string filepath = Application.persistentDataPath + "/" + GameDataFileName;

        if (File.Exists(filepath))
        {
            string FromJsonData = File.ReadAllText(filepath);
            data = JsonUtility.FromJson<Data>(FromJsonData);
        }
    }

    public void LoadStage(int stageNum)
    {
        if (data.isComplete[stageNum])
        {
            CharacterManager.Instance.Player.transform.position = data.respawnPoint[stageNum];
        }
    }

    public void LoadCheckPoint()
    {
        Object[] objects = GameObject.FindObjectsOfType<Object>();

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
            foreach (var obj in objects)
            {
                obj.Initialize();
            }

            CharacterManager.Instance.Player.transform.position = data.respawnPoint[lastCompletedStage];
        }
    }

    public void SaveGameData()
    {
        string ToJsonData = JsonUtility.ToJson(data, true);
        string filePath = Application.persistentDataPath + "/" + GameDataFileName;

        File.WriteAllText(filePath, ToJsonData);     
    }

    private void OnApplicationQuit()
    {
        SaveGameData();
    }
}
