using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System.IO;
using System.ComponentModel;
using UnityEngine.SceneManagement;

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

    void Start()
    {
        // 씬에 있는 모든 오브젝트의 초기 위치 저장
        Transform[] allObjects = FindObjectsOfType<Transform>();
        data.isComplete[1] = true;
        Debug.Log(data.respawnPoint[1]);

        foreach (var obj in allObjects)
        {
            if (obj.gameObject != CharacterManager.Instance.Player || obj.gameObject != Camera.main)
            {
                originalStates[obj.gameObject] = new ObjectState(obj.localPosition, obj.localScale);
            }
        }
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
            //SceneManager.LoadScene("Stage");
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
            foreach (var pair in originalStates)
            {
                pair.Key.transform.localPosition = pair.Value.position;
                pair.Key.transform.localScale = pair.Value.scale;
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
