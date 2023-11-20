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
        Tofu
    }
    public HUDType type;
    Slider slider;
    Text text;

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
                        if (minDistance > distance)
                        {
                            nearBoss = manager.Boss;
                            minDistance = distance;
                        }
                    }
                    else
                    {
                        float distance = Vector3.Distance(manager.player.transform.position, manager.SubBoss[i - 1].transform.position);
                        if (minDistance > distance)
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
                    if (nearSript.personalColor != null)
                    {
                        sliderColor.color = nearSript.personalColor;
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
                float tofu_hp = manager.tofuFoolr.HP;
                float tofu_maxHP = manager.tofuFoolr.MaxHP;
                if (tofu_hp > 0)
                {
                    text.text = string.Format("{0:F1}", tofu_hp / tofu_maxHP * 100) + "%";
                }
                else
                {
                    text.text = 0+"%";
                }
                break;
        }
    }
}
