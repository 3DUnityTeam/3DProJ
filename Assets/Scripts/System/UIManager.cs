using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject PauseUI;
    public GameObject CollectUI;
    public GameObject BattleUI;
    public GameObject ResultUIDefeat;
    public GameObject ResultUIVictory;
    public GameObject DeadUI;
    public Image[] WeaponIcons;

    public void Init()
    {
        PauseUI.SetActive(false);
        BattleUI.SetActive(false);
        ResultUIDefeat.SetActive(false);
        ResultUIVictory.SetActive(false);
        CollectUI.SetActive(true);
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.anyKey)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (PauseUI.activeSelf)
                {
                    GameManager.instance.StopManager.TimePass();
                    PauseUI.SetActive(false);
                }
                else
                {
                    GameManager.instance.StopManager.TimeStop();
                    PauseUI.SetActive(true);
                }
            }
        }
        if (PauseUI.activeSelf || ResultUIDefeat.activeSelf || ResultUIVictory.activeSelf || CollectUI.activeSelf)
        {
            GameManager.instance.isCursorLocked = false;
        }
    }

    public void Fadeinout(bool inout)
    {
        //true : 화면 하얗지 않게 false:화면 하얗게
        if (inout == true)
        {
            BattleUI.GetComponent<BattleUI>().fadeIn.state = FadeIn.State.fadein;
        }
        else if (inout == false)
        {
            BattleUI.GetComponent<BattleUI>().fadeIn.state = FadeIn.State.fadeout;
        }

    }
    public void FinshGame(bool result)
    {
        //true: 승리, false: 패배
        /*
        if(!result)
        {
            GameManager.instance.StopManager.TimeStop();
        }
        */
        BattleUI.SetActive(false);
        CollectUI.SetActive(false);
        switch(result)
        {
            case true:
                ResultUIVictory.SetActive(true);
                break;
            case false:
                GameManager.instance.StopManager.TimeStop();
                ResultUIDefeat.SetActive(true);
                break;
            default:
                return;
                break;
        }
    }

    public void Dead(float deadtime)
    {
        StartCoroutine(stopdead(deadtime));
        DeadUI.SetActive(true);
        DeadUI.GetComponent<DeadUI>().Dead(deadtime);
    }

    IEnumerator stopdead(float deadtime)
    {
        yield return new WaitForSeconds(deadtime);
        DeadUI.SetActive(false);
    }
}
