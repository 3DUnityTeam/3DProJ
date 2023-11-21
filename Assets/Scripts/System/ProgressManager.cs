using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressManager : MonoBehaviour
{
    GameManager gameManager;
    public GameObject[] Map;
    public GameObject[] Fires;
    public GameObject circle;
    public GameObject ready;
    public GameObject Breath;
    
    public GameObject EndingPoz;
    public GameObject player;

    public Transform bossspawnpoint;
    public Transform playerspawnpoint;
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
            player.transform.position = EndingPoz.transform.position;
            gameManager.UIManager.FinshGame(true);
        }
    }

    public void Clear(int i)
    {
        switch (i)
        {
            case 1:
                boss1Cleared = true;
                gameManager.statemessage.MassageState("뱀이 두부맛에 행복해해요!");
                break;
            case 2:
                boss2Cleared = true;
                gameManager.statemessage.MassageState("쥐가 두부맛에 행복해해요!");
                break;
            case 3:
                dragonCleared = true;
                break;
        }
        int a = i - 1;
        if(Fires[a] !=null)
        {
            Fires[a].SetActive(false);
        }
    }

    public IEnumerator Mapchange()
    {
        mapchanged = true;
        yield return new WaitForSeconds(3f);
        gameManager.AudioManager.StopBgm();
        gameManager.statemessage.MassageState("드래곤이 보다못해 화가 난 것 같아요!");
        yield return new WaitForSeconds(1.2f);
        circle.SetActive(true);
        yield return new WaitForSeconds(2.3f);
        gameManager.statemessage.MassageState("드래곤이 두부를 진심으로 해치려 해요!");
        ready.SetActive(true);
        yield return new WaitForSeconds(1.2f);
        Breath.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        gameManager.UIManager.Fadeinout(false);
        yield return new WaitForSeconds(3f);
        Map[0].SetActive(false);
        //불 전부 비활성화
        circle.SetActive(false);
        ready.SetActive(false);
        Breath.SetActive(false);
        Map[1].SetActive(true);
        gameManager.Boss.transform.position = bossspawnpoint.position;
        gameManager.Boss.GetComponent<DragonController>().enabled = true;
        gameManager.player.gameObject.SetActive(true);
        gameManager.player.gameObject.transform.position = playerspawnpoint.position;
        gameManager.UIManager.Fadeinout(true);
        /*GameObject deadeffect = GameManager.instance.effectPoolManger.Get(7 - 1);
        deadeffect.transform.position = gameManager.player.gameObject.transform.position;
        gameManager.player.gameObject.SetActive(false);
        StartCoroutine(RevivePly(3));*/
        yield return new WaitForSeconds(3f);
        gameManager.AudioManager.PlayBgm(AudioManager.Bgm.Page2);
        gameManager.statemessage.MassageState("두부를 지켜야 합니다!");

    }


    IEnumerator RevivePly(float revivetime)
    {
        GameManager.instance.UIManager.Dead(revivetime);
        yield return new WaitForSeconds(revivetime);
        gameManager.player.gameObject.SetActive(true);
        gameManager.player.GetComponent<Rigidbody>().velocity = Vector3.zero;
        gameManager.player.HP = gameManager.player.MaxHP;
    }
}
