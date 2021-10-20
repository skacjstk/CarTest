using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScoreBoardController : MonoBehaviour
{
    private Text[] scores;
    private Image backGroundImage;
    private int raceEndPlaceCheck;
    private List<string> scoreList;

    private float raceTimeGotoSaveData;
    private Button mainBtn;
    private Text btnText;

    public PlayerSaveDataManager obj;
    List<Transform> carTransformAnotherList;
    /*
     public String playerName; 
     */


    private void Start()
    {
        scoreList = new List<string>();
        carTransformAnotherList = new List<Transform>();
        //현재 버그로 마지막 텍스트는 버튼의 텍스트

        mainBtn = GetComponentInChildren<Button>();

        mainBtn.gameObject.SetActive(false);
        scores = GetComponentsInChildren<Text>();
        for(int i=0; i<scores.Length; ++i)
        {
            scoreList.Add("");      //빈 칸으로 초기화
        }
        mainBtn.gameObject.SetActive(true);

        btnText = mainBtn.GetComponentInChildren<Text>();
        backGroundImage = GetComponentInChildren<Image>();
        Debug.Log(scores.Length);
        raceEndPlaceCheck = 0;
        HideScoreBoard();
        obj = FindObjectOfType<PlayerSaveDataManager>();
        if (obj)
            Debug.Log("세이브데이터 매니저 찾았음");
    }
    public void RaceEndScoreBoardShow()
    {
        ShowScoreBoard();
        SendScoreBoard();
        UserTimeSave();     //다음주 JSON 때 구현
    }
    //게임 종료시, 저장해뒀던 기록 정보를 scoreboard 로 전송
    private void SendScoreBoard()
    {
        for(int i = 0; i < scores.Length; ++i)
        {
            scores[i].text = scoreList[i];
        }
    }

    /// <summary>
    /// 처음 타임랩스 체크가 완료되었을때 출력 
    /// </summary>
    /// <param name="raceTime"></param>
    /// <param name="carTransform"></param>
    /// <param name="carTransformList"></param>
    public void RaceTimeAnyCast(float raceTime, Transform carTransform, List<Transform> carTransformList)
    {
        bool flag = true;
        foreach(Transform carT in carTransformAnotherList)
        {
            if (carT == carTransform)
                flag = false;                        
        }
        if (flag)
        {
            scoreList[raceEndPlaceCheck++] = textChange(raceTime);
            carTransformAnotherList.Add(carTransform);
        }
    }
    //float 을 기록에 맞는 text로 변환
    private string textChange(float raceTime)
    {
        raceTimeGotoSaveData = raceTime;
        int minute, second;
        float another;
        minute = (int)raceTime / 60;
        second = (int)raceTime % 60;
        another = raceTime - (minute * 60 + second);
        
        return string.Format("{0:D2}:{1:D2}:{2:N0}", minute, second, another * 1000);        
    }
    /// <summary>
    /// 유저 기록 JSON 파일에 저장(다음 주)
    /// </summary>
    private void UserTimeSave()
    {
        obj.NewRecordSave(raceTimeGotoSaveData);
    }
    public void MainLoad()
    {
        //기록 저장
        UserTimeSave();
        Debug.Log("메인화면으로");
        SceneManager.LoadScene("Lobby");
    }

    public void ShowScoreBoard()
    {
        for (int i = 0; i < scores.Length; ++i)
        {
            scores[i].enabled = true;
        }
        mainBtn.gameObject.SetActive(true);
        backGroundImage.enabled = true;

    }
    private void HideScoreBoard()
    {
        for (int i = 0; i < scores.Length; ++i)
        {
            scores[i].enabled = false;
        }
        mainBtn.gameObject.SetActive(false);
        backGroundImage.enabled = false;
    }
}
