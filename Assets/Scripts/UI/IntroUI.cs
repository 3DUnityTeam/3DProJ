using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class IntroUI : MonoBehaviour
{
    public enum State
    {
        fadein, fadeout
    }
    public Image myImage;
    public float alphaSpeed = 0.5f;
    public Sprite[] stories;

    public State state = State.fadein;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(StartStory());
    }

    // Update is called once per frame
    void Update()
    {
        float currentAlpha;
        switch (state)
        {
            case State.fadeout:
                currentAlpha = myImage.color.a;

                // Time.deltaTime�� ����Ͽ� ���� �� ����
                currentAlpha -= alphaSpeed * Time.deltaTime;

                // ���� ���� 0���� 1 ���̷� ����
                currentAlpha = Mathf.Clamp01(currentAlpha);

                // ����� ���� ���� �̹����� ����
                myImage.color = new Color(myImage.color.r, myImage.color.g, myImage.color.b, currentAlpha);
                break;
            case State.fadein:
                currentAlpha = myImage.color.a;

                // Time.deltaTime�� ����Ͽ� ���� �� ����
                currentAlpha += alphaSpeed * Time.deltaTime;

                // ���� ���� 0���� 1 ���̷� ����
                currentAlpha = Mathf.Clamp01(currentAlpha);

                // ����� ���� ���� �̹����� ����
                myImage.color = new Color(myImage.color.r, myImage.color.g, myImage.color.b, currentAlpha);
                break;
        }

        
        
        
    }

    public void skip()
    {
        SceneManager.LoadScene("Title");
    }

    public IEnumerator StartStory()
    {
        int a = stories.Length;
        for (int i = 0; i < a; i++)
        {
            myImage.sprite = stories[i];
            state = State.fadein;

            yield return new WaitForSeconds(6f);

            state = State.fadeout;

            yield return new WaitForSeconds(4f);

            myImage.sprite = stories[i];

            if (i == a - 1)
            {
                SceneManager.LoadScene("Title");
            }
        }
    }
}
