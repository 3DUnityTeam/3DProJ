using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject PauseUI;
    public GameObject CollectUI;
    public GameObject BattleUI;
    public GameObject ResultUI;
    public Image[] WeaponIcons;

    public void Init()
    {
        PauseUI.SetActive(false);
        BattleUI.SetActive(false);
        ResultUI.SetActive(false);
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
        if (PauseUI.activeSelf || ResultUI.activeSelf || CollectUI.activeSelf)
        {
            GameManager.instance.isCursorLocked = false;
        }
    }

    public void Fadeinout(bool inout)
    {
        //true : ȭ�� �Ͼ��� �ʰ� false:ȭ�� �Ͼ��
        if(inout == true)
        {
            BattleUI.GetComponent<BattleUI>().fadeIn.state = FadeIn.State.fadein;
        }
        else if(inout == false)
        {
            BattleUI.GetComponent<BattleUI>().fadeIn.state = FadeIn.State.fadeout;
        }
        
    }
    public void FinshGame(bool result)
    {
        //true: �¸�, false: �й�
        GameManager.instance.StopManager.TimeStop();
        BattleUI.SetActive(false);
        CollectUI.SetActive(false);
        ResultUI.SetActive(true);
    }
}
