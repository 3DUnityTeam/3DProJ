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
                float hp = manager.Boss.GetComponent<DragonController>().HP;
                float maxHP= manager.Boss.GetComponent<DragonController>().MaxHP;
                slider.value = (maxHP-hp) / maxHP;
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
