using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public float tofuMaxHp =100f;
    [Range(0,100)]
    public float tofuHp=80f;

    public float bossMaxHp=100f;
    [Range(0,100)]
    public float bossHp = 50f;

    [Header("#Player")]
    //�ΰ��� �÷��̾�
    public Player player;
    //����
    public GameObject focus;
    
    [Header("#Manager")]
    //UI �Ŵ��� 
    public UIManager UIManager;
    //����� �Ŵ���
    public AudioManager AudioManager;
    //���� �Ŵ���
    public WeaponManager WeaponManager;
    //���� ���� �Ŵ���
    public AimManager AimManager;
    //�ð� ���� �Ŵ���
    public StopManager StopManager;

    [Header("#WeaponImage")]
    //���� �̹���
    public Sprite[] WeaponImages;
    public Sprite BaseImage;

    private void Awake()
    {
        instance = this;
        Init();
        // ���콺 Ŀ���� �߾����� ����
    }
    //�Ŵ����� �ʱ�ȭ
    /// <summary>
    /// ���� �Ŵ������� �Ŵ����� �ʱ�ȭ�ϴ� ������ 
    /// �� �Ŵ������� Awake�� �� ��� �ҷ����� ������ �����Ͽ� ������ ����� �����̴�
    /// </summary>
    void Init()
    {
        if (AudioManager != null)
            AudioManager.Init();
        if(UIManager!=null)
            UIManager.Init();
    }
    private void Start()
    {
        StopManager.TimeStop();
        AudioManager.PlayBgm(AudioManager.Bgm.Title);
        AudioManager.PlaySfx(AudioManager.Sfx.Dead);
    }

    public void Save(string key,float value)
    {
        PlayerPrefs.SetFloat(key, value);
    }
    public float Load(string key)
    {
        if (PlayerPrefs.HasKey(key))
        {
            return PlayerPrefs.GetFloat(key);
        }
        return -1;
    }
}
