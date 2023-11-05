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
    public Player player;


    [Header("#Manager")]
    //UI 매니저 
    public UIManager UIManager;
    //오디오 매니저
    public AudioManager AudioManager;
    //무기 매니저
    public WeaponManager WeaponManager;

    [Header("#WeaponImage")]
    //무기 이미지
    public Sprite[] WeaponImages;
    public Sprite BaseImage;

    private void Awake()
    {
        instance = this;
        Init();
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
        AudioManager.PlayBgm(AudioManager.Bgm.Title);
        AudioManager.PlaySfx(AudioManager.Sfx.Dead);
    }

    private void Update()
    {

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
