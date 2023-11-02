using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClickEvent : MonoBehaviour
{
    //베이스되는 UI
    public GameObject BaseUI;
    //옵션창
    public GameObject OptionUI;
    //Battle 씬으로 이동
    public void GoToBattle()
    {
        SceneManager.LoadScene("Battle");
    }
    //타이틀 씬으로 이동
    public void GoToTitle()
    {
        SceneManager.LoadScene("Title");
    }
    //설정창 열기(기존의 창 비활성화)
    public void OpenOption()
    {
        OptionUI.SetActive(true);
        BaseUI.SetActive(false);
    }
    //설정창 닫기(옵션의 창 비활성화)
    public void BackOption()
    {
        BaseUI.SetActive(true);
        OptionUI.SetActive(false);
    }
    //게임 종료 버튼
    public void ExitGame()
    {
        Debug.Log("test");
        Application.Quit();
    }

    //게임 시작
    public void StartGame()
    {
        Time.timeScale = 1f;
        gameObject.SetActive(false);
    }
}
