using System;
using System.IO;
using UnityEngine;
using UnityEngine.Events;


//���� �����������
public class DataManager : Singleton<DataManager>
{
    private GameData gameData;
    public GameData GameData { get { return gameData; } }

#if UNITY_EDITOR          //���յ� ��θ� ����
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
        //��ο� ������ �ִ��� Ȯ��
        if (Directory.Exists(path) == false)
        {
            //������ ����
            Directory.CreateDirectory(path);
        }
        //gameData�� json�������� ��ȯ
        string json = JsonUtility.ToJson(gameData, true);
                           //��ο�    �ؽ�Ʈ �ۼ��� ���� ����
        File.WriteAllText($"{path}/{index}.txt", json);
    }   //������ �̹� �����ϴ� ��� �ش� ������ ������ �����, ������ ���� ��� �� ������ �����.

    public void LoadData(int index = 0)
    {   // ��ο� �������ִ��� Ȯ��
        if (File.Exists($"{path}/{index}.txt") == false)
        {
            NewData(); //������ ������ ����
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
