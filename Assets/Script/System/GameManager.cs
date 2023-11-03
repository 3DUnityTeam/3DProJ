using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("#Player")]
    public Player player;


    [Header("#Manager")]
    public UIManager UIManager;
    public AudioManager AudioManager;
    public WeaponManager WeaponManager;

    [HideInInspector] public float sfxPer;
    [HideInInspector] public float bgmPer;

    private void Awake()
    {
        instance = this;
        AudioManager.Init();
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
