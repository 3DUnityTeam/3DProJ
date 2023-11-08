using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopManager : MonoBehaviour
{
    public void TimeStop()
    {
        Time.timeScale = 0f;
    }
    public void TimePass()
    {
        GameManager manager = GameManager.instance;
        if (manager.UIManager != null)
        {
            if (manager.UIManager.CollectUI.activeSelf && manager.UIManager.PauseUI.activeSelf)
                return;
            if (manager.UIManager.ResultUI.activeSelf && manager.UIManager.PauseUI.activeSelf)
                return;
        }

        Time.timeScale = 1f;
    }
}
