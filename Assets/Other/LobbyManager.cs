using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LobbyManager : MonoBehaviour
{
    private int currentUIDepth = 0;
    private int setupUIDepth = 0;
    private int removeUIDepth = 0;
    private int currentOpeningMap = 0;

    [System.Serializable]
    public struct UICollection
    {
        public Image plateImage;
        public Button[] plateBtns;
        public Button closeBtn;
    }
   [System.Serializable]
   public struct MapUICollection
    {
        public Image plateImage;
        public Button[] mapBtns;
        public Button closeBtn;
    }

    public UICollection[] lobbyUI;
    public MapUICollection[] MapUI;
    

    void awake()
    {
        //JSON 파일 로딩 
    }
    void Start()
    {
        setupZeroDepth();
    }
    private void setupZeroDepth()
    {
        currentUIDepth = setupUIDepth;

        //0번 Depth 빼고 false 
        for (int i = setupUIDepth + 1; i < lobbyUI.Length; ++i)
        {
            lobbyUI[i].plateImage.enabled = false;
            lobbyUI[i].closeBtn.gameObject.SetActive(false);
            for (int j = 0; j < lobbyUI[i].plateBtns.Length; ++j)
            {
                lobbyUI[i].plateBtns[j].gameObject.SetActive(false);
            }
        }

        for (int i = setupUIDepth; i < MapUI.Length; ++i)
        {
            MapUI[i].plateImage.enabled = false;
            MapUI[0].closeBtn.gameObject.SetActive(false); //close 버튼 닫기
            for (int j = 0; j < MapUI[i].mapBtns.Length; ++j)
            {
                MapUI[i].mapBtns[j].gameObject.SetActive(false);
            }
        }
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            currentUIDepth = Mathf.Clamp(--currentUIDepth, 0, lobbyUI.Length); // -1 ~ 3 사이의 값(그 범위를 넘어서지 않음)
        }
    }
    void FixedUpdate()
    {
        Debug.Log(currentUIDepth);
        //ESC 누를 때 각 동작

       

        DepthBaseSetupUI();
        DepthBaseSetRemoveUI();
    }
    private void DepthBaseSetupUI()
    {
        lobbyUI[currentUIDepth].plateImage.enabled = true;
        for (int j = 0; j < lobbyUI[currentUIDepth].plateBtns.Length; ++j)
        {
            lobbyUI[currentUIDepth].plateBtns[j].gameObject.SetActive(true);
        }
        lobbyUI[currentUIDepth].closeBtn.gameObject.SetActive(true);
    }
    private void DepthBaseSetRemoveUI()
    {
        removeUIDepth = currentUIDepth;        
        for (int i = removeUIDepth + 1; i < lobbyUI.Length; ++i)
        {
            lobbyUI[i].plateImage.enabled = false;
            lobbyUI[removeUIDepth + 1].closeBtn.gameObject.SetActive(false);
            for (int j = 0; j < lobbyUI[i].plateBtns.Length; ++j)
            {
                lobbyUI[i].plateBtns[j].gameObject.SetActive(false);
            }
        }
    }

    private bool IsQuit()
    {
        return true;
       // Do you want Quit Game ?  (미완성)
    }
    //1. 게임모드 선택
    //1-1. 게임모드 선택 UI 오픈, Play를 눌렀을 때
    public void SelectGameModeUI()
    {
        currentUIDepth++;  //현재 UI깊이는 1
    }
    //버튼에 할당
    //1-2. 버튼 클릭시 알맞은 파라미터와 함께 2단계 UI 출력
    public void SelectGameMode(int parameter)
    {
        switch (parameter)
        {
            case 0: //일반
                SelectMap(parameter);
                break;
            case 1: // 1인 타임어택
                SelectMap(parameter);
                break;
            default:
                break;

        }
    }
    //2. 선택된 게임모드에서 선택 가능한 맵 선택
    public void SelectMap(int parameter)
    {
      
        switch (parameter)
        {
            case 0:
                ShowMap(parameter);
                break;
            case 1:
                ShowMap(parameter);
                break;
            default:
                break;
        }
    }

    private void ShowMap(int parameter)
    {
        switch (parameter)
        {
            case 0:
                ShowMapUI(parameter);
                break;
            case 1:
                ShowMapUI(parameter);
                break;
            default:
                break;
        }
    }
    private void ShowMapUI(int parameter)
    {
        currentOpeningMap = parameter;
        MapUI[parameter].plateImage.enabled = true;
        for (int j = 0; j < MapUI[parameter].mapBtns.Length; ++j)
        {
            MapUI[parameter].mapBtns[j].gameObject.SetActive(true);
        }
        MapUI[parameter].closeBtn.gameObject.SetActive(true);
    }
    private void HideMapUI(int parameter)
    {
        MapUI[parameter].plateImage.enabled = false;
        for (int j = 0; j < MapUI[parameter].mapBtns.Length; ++j)
        {
            MapUI[parameter].mapBtns[j].gameObject.SetActive(false);
        }
        MapUI[parameter].closeBtn.gameObject.SetActive(false);
    }


    //3. 실제 씬 로드
    public void PlayGame(int i)
    {
        switch (i)
        {
            case 0: 
                SceneManager.LoadScene("CarMap1_1");
                break;
            case 1: 
                break;
            default:
                break;

        }
    }

    public void GameExit()
    {
        Debug.LogWarning("quit 호출됨: 에디터 상에서 무시");
        Application.Quit(0);
    }
    public void DepthCloseBtn()
    {
        currentUIDepth = Mathf.Clamp(--currentUIDepth, 0, lobbyUI.Length); // -1 ~ 3 사이의 값(그 범위를 넘어서지 않음)
    }
    public void MapCloseBtn()
    {
        HideMapUI(currentOpeningMap);
    }
}
