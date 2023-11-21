using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public enum HUDType
    {
        Boss,
        Player,
        AP,
        Tofu,
        Timer
    }
    public HUDType type;
    Slider slider;
    Text text;

    bool result = false;
    int     min_result;
    int     sec_result;
    int millSec_result;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<Text>();
        slider = GetComponent<Slider>();
    }
    private void FixedUpdate()
    {
        GameManager manager = GameManager.instance;
        switch (type)
        {
            case HUDType.Player:
                slider.value = manager.player.HP / manager.player.MaxHP;
                break;
            case HUDType.Boss:
                GameObject nearBoss = null;
                float minDistance = manager.AimManager.maxDistance * 2;
                for (int i = 0; i < 1 + manager.SubBoss.Length; i++)
                {
                    if (i == 0)
                    {
                        float distance = Vector3.Distance(manager.player.transform.position, manager.Boss.transform.position);
                        MobParent bossSript = manager.Boss.GetComponent<MobParent>();
                        if (minDistance > distance && bossSript.HP<bossSript.MaxHP)
                        {
                            nearBoss = manager.Boss;
                            minDistance = distance;
                        }
                    }
                    else
                    {
                        float distance = Vector3.Distance(manager.player.transform.position, manager.SubBoss[i - 1].transform.position);
                        MobParent bossSript = manager.SubBoss[i - 1].GetComponent<MobParent>();
                        if (minDistance > distance && bossSript.HP < bossSript.MaxHP)
                        {
                            nearBoss = manager.SubBoss[i - 1];
                            minDistance = distance;
                        }
                    }
                }
                if (minDistance < manager.AimManager.maxDistance * 1.3f)
                {
                    int childCount = gameObject.transform.childCount;
                    for (int j = 0; j < childCount; j++)
                    {
                        Transform child = gameObject.transform.GetChild(j);
                        child.gameObject.SetActive(true);
                    }
                    MobParent nearSript = nearBoss.GetComponent<MobParent>();

                    //색깔 지정
                    Transform fillArea=gameObject.transform.Find("Fill Area");
                    Transform fill=fillArea.transform.Find("Fill");
                    Image sliderColor = fill.gameObject.GetComponent<Image>();
                    Transform background=gameObject.transform.Find("Background");
                    Image backgroundColor = background.gameObject.GetComponent<Image>();
                    if (nearSript.personalColor != null)
                    {
                        sliderColor.color = nearSript.personalColor;
                        backgroundColor.color = nearSript.personalColor;
                    }
                    //체력 조정
                    float hp = nearSript.HP;
                    float maxHP = nearSript.MaxHP;
                    slider.value = (maxHP - hp) / maxHP;
                }
                else
                {
                    int childCount = gameObject.transform.childCount;
                    for (int j = 0; j < childCount; j++)
                    {
                        Transform child = gameObject.transform.GetChild(j);
                        child.gameObject.SetActive(false);
                    }
                }
                break;
            case HUDType.AP:
                slider.value = manager.player.AP / manager.player.MaxAP;
                break;
            case HUDType.Tofu:
                //00:00.00
                float timer = GameManager.instance.tofuFoolr.HP;
                int min = Mathf.FloorToInt(timer / 60);
                int sec = Mathf.FloorToInt(timer % 60);
                int millSec= Mathf.FloorToInt((timer % 60 - Mathf.FloorToInt(timer % 60))*100);
                float count = manager.tofuFoolr.attackCount;
                text.text= string.Format("{0:D2}:{1:D2}.{2:D2}(ⅹ{3:#.#})", min, sec, millSec,count+1);
                break;
            case HUDType.Timer:
                //00:00.00
                if (!result)
                {
                    result = true;
                    float timer_result = GameManager.instance.tofuFoolr.HP;
                    min_result = Mathf.FloorToInt(timer_result / 60);
                    sec_result = Mathf.FloorToInt(timer_result % 60);
                    millSec_result = Mathf.FloorToInt((timer_result % 60 - Mathf.FloorToInt(timer_result % 60)) * 100);
                }
                text.text = string.Format("{0:D2}:{1:D2}.{2:D2}", min_result, sec_result, millSec_result);
                break;
        }
    }
}
