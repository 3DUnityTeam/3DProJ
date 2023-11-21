using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("#BGM")]
    public AudioClip[] bgmClip;
    public float bgmVolume=0.5f;
    AudioSource bgmPlayer;

    [Header("#SFX")]
    public AudioClip[] sfxClip;
    public float sfxVolume = 0.5f;
    public int channels;
    AudioSource[] sfxPlayers;
    int channelIndex;


    public enum Bgm
    {
        TitleBGM,
        Page1,
        Page2,
    }
    public enum Sfx
    {
        //타이틀 화면용

        Opening,
        Breath1,
        Breath2,
        Wind,


        //실제 전투용

        Weapon1,
        Weapon1effect,
        Weapon2,
        Weapon2effect,
        Weapon3,
        Weapon3effect,
        Sweapon1,
        Sweapon1effect,
        Sweapon2,
        Sweapon2effect,
        Boost,
        BoostLoop,
        StateSound

    }
    public void Init()
    {
        //볼륨 초기화
        if (GameManager.instance.Load("BGM") == -1)
        {
            bgmVolume = 0.5f;
        }
        else
        {
            bgmVolume = GameManager.instance.Load("BGM");
        }
        if (GameManager.instance.Load("SFX") == -1)
        {
            sfxVolume = 0.5f;
        }
        else
        {
            sfxVolume = GameManager.instance.Load("SFX");
        }

        //배경음 플레이어 초기화
        GameObject bgmObject = new GameObject("BgmPlayer");//지정하려면 괄호안에
        bgmObject.transform.parent = transform;
        bgmPlayer = bgmObject.AddComponent<AudioSource>(); //컴포넌트 지정
        bgmPlayer.playOnAwake = false; //바로 작동되는거 차단.한번만 출력X
        bgmPlayer.loop = true; //반복
        bgmPlayer.volume = bgmVolume;


        //효과음 플레이어 초기화
        GameObject sfxObject = new GameObject("SfxPlayer");
        sfxObject.transform.parent = transform;
        sfxPlayers = new AudioSource[channels];

        for (int index = 0; index < sfxPlayers.Length; index++)
        {
            sfxPlayers[index] = sfxObject.AddComponent<AudioSource>();
            sfxPlayers[index].playOnAwake = false; //바로 작동되는거 차단.한번만 출력X
            sfxPlayers[index].volume = sfxVolume;
        }
    }

    public void PlaySfx(Sfx sfx)
    {
        for (int index = 0; index < sfxPlayers.Length; index++)
        {
            int loopIndex = (index + channelIndex) % sfxPlayers.Length;

            if (sfxPlayers[loopIndex].isPlaying)
            {
                continue;
            }
            channelIndex = loopIndex;
            sfxPlayers[loopIndex].clip = sfxClip[(int)sfx];
            sfxPlayers[loopIndex].Play();
            break;
        }
    }

    public int PlaySfxLoop(Sfx sfx)
    {
        for (int index = 0; index < sfxPlayers.Length; index++)
        {
            int loopIndex = (index + channelIndex) % sfxPlayers.Length;

            if (sfxPlayers[loopIndex].isPlaying)
            {
                continue;
            }
            channelIndex = loopIndex;
            sfxPlayers[loopIndex].clip = sfxClip[(int)sfx];
            sfxPlayers[loopIndex].Play();
            sfxPlayers[loopIndex].loop = true;
            return loopIndex;
        }
        return -1;
    }

    public void PlayBgm(Bgm bgm)
    {
        bgmPlayer.clip = bgmClip[(int)bgm];
        bgmPlayer.Play();
    }

    public void ChangeVolume()
    {
        bgmPlayer.volume = bgmVolume;
        for (int index = 0; index < sfxPlayers.Length; index++)
        {
            int loopIndex = (index + channelIndex) % sfxPlayers.Length;
            sfxPlayers[loopIndex].volume = sfxVolume;
        }
    }

    public void StopBgm()
    {
        bgmPlayer.Stop();
    }

    //바뀐 BGM 사운드 저장
    public void SaveBGMSound(float per)
    {
        float volume = per;
        bgmVolume = volume;
        GameManager.instance.Save("BGM", volume);
        ChangeVolume();
    }
    //바뀐 SFX 사운드 저장
    public void SaveSFXSound(float per)
    {
        float volume = per;
        sfxVolume = volume;
        GameManager.instance.Save("SFX", volume);
        ChangeVolume();
    }
}
