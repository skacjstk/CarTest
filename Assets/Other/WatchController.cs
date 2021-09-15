using System;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WatchController : MonoBehaviour
{
    public Text[] watch;

    public event EventHandler RaceStartEvent;
    private bool isRaceStart = false;
    public float startTime = 3;
    private float raceTime = 0;
    int minute = 0, second = 0;
    float another = 0;

    // Start is called before the first frame update
    void Start()
    {
        for(int i=0; i<transform.childCount; ++i)
        {
            watch = GetComponentsInChildren<Text>();
        }
        StartCoroutine("PreRace");
    }

    // Update is called once per frame
    void Update()
    {
        //0번, startWatch, 1번 RaceWatch
        if (isRaceStart)
        {
            Racing();
        }
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
            watch[0].enabled = false;
            RaceStart();        
    }
    /// <summary>
    /// 경주 시작 후, 완료까지 시간을 계산하는 것
    /// FixedUpdate 에서 계속 호출되는 것
    /// 
    /// 
    /// </summary>
    private void RaceStart()
    {
        isRaceStart = true;
        RaceStartEvent?.Invoke(this, EventArgs.Empty);
    }
    private void RaceEnd()
    {
        
    }

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

    public float GetRaceTime()
    {
        return raceTime;
    }
}
