using System;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WatchController : MonoBehaviour
{
    public Text[] watch;
    private Image backGroundImage;

    public event EventHandler RaceStartEvent;
    private bool isRaceStart = false;
    public float startTime = 3;
    private float raceTime = 0;
    int minute = 0, second = 0;
    float another = 0;

    private bool timeToggle = true;

    private ScoreBoardController scoreBoard;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < transform.childCount; ++i)
        {
            watch = GetComponentsInChildren<Text>();
        }
        backGroundImage = GetComponentInChildren<Image>();
        scoreBoard = GetComponentInChildren<ScoreBoardController>();

        StartCoroutine("PreRace");
    }

    // Update is called once per frame
    void Update()
    {
        //0번, startWatch, 1번 RaceWatch
        if (isRaceStart)
        {
            Racing();
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                TimeToggle();
            }
        }
    }
    private void TimeToggle()
    {
        if (timeToggle)        
            TimeStop();        
        else        
            Remuse();        
        timeToggle = !timeToggle;
        //Show 일시정지 UI
    }
    private void TimeStop()
    {
        Time.timeScale = 0.0f;
    }
    private void Remuse()
    {
        Time.timeScale = 1.0f;
    }
    /// <summary>
    /// Start 시점에서 호출, 3,2,1 숫자 세고 땅 하기 직전까지 
    /// </summary>
    /// <returns></returns>
    IEnumerator PreRace()
    {
        while (startTime >= 0.0f)
        {
            startTime -= Time.deltaTime;
            watch[0].text = Mathf.Ceil(startTime).ToString();
            yield return null;
        }
        Hide();
        RaceStart();
    }
    private void Hide()
    {
        watch[0].enabled = false;
        backGroundImage.enabled = false;
    }

    private void RaceStart()
    {
        isRaceStart = true;
        RaceStartEvent?.Invoke(this, EventArgs.Empty);
    }
    /// <summary>
    /// 경주 시작 후, 완료까지 시간을 계산하는 것
    /// update에서 계속 호출되는 것
    /// 
    /// 
    /// </summary>
    private void Racing()
    {
        raceTime += Time.deltaTime;
        //  Debug.Log(raceTime);
        minute = (int)raceTime / 60;
        second = (int)raceTime % 60;
        //Debug.Log(minute +"\n"+ second);
        another = raceTime - (minute * 60 + second);
        watch[1].text = string.Format("{0:D2}:{1:D2}:{2:N0}", minute, second, another * 1000);
    }
    public void RaceEnd()
    {
        isRaceStart = false;
        scoreBoard.RaceEndScoreBoardShow();
    }
    public void RaceTimeAnyCast(float raceTime, Transform carTransform, List<Transform> carTransformList)
    {
        scoreBoard.RaceTimeAnyCast(raceTime, carTransform, carTransformList);
    }

    public float GetRaceTime()
    {
        return raceTime;
    }

}
