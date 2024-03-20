using System;
using System.IO;
using UnityEngine;
using UnityEngine.Events;


//게임 데이터저장용
public class DataManager : Singleton<DataManager>
{
    private GameData gameData;
    public GameData GameData { get { return gameData; } }

#if UNITY_EDITOR          //결합된 경로를 생성
    private string path => Path.Combine(Application.dataPath, $"Resources/Data/SaveLoad");
#else
    private string path => Path.Combine(Application.persistentDataPath, $"Resources/Data/SaveLoad");
#endif

    public void NewData()
    {
        gameData = new GameData();
    }

    public void SaveData(int index = 0)
    {
        //경로에 폴더가 있는지 확인
        if (Directory.Exists(path) == false)
        {
            //없으면 생성
            Directory.CreateDirectory(path);
        }
        //gameData를 json형식으로 변환
        string json = JsonUtility.ToJson(gameData, true);
                           //경로에    텍스트 작성한 파일 생성
        File.WriteAllText($"{path}/{index}.txt", json);
    }   //파일이 이미 존재하는 경우 해당 파일의 내용을 덮어쓰고, 파일이 없는 경우 새 파일을 만든다.

    public void LoadData(int index = 0)
    {   // 경로에 파일이있는지 확인
        if (File.Exists($"{path}/{index}.txt") == false)
        {
            NewData(); //없으면 새파일 생성
            return;
        }

        string json = File.ReadAllText($"{path}/{index}.txt");
        try
        {
            gameData = JsonUtility.FromJson<GameData>(json);
        }
        catch (Exception ex)
        {
            Debug.LogWarning($"Load data fail : {ex.Message}");
            NewData();
        }
    }

    public bool ExistData(int index = 0)
    {
        return File.Exists($"{path}/{index}.txt");
    }


    private void Start()
    {
        LoadData(0);
        LoadData(1);
        LoadData(2);
    }

    private void OnDestroy()
    {
        //SaveData(0);
        //SaveData(1);
        //SaveData(2);
    }
}
