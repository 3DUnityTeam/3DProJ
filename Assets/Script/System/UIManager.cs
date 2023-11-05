using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject PauseUI;
    public GameObject CollectUI;
    public GameObject BattleUI;
    public Image[] WeaponIcons;

    public void Init()
    {
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
                    Time.timeScale = 1f;
                    PauseUI.SetActive(false);
                }
                else
                {
                    Time.timeScale = 0f;
                    PauseUI.SetActive(true);
                }
            }
        }
        
    }
}
