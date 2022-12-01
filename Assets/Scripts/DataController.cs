using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

// json ���Ϸ� ������ ������ Ŭ����
[Serializable]
public class GameData
{
    public int[] picture = { 0, 0, 0, 0, 0, 0, 0 }; // ���� ���� Ƚ��
}

public class DataController : MonoBehaviour
{
    private string DataFileName = "Data.json";  // json ���� �̸�

    static GameObject _container;
    static GameObject container
    {
        get
        {
            return _container;
        }
    }

    static DataController _instance;
    public static DataController instance
    {
        get
        {
            if (!_instance)
            {
                _container = new GameObject();
                _container.name = "DataController";
                _instance = _container.AddComponent(typeof(DataController)) as DataController;
                DontDestroyOnLoad(_container);  // scene�� �̵��ص� game object ����
            }
            return _instance;
        }
    }

    // json ���Ϸ� ������ ��ü
    private GameData _data;
    public GameData data
    {
        get
        {
            if (_data == null)
            {
                LoadGameData();
                SaveGameData();
            }
            return _data;
        }
    }


    // json ���� �ҷ�����
    private void LoadGameData()
    {
        string filePath = Application.persistentDataPath + "/" + DataFileName;
        Debug.Log(filePath);

        if (File.Exists(filePath))
        {
            Debug.Log("������ �ҷ�����");
            string FromJsonData = File.ReadAllText(filePath);
            _data = JsonUtility.FromJson<GameData>(FromJsonData);
        }
        else
        {
            Debug.Log("���ο� ������ ���� ����");
            _data = new GameData();
        }
    }

    // json ���Ϸ� ����
    private void SaveGameData()
    {
        string ToJsonData = JsonUtility.ToJson(data);
        string filePath = Application.persistentDataPath + "/" + DataFileName;
        File.WriteAllText(filePath, ToJsonData);
        Debug.Log("������ ���� �Ϸ�");
    }

    // ���α׷� ���� �� ������ ����
    private void OnApplicationPause()
    {
        SaveGameData();
    }

    // ���α׷� ���� �� ������ ����
    private void OnApplicationQuit()
    {
        SaveGameData();
    }
}
