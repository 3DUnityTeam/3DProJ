using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressManager : MonoBehaviour
{
    GameManager gameManager;
    public GameObject[] Map;
    public GameObject[] Fires;
    public bool boss1Cleared = false;
    public bool boss2Cleared = false;
    public bool dragonCleared = false;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.instance;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if(boss1Cleared && boss2Cleared)
        {
            Map[0].SetActive(false);
            Map[1].SetActive(true);
        }
        if(dragonCleared)
        {
            gameManager.UIManager.FinshGame(true);
        }
    }

    void Clear(int i)
    {
        switch (i)
        {
            case 1:
                boss1Cleared = true;
                break;
            case 2:
                boss2Cleared = true;
                break;
            case 3:
                dragonCleared = true;
                break;
        }

        if(Fires[i-1] !=null)
        {
            Fires[i - 1].SetActive(true);
        }
    }
}
