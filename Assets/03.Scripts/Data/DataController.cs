using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class DataController : BaseMonoSingleton<DataController>
{
    private string DataFileName = "save.json"; // ���� ����

    private Data _data;
    public Data data
    {
        get
        {
            if (_data == null)
            {
                LoadData();
                SaveData();
            }
            return _data;
        }
    }

    private void Start()
    {
        DataController.Instance.LoadData();
    }
    //����� ������ �ҷ��ɴϴ�.
    public void LoadData()
    {
        string filePath = Application.persistentDataPath + DataFileName;

        if (File.Exists(filePath))
        {
            Debug.Log("Data Load Succes");
            string FromJsonData = File.ReadAllText(filePath);
            _data = JsonUtility.FromJson<Data>(FromJsonData);
        }
        else
        {
            Debug.Log("New Data File Create");
            _data = new Data();
        }
    }

    //������ ����
    public void SaveData()
    {
        string ToJsonData = JsonUtility.ToJson(data);
        string filePath = Application.persistentDataPath + DataFileName;
        File.WriteAllText(filePath, ToJsonData);
        Debug.Log("����Ϸ�");
    }

    private void OnApplicationQuit()
    {
        SaveData();
    }
}
