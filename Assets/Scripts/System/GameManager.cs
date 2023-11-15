using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("#Player")]
    //�ΰ��� �÷��̾�
    public Tofu tofuFoolr;
    public Player player;
    //����
    public GameObject focus;
    //Ŀ��
    public bool isCursorLocked;

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
    //�Ѿ� Ǯ �Ŵ���
    public PoolManager bulletPoolManger;
    //����Ʈ Ǯ �Ŵ���
    public PoolManager effectPoolManger;
    //���� �Ŵ���
    public SpawnManager SpawnManager;

    [Header("#WeaponImage")]
    //���� �̹���
    public Sprite[] WeaponImages;
    public Sprite[] SpecialWeaponImages;
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
        if (UIManager != null)
        {
            isCursorLocked = true;
        }
        StopManager.TimeStop();
        AudioManager.PlayBgm(AudioManager.Bgm.Title);
        AudioManager.PlaySfx(AudioManager.Sfx.Dead);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C)){
            isCursorLocked = !isCursorLocked;
        }
        if (UIManager != null)
        {
            player.AP = 100;
        }
    }

    private void LateUpdate()
    {
        if (tofuFoolr == null)
        {
            return;
        }
        DragonController dragon;
        if (tofuFoolr.HP <= 0)
        {
            UIManager.FinshGame(false);
        }
        else if (SpawnManager.Boss.TryGetComponent<DragonController>(out dragon))
        {
            if (dragon.HP >= dragon.MaxHP)
            {
                //UIManager.FinshGame(true);
            }
        }
        //Ŀ�� �߾� ��� ����
        Cursor.visible = !isCursorLocked;
        Cursor.lockState= !isCursorLocked?(CursorLockMode)0:(CursorLockMode)1;
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
