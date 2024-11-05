using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System.IO;
using System.ComponentModel;

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

    string GameDataFileName = "GameData.json";

    public Data data = new Data();

    public void LoadGameData()
    {
        string filepath = Application.persistentDataPath + "/" + GameDataFileName;

        if(File.Exists(filepath))
        {
            string FromJsonData = File.ReadAllText(filepath);
            data = JsonUtility.FromJson<Data>(FromJsonData);
            print(filepath);

            for (int i = 0; i < data.isComplete.Length; i++)
            {
                if (data.isComplete[i] && data.respawnPoint[i] != Vector3.zero)
                {
                    CharacterManager.Instance.Player.transform.position = data.respawnPoint[i];
                }
            }
        }
    }

    public void SaveGameData()
    {
        string ToJsonData = JsonUtility.ToJson(data, true);
        string filePath = Application.persistentDataPath + "/" + GameDataFileName;

        File.WriteAllText(filePath, ToJsonData);

        print(filePath);       
    }
}
