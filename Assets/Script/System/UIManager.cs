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
        
    }
}
