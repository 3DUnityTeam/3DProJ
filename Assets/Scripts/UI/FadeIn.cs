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

                // Time.deltaTime에 비례하여 알파 값 증가
                currentAlpha -= alphaSpeed * Time.deltaTime;

                // 알파 값은 0에서 1 사이로 유지
                currentAlpha = Mathf.Clamp01(currentAlpha);

                // 변경된 알파 값을 이미지에 적용
                myImage.color = new Color(myImage.color.r, myImage.color.g, myImage.color.b, currentAlpha);
                break;
            case State.fadeout:
                currentAlpha = myImage.color.a;

                // Time.deltaTime에 비례하여 알파 값 증가
                currentAlpha += alphaSpeed * Time.deltaTime;

                // 알파 값은 0에서 1 사이로 유지
                currentAlpha = Mathf.Clamp01(currentAlpha);

                // 변경된 알파 값을 이미지에 적용
                myImage.color = new Color(myImage.color.r, myImage.color.g, myImage.color.b, currentAlpha);
                break;
        }
    }
}
