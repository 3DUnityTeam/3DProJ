using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("#Boss")]
    public GameObject Boss;
    public GameObject[] SubBoss;

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
    public PoolManager SpawnManager;
    //진행 매니저
    public ProgressManager progressManager;

    [Header("#WeaponImage")]
    //무기 이미지
    public Sprite[] WeaponImages;
    public Sprite[] SpecialWeaponImages;
    public Sprite BaseImage;

    [Header("#UImessage")]
    //UI에 상황 메시지를 표기하는 공간
    public BattleUI statemessage;

    [Header("#BasicBGM")]
    public AudioManager.Bgm bgm;

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
        if (UIManager == null)
        {
            isCursorLocked = false;
            StopManager.TimePass();
        }
        if(Boss.GetComponent<DragonController>() != null)
        {
            Boss.GetComponent<DragonController>().enabled = false;
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C)){
            isCursorLocked = !isCursorLocked;
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
        else if (Boss.TryGetComponent<DragonController>(out dragon))
        {
        }

        if(progressManager.boss1Cleared && progressManager.boss2Cleared)
        {
            bgm = AudioManager.Bgm.Page2;
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
