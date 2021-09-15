﻿using System;


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackCheckpoints : MonoBehaviour
{
    public event EventHandler OnPlayerCorretCheckpoint;
    public event EventHandler OnPlayerWrongCheckpoint;

    private List<CheckpointSingle> checkpointSingleList;
    private List<int> nextCheckpointSingleIndexList;    //각 car 단위 다음 체크포인트 위치값 int List 

    public int laps = 3;
    //각 차량별 현재 laps
    private List<int> currentLapsList;

    [SerializeField] private List<Transform> carTransformList;
    [SerializeField] private WatchController watchController;


    private void Awake()
    {
        Debug.Log("체크포인트 개수: "+ transform.childCount);
        checkpointSingleList = new List<CheckpointSingle>();
        //모든 자식Checkpoint 와 TrackCheckpoints 연동 
        for(int i=0; i < transform.childCount; ++i)
        {
            checkpointSingleList.Add(transform.GetChild(i).GetComponent<CheckpointSingle>());
            checkpointSingleList[i].SetTrackCheckpoints(this);
        }
        nextCheckpointSingleIndexList = new List<int>();
        currentLapsList = new List<int>();

        //직렬화 등록한 자동차 트랜스폼=vehicleControll 있는 transform 체크포인트 리스트 갱신
        foreach (Transform carTransform in carTransformList)
        {
            nextCheckpointSingleIndexList.Add(0);
            currentLapsList.Add(0);
        }
        //currentLaps 의 index 수를 carTransform 갯수와 동기화

        WatchController.RaceStartEvent += CanCarRaceStart;


    }
    private void CanCarRaceStart(object sender, EventArgs e)
    {
        foreach (Transform carTransform in carTransformList)
        {
            carTransform.GetComponent<VehicleControl>().canDrive = true;
        }
    }

    /// <summary>
    /// 자동차가 체크포인트 통과할 때 호출 (CheckpointSingle 에서 호출)
    /// 자동차가 적합한 체크포인트를 통과했는지 검사 (역주행방지)
    /// AI와 플레이어 모두에게 적용
    /// </summary>
    /// <param name="checkpointSingle">접촉한 checkpointSingle</param>
    /// <param name="carTransform">접촉한 car </param>
    public void CarThroughCheckpoint(CheckpointSingle checkpointSingle, Transform carTransform)
    {           //Ctrl + R 로 일괄적용
        int nextCheckpointSingleIndex = nextCheckpointSingleIndexList[carTransformList.IndexOf(carTransform)];
        Debug.Log(checkpointSingleList.IndexOf(checkpointSingle));

        if(checkpointSingleList.IndexOf(checkpointSingle) == nextCheckpointSingleIndex)
        {
            //Correct checkpoint
            Debug.Log("맞음");
            OnPlayerCorretCheckpoint?.Invoke(this, EventArgs.Empty);
            CheckpointSingle correctCheckpointSingle = checkpointSingleList[nextCheckpointSingleIndex];
            correctCheckpointSingle.Hide();

            //모든 리스트를 돌고 나면 0으로 초기화 
            nextCheckpointSingleIndexList[carTransformList.IndexOf(carTransform)]
                = (nextCheckpointSingleIndex + 1) % checkpointSingleList.Count;

            //0이 될 경우 laps + 1 (한 바퀴 완료한 것으로 판단)
            if(nextCheckpointSingleIndexList[carTransformList.IndexOf(carTransform)] == 0)
            {
                ++currentLapsList[carTransformList.IndexOf(carTransform)];
                //currentlaps( 0 에서 시작) 이 laps 와 같다면 경주 완료 
                if (laps <= currentLapsList[carTransformList.IndexOf(carTransform)])
                {
                    //해당 자동차 경주완료 이벤트 호출 
                    Debug.LogWarning(carTransformList.IndexOf(carTransform)+ "번 차 경주 완료");
                }
            }

            
        }
        else
        {
            //Wrong checkpoint
            Debug.Log("틀림");
            OnPlayerWrongCheckpoint?.Invoke(this, EventArgs.Empty);

            CheckpointSingle correctCheckpointSingle = checkpointSingleList[nextCheckpointSingleIndex];
            correctCheckpointSingle.Show();
        }
    }
}
