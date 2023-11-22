using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClickEvent : MonoBehaviour
{
    //���̽��Ǵ� UI
    public Player player;
    public GameObject ButtonBox;
    public GameObject keys;
    public GameObject BaseUI;
    //�ɼ�â
    public GameObject OptionUI;
    //Battle ������ �̵�
    public GameObject effect1;
    public GameObject effect2;
    public FadeIn fadeinbox;

    public IEnumerator Start()
    {
        if(player != null)
        {
            player.enabled = false;
            GameManager.instance.AudioManager.PlayBgm(AudioManager.Bgm.TitleBGM);
            yield return new WaitForSeconds(3);
            player.enabled = true;
            ButtonBox.SetActive(true);
            keys.SetActive(true);
        }
        
    }
    public void GoToBattle()
    {
        GameManager.instance.AudioManager.PlaySfx(AudioManager.Sfx.Boost);
        GameManager.instance.AudioManager.StopBgm();
        keys.SetActive(false);
        ButtonBox.SetActive(false);
        StartCoroutine(Battlemovie());
    }
    //Ÿ��Ʋ ������ �̵�
    public void GoToTitle()
    {
        GameManager.instance.AudioManager.PlaySfx(AudioManager.Sfx.Boost);
        SceneManager.LoadScene("Title");
    }
    //����â ����(������ â ��Ȱ��ȭ)
    public void OpenOption()
    {
        GameManager.instance.AudioManager.PlaySfx(AudioManager.Sfx.Boost);
        OptionUI.SetActive(true);
        if (BaseUI != null)
        {
            BaseUI.SetActive(false);
        }
    }
    //����â �ݱ�(�ɼ��� â ��Ȱ��ȭ)
    public void BackOption()
    {
        GameManager.instance.AudioManager.PlaySfx(AudioManager.Sfx.Boost);
        if (BaseUI != null)
        {
            BaseUI.SetActive(true);
        }
        OptionUI.SetActive(false);
    }
    //���� ���� ��ư
    public void ExitGame()
    {
        GameManager.instance.AudioManager.PlaySfx(AudioManager.Sfx.Boost);
        Debug.Log("test");
        Application.Quit();
    }

    //���� ����
    public void StartGame()
    {
        GameManager.instance.AudioManager.PlaySfx(AudioManager.Sfx.Boost);
        GameManager.instance.StopManager.TimePass();
        if (gameObject != GameManager.instance.UIManager.PauseUI) { 
            GameManager.instance.AudioManager.PlayBgm(GameManager.instance.bgm);
        }
        gameObject.SetActive(false);
    }

    public IEnumerator Battlemovie()
    {
        GameManager.instance.AudioManager.PlaySfxLoop(AudioManager.Sfx.BoostLoop);
        GameManager.instance.AudioManager.PlaySfx(AudioManager.Sfx.Opening);
        effect1.SetActive(true);
        yield return new WaitForSeconds(1f);
        GameManager.instance.AudioManager.PlaySfx(AudioManager.Sfx.Breath1);
        GameManager.instance.AudioManager.PlaySfx(AudioManager.Sfx.Breath1);
        GameManager.instance.AudioManager.PlaySfx(AudioManager.Sfx.Breath1);
        effect2.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        fadeinbox.state = FadeIn.State.fadeout;
        yield return new WaitForSeconds(4f);
        SceneManager.LoadScene(2);
    }
}
