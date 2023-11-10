using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public enum HUDType
    {
        Boss,
        Tofu,
        AP,
    }
    public HUDType type;
    Slider slider;

    // Start is called before the first frame update
    void Start()
    {
        slider = GetComponent<Slider>();
    }
    private void FixedUpdate()
    {
        GameManager manager = GameManager.instance;
        switch (type)
        {
            case HUDType.Tofu:
                slider.value = manager.player.HP / manager.player.MaxHP;
                break;
            case HUDType.Boss:
                slider.value = manager.bossHp / manager.bossMaxHp;
                break;
            case HUDType.AP:

                slider.value = manager.player.AP / manager.player.MaxAP;
                break;
        }
    }
}
