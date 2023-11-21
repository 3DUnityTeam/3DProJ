using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClickEvent : MonoBehaviour
{
    //���̽��Ǵ� UI
    public GameObject BaseUI;
    //�ɼ�â
    public GameObject OptionUI;
    //Battle ������ �̵�
    public GameObject effect1;
    public GameObject effect2;
    public FadeIn fadeinbox;
    public void GoToBattle()
    {
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
        effect1.SetActive(true);
        yield return new WaitForSeconds(1f);
        effect2.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        fadeinbox.state = FadeIn.State.fadeout;
        yield return new WaitForSeconds(4f);
        SceneManager.LoadScene(1);
    }
}
