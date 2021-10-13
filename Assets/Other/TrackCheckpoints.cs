using System;


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
    private List<float> raceTimeList;
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
        raceTimeList = new List<float>();

        //직렬화 등록한 자동차 트랜스폼=vehicleControll 있는 transform 체크포인트 리스트 갱신
        foreach (Transform carTransform in carTransformList)
        {
            nextCheckpointSingleIndexList.Add(0);
            currentLapsList.Add(0);
            raceTimeList.Add(999.999f);
        }
        //currentLaps 의 index 수를 carTransform 갯수와 동기화

        watchController.RaceStartEvent += WatchController_CanCarRaceStart;


    }
    private void SendNextCheckpointSInglePos(Transform carTransform, int currentIndex)
    {
        carTransform.GetComponent<VehicleControl>().checkpointSinglePos = checkpointSingleList[currentIndex].transform.position;
    }
    private void WatchController_CanCarRaceStart(object sender, EventArgs e)
    {
        foreach (Transform carTransform in carTransformList)
        {
            carTransform.GetComponent<VehicleControl>().SetCanDrive(true);
            SendNextCheckpointSInglePos(carTransform, 0);
        }
    }
    /// <summary>
    /// 주행 종료 시 호출
    /// </summary>
    /// <param name="carTransform">종료 대상 car의 Transform</param>
    private void RaceEndTimeCheck(Transform carTransform)
    {
        carTransform.GetComponent<VehicleControl>().activeControl = false;
        raceTimeList[carTransformList.IndexOf(carTransform)] = watchController.GetRaceTime();

        Debug.Log("주행 기록:" + raceTimeList[carTransformList.IndexOf(carTransform)]);
        watchController.RaceTimeAnyCast(raceTimeList[carTransformList.IndexOf(carTransform)], carTransform, carTransformList);
        //스코어보드 보여주기 및 시간정지 
        if(carTransform.GetComponent<VehicleControl>().controlMode != ControlMode.AI)
            watchController.RaceEnd();
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
        //       Debug.Log(checkpointSingleList.IndexOf(checkpointSingle));


        if (checkpointSingleList.IndexOf(checkpointSingle) == nextCheckpointSingleIndex)
        {
            //Correct checkpoint
           // Debug.Log("맞음");
            CheckpointSingle correctCheckpointSingle = checkpointSingleList[nextCheckpointSingleIndex];

            //오직 UI를 위한 기능, AI가 아닐 때만
            if (carTransform.GetComponent<VehicleControl>().controlMode != ControlMode.AI)
            {
                OnPlayerCorretCheckpoint?.Invoke(this, EventArgs.Empty);
                correctCheckpointSingle.Hide();
            }
            //모든 리스트를 돌고 나면 0으로 초기화             
            nextCheckpointSingleIndexList[carTransformList.IndexOf(carTransform)]
                = (nextCheckpointSingleIndex + 1) % checkpointSingleList.Count;

            SendNextCheckpointSInglePos(carTransform, nextCheckpointSingleIndexList[carTransformList.IndexOf(carTransform)]);

            //걍 클리어해버리기(디버그 모드)
            RaceEndTimeCheck(carTransform);

            //0이 될 경우 laps + 1 (한 바퀴 완료한 것으로 판단)
            if(nextCheckpointSingleIndexList[carTransformList.IndexOf(carTransform)] == 0)
            {
                ++currentLapsList[carTransformList.IndexOf(carTransform)];
                //currentlaps( 0 에서 시작) 이 laps 와 같다면 경주 완료 
                if (laps <= currentLapsList[carTransformList.IndexOf(carTransform)])
                {
                    //해당 자동차 경주완료 이벤트 호출 
                    Debug.LogWarning(carTransformList.IndexOf(carTransform)+ "번 차 경주 완료");
                    RaceEndTimeCheck(carTransform);
                }
            }
        }
        else
        {
            //Wrong checkpoint
          //  Debug.Log("틀림");
            CheckpointSingle correctCheckpointSingle = checkpointSingleList[nextCheckpointSingleIndex];
            //AI가 아닐 때만 
            //오직 UI를 위한 기능
            if (carTransform.GetComponent<VehicleControl>().controlMode != ControlMode.AI)
            {
                OnPlayerWrongCheckpoint?.Invoke(this, EventArgs.Empty);
                correctCheckpointSingle.Show();
            }
   
        }
    }
}
