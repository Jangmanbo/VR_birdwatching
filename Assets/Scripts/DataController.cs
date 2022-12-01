using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

// json 파일로 저장할 데이터 클래스
[Serializable]
public class GameData
{
    public int[] picture = { 0, 0, 0, 0, 0, 0, 0 }; // 사진 찍은 횟수
}

public class DataController : MonoBehaviour
{
    private string DataFileName = "Data.json";  // json 파일 이름

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
                DontDestroyOnLoad(_container);  // scene을 이동해도 game object 유지
            }
            return _instance;
        }
    }

    // json 파일로 저장할 객체
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


    // json 파일 불러오기
    private void LoadGameData()
    {
        string filePath = Application.persistentDataPath + "/" + DataFileName;
        Debug.Log(filePath);

        if (File.Exists(filePath))
        {
            Debug.Log("데이터 불러오기");
            string FromJsonData = File.ReadAllText(filePath);
            _data = JsonUtility.FromJson<GameData>(FromJsonData);
        }
        else
        {
            Debug.Log("새로운 데이터 파일 생성");
            _data = new GameData();
        }
    }

    // json 파일로 저장
    private void SaveGameData()
    {
        string ToJsonData = JsonUtility.ToJson(data);
        string filePath = Application.persistentDataPath + "/" + DataFileName;
        File.WriteAllText(filePath, ToJsonData);
        Debug.Log("데이터 저장 완료");
    }

    // 프로그램 중지 시 데이터 저장
    private void OnApplicationPause()
    {
        SaveGameData();
    }

    // 프로그램 중지 시 데이터 저장
    private void OnApplicationQuit()
    {
        SaveGameData();
    }
}
