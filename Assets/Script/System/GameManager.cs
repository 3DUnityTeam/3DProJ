using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [Header("#Manager")]
    public UIManager UIManager;
    public AudioManager AudioManager;

    [HideInInspector] public float sfxPer;
    [HideInInspector] public float bgmPer;

    private void Awake()
    {
        instance = this;
        if (Load("BGM") == -1)
        {
            AudioManager.bgmVolume = 0.5f;
        }
        else
        {
            AudioManager.bgmVolume = Load("BGM");
        }
        if (Load("SFX") == -1)
        {
            AudioManager.sfxVolume = 0.5f;
        }
        else
        {
            AudioManager.sfxVolume = Load("SFX");
        }
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

    public void SaveBGMSound(float per)
    {
        bgmPer = per;
        float volume = bgmPer;
        AudioManager.bgmVolume = volume;
        Save("BGM", volume);
        AudioManager.ChangeVolume();
    }
    public void SaveSFXSound(float per)
    {
        sfxPer = per;
        float volume = sfxPer;
        AudioManager.sfxVolume = volume;
        Save("SFX", volume);
        AudioManager.ChangeVolume();
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
