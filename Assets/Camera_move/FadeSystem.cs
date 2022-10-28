using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeSystem : MonoBehaviour
{
    float _time;
    float red, green, blue, alfa;
    [SerializeField] Image fadeImage;


    private void Start()
    {
        red = fadeImage.color.r;
        green = fadeImage.color.g;
        blue = fadeImage.color.b;
        alfa = fadeImage.color.a;
    }

    public IEnumerator StartFadeIn(float time, Action action)
    {
        fadeImage.gameObject.SetActive(true);
        alfa = 1;
        while (true)
        {
            yield return null;
            _time += Time.deltaTime;
            alfa = 1.0f - _time / time;
            fadeImage.color = new Color(red, green, blue, alfa);
            if (alfa <= 0) { break; }
        }
        _time = 0;
        if (action != null) { action(); }
        fadeImage.gameObject.SetActive(false);
    }
    public IEnumerator StartFadeOut(float time, Action action)
    {
        fadeImage.gameObject.SetActive(true);
        alfa = 0;
        while (true)
        {
            yield return null;
            _time += Time.deltaTime;
            alfa = _time / time;
            fadeImage.color = new Color(red, green, blue, alfa);
            if(alfa >= 1) { break; }
        }
        _time = 0;
        if (action != null) { action(); }
        fadeImage.gameObject.SetActive(false);
    }
}
