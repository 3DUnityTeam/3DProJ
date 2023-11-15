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
    //인게임 플레이어
    public Tofu tofuFoolr;
    public Player player;
    //초점
    public GameObject focus;
    //커서
    public bool isCursorLocked;

    [Header("#Manager")]
    //UI 매니저 
    public UIManager UIManager;
    //오디오 매니저
    public AudioManager AudioManager;
    //무기 매니저
    public WeaponManager WeaponManager;
    //에임 추적 매니저
    public AimManager AimManager;
    //시간 정지 매니저
    public StopManager StopManager;
    //총알 풀 매니저
    public PoolManager bulletPoolManger;
    //이펙트 풀 매니저
    public PoolManager effectPoolManger;
    //스폰 매니저
    public SpawnManager SpawnManager;

    [Header("#WeaponImage")]
    //무기 이미지
    public Sprite[] WeaponImages;
    public Sprite[] SpecialWeaponImages;
    public Sprite BaseImage;

    private void Awake()
    {
        instance = this;
        Init();
        // 마우스 커서를 중앙으로 고정
    }
    //매니저들 초기화
    /// <summary>
    /// 게임 매니저에서 매니저를 초기화하는 이유는 
    /// 각 매니저에서 Awake를 할 경우 불러오는 순서가 존재하여 문제가 생기기 때문이다
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
        //커서 중앙 잠금 구현
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
