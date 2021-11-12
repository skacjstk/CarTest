using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PlayerSaveDataManager : MonoBehaviour
{
    string fileName;
    string path;
    public int currentSelectedMapCode = 0;
    [System.Serializable]
    public class PlayerDataClass
    {
        public List<float> playerRecordList;
    }
    private void Awake()
    {
        var obj = FindObjectsOfType<PlayerSaveDataManager>();
        if(obj.Length == 1)
        DontDestroyOnLoad(this.gameObject);
        else
            Destroy(this.gameObject);
    }
    private int currentMapCount = 2; //하드코딩
    //전역변수 플레이어데이터
    public PlayerDataClass myPlayerData = new PlayerDataClass();

    private void Start()
    {
        fileName = "PlayerRecord";
        path = Application.dataPath + "/" + fileName + ".Json";
        PlayerSaveDataCheck(fileName, path);
    }

    private void PlayerSaveDataCheck(string fileName, string path)
    {
        Load(fileName, path);

    }
    public void NewRecordSave(float record)
    {
        float beforeRecord = myPlayerData.playerRecordList[currentSelectedMapCode];
        //기록을 갈아치우자 (NewSave에서 각 기록을 -1로 저장했다.
        if (beforeRecord < 0 || beforeRecord > record)
        {
            myPlayerData.playerRecordList[currentSelectedMapCode] = record;
           // Debug.Log("신기록 달성");
        }
        Save(path);
    }
    private void Load(string fileName, string path)
    {
        string json;
        try
        {
            json = File.ReadAllText(path);
        }
        catch (FileNotFoundException e1)
        {
            Debug.Log(e1 + "오류: 파일이 없습니다. 파일을 새로 생성합니다");
            NewSave(fileName, path);
            json = File.ReadAllText(path);
        }

        myPlayerData = JsonUtility.FromJson<PlayerDataClass>(json);

    }
    private void Save(string path)
    {
        string json = JsonUtility.ToJson(myPlayerData);

        Debug.Log(json);

        File.WriteAllText(path, json);
    }
    public void Save()
    {
        string json = JsonUtility.ToJson(myPlayerData);

        Debug.Log(json);

        File.WriteAllText(path, json);
    }
    void NewSave(string fileName, string path)
    {
        myPlayerData.playerRecordList = new List<float>();
        for (int i = 0; i < currentMapCount; ++i)
        {
            myPlayerData.playerRecordList.Add(-1);
        }

        

        Save(path);
    }
}
