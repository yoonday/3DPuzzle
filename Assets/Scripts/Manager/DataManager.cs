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

    private Dictionary<GameObject, ObjectState> originalStates = new Dictionary<GameObject, ObjectState>();

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
                DontDestroyOnLoad(container);
            }
            return _instance;
         }
    }

    private void Awake()
    {
        LoadData();
    }

    void Start()
    {
        // 씬에 있는 모든 오브젝트의 초기 위치 저장
        Transform[] allObjects = FindObjectsOfType<Transform>();

        foreach (var obj in allObjects)
        {
            if (obj.gameObject != CharacterManager.Instance.Player.gameObject || obj.gameObject != Camera.main.gameObject)
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
            print(filepath);
        }
    }

    public void LoadStage(int stageNum)
    {
        if (data.isComplete[stageNum])
        {
            SceneManager.LoadScene("Stage");
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

            //SceneManager.LoadScene("TestScene_Seo");
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
