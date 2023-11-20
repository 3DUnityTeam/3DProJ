using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressManager : MonoBehaviour
{
    GameManager gameManager;
    public GameObject[] Map;
    public GameObject[] Fires;
    public Transform bossspawnpoint;
    public bool boss1Cleared = false;
    public bool boss2Cleared = false;
    public bool dragonCleared = false;

    private bool mapchanged = false;
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
            if(!mapchanged)
            {
                StartCoroutine(Mapchange());
            }
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

    public IEnumerator Mapchange()
    {
        mapchanged = true;
        //gameManager.StopManager.TimeStop();
        gameManager.UIManager.Fadeinout(false);
        yield return new WaitForSeconds(3f);
        Map[0].SetActive(false);
        Map[1].SetActive(true);
        gameManager.Boss.transform.position = bossspawnpoint.position;
        gameManager.Boss.GetComponent<DragonController>().enabled = true;
        //gameManager.StopManager.TimePass();
        gameManager.UIManager.Fadeinout(true);

    }
}
