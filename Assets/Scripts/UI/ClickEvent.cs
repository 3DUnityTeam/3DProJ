using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClickEvent : MonoBehaviour
{
    //���̽��Ǵ� UI
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
        GameManager.instance.AudioManager.PlayBgm(AudioManager.Bgm.TitleBGM);
        yield return new WaitForSeconds(3);
        ButtonBox.SetActive(true);
        keys.SetActive(true);
    }
    public void GoToBattle()
    {
        GameManager.instance.AudioManager.StopBgm();
        keys.SetActive(false);
        ButtonBox.SetActive(false);
        StartCoroutine(Battlemovie());
    }
    //Ÿ��Ʋ ������ �̵�
    public void GoToTitle()
    {
        SceneManager.LoadScene("Title");
    }
    //����â ����(������ â ��Ȱ��ȭ)
    public void OpenOption()
    {
        OptionUI.SetActive(true);
        BaseUI.SetActive(false);
    }
    //����â �ݱ�(�ɼ��� â ��Ȱ��ȭ)
    public void BackOption()
    {
        BaseUI.SetActive(true);
        OptionUI.SetActive(false);
    }
    //���� ���� ��ư
    public void ExitGame()
    {
        Debug.Log("test");
        Application.Quit();
    }

    //���� ����
    public void StartGame()
    {
        GameManager.instance.StopManager.TimePass();
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
        SceneManager.LoadScene(1);
    }
}
