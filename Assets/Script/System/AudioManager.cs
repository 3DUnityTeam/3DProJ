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
        Title,
        Page1,
        Page2,
    }
    public enum Sfx
    {
        Dead,
        Hit,
        Lose,
        Win,
        Select,
    }
    public void Init()
    {
        //����� �÷��̾� �ʱ�ȭ
        GameObject bgmObject = new GameObject("BgmPlayer");//�����Ϸ��� ��ȣ�ȿ�
        bgmObject.transform.parent = transform;
        bgmPlayer = bgmObject.AddComponent<AudioSource>(); //������Ʈ ����
        bgmPlayer.playOnAwake = false; //�ٷ� �۵��Ǵ°� ����.�ѹ��� ���X
        bgmPlayer.loop = true; //�ݺ�
        bgmPlayer.volume = bgmVolume;


        //ȿ���� �÷��̾� �ʱ�ȭ
        GameObject sfxObject = new GameObject("SfxPlayer");
        sfxObject.transform.parent = transform;
        sfxPlayers = new AudioSource[channels];

        for (int index = 0; index < sfxPlayers.Length; index++)
        {
            sfxPlayers[index] = sfxObject.AddComponent<AudioSource>();
            sfxPlayers[index].playOnAwake = false; //�ٷ� �۵��Ǵ°� ����.�ѹ��� ���X
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
}
