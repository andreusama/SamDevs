using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

[Serializable]
public class PlayerData
{
    public float actualMaxHp = 0f;
    public float lastMaxHp = 0f;
    public float actualLife = 0f;
    public FloorsList floorSerializable = new FloorsList();
    public PlayerData(float _actualLife, float _actualXp, float _lastXp, FloorsList serializeFloors)
    {
        actualMaxHp = _actualXp;
        lastMaxHp = _lastXp;
        actualLife = _actualLife;
        floorSerializable = serializeFloors;
    }
}


public class InLevelSaveLoad : MonoBehaviour
{

    private static InLevelSaveLoad saveController;

    public static InLevelSaveLoad MySaveController
    {
        get
        {
            if (saveController == null)
            {
                saveController = FindObjectOfType<InLevelSaveLoad>();
            }
            return saveController;
        }


    }

    public TextAsset saveJSON;
    string path = "";
    public bool newGame = false;
    public bool loading = false;

    public bool started = false;
    // Start is called before the first frame update
    void Awake()
    {
        path = Application.persistentDataPath + "/SaveFile.json";
        newGame = true;
        Debug.Log("Loading!");
        //if (File.Exists(path))
        //{
        //    newGame = false;
        //    loading = true;
        //}
        //else
        //{
        //    newGame = true;
        //    Debug.Log("File doesn't exist");
        //}
    }
    void Start()
    {
        started = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (started == false && newGame == false)
        {
            //Load();
            started = true;
        }
        if(Input.GetKeyDown(KeyCode.Q))
        {
            Save();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            //Load();
        }
    }

    public void Save()
    {
        Debug.Log("Saved in:" + path);

        BinaryFormatter formatter = new BinaryFormatter();

        PlayerData data = new PlayerData(PlayerStats.MyInstance.Salud.Vidactual, buildController.MyBuildInstance.GetMaxHp(), buildController.MyBuildInstance.GetLastMaxHp(), buildController.MyBuildInstance.Save(path));

        string jsonString = JsonUtility.ToJson(data);

        Debug.Log(jsonString);

        File.WriteAllText(path, jsonString);
    }

    public void Load()
    {
        string fileContents = File.ReadAllText(path);

        PlayerData data = new PlayerData(0 ,0, 0, null);
        data = JsonUtility.FromJson<PlayerData>(fileContents);

        LifeLoad(data);

        buildController.MyBuildInstance.Load(data.floorSerializable);

        Debug.Log("Loaded actual XP:" + data.actualMaxHp);

        //use loading
        loading = false;
    }

    public void LifeLoad(PlayerData data)
    {
        buildController.MyBuildInstance.SetActualXp(data.actualMaxHp);
        buildController.MyBuildInstance.SetLastXp(data.lastMaxHp);
        PlayerStats.MyInstance.Salud.Vidactual = data.actualLife;
        PlayerStats.MyInstance.Salud.VidaM = Mathf.Round((data.actualMaxHp + ((data.actualMaxHp / 1.2f) - (data.lastMaxHp / 1.2f) + 10)));
    }
}
