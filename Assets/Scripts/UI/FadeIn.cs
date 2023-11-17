using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeIn : MonoBehaviour
{
    public enum State
    {
        fadein,fadeout
    }
    public Image myImage;
    public float alphaSpeed = 0.5f;


    public State state = State.fadein;
    // Start is called before the first frame update
    void Start()
    {
        myImage = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        float currentAlpha;
        switch (state)
        {
            case State.fadein:
                currentAlpha = myImage.color.a;

                // Time.deltaTime�� ����Ͽ� ���� �� ����
                currentAlpha -= alphaSpeed * Time.deltaTime;

                // ���� ���� 0���� 1 ���̷� ����
                currentAlpha = Mathf.Clamp01(currentAlpha);

                // ����� ���� ���� �̹����� ����
                myImage.color = new Color(myImage.color.r, myImage.color.g, myImage.color.b, currentAlpha);
                break;
            case State.fadeout:
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
}
