using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionFade : UnityEngine.UI.Graphic
{
    [SerializeField]
    Texture _masktexture;
    [SerializeField, Range(0f, 1f)]
    float _cutOutRange;

    public float Range
    {
        get { return _cutOutRange; }
        set
        {
            _cutOutRange = value;
            UpdateCutOutMask(_cutOutRange);
        }
    }

    void UpdateCutOutMask(float range)
    {
        //if (range <= 0f)
        //{
        //    if (enabled)
        //    {
        //        material.SetFloat("_Range", 1 - range);
        //        enabled = false;
        //    }
        //    return;
        //}

        //if (!enabled) { enabled = true; }

        material.SetFloat("_Range", 1 - range);
    }

    public void UpdateMaskTexture(Texture texture)
    {
        material.SetTexture("_MaskTex", texture);
        material.SetColor("_Color", color);
    }

    IEnumerator StartFadeIn(float time, Action action)
    {
        Range = 1;
        float _time = 0;
        while (true)
        {
            yield return null;
            _time += Time.deltaTime;
            Range = 1.0f - _time / time;
            if (Range <= 0) { break; }
        }
        if (action != null) { action(); }
    }
    IEnumerator StartFadeOut(float time, Action action)
    {
        Range = 0;
        float _time = 0;
        while (true)
        {
            yield return null;
            _time += Time.deltaTime;
            Range = _time / time;
            if (Range >= 1) { break; }
        }
        if (action != null) { action(); }
    }
    public void FadeOut(float time, Action action)
    {
        StopAllCoroutines();
        StartCoroutine(StartFadeOut(time, action));
    }

    public void FadeOut(float time)
    {
        FadeOut(time, null);
    }

    public void FadeIn(float time, Action action)
    {
        StopAllCoroutines();
        StartCoroutine(StartFadeIn(time, action));
    }

    public void FadeIn(float time)
    {
        FadeIn(time, null);
    }

#if UNITY_EDITOR
    protected override void OnValidate()
    {
        base.OnValidate();
        UpdateCutOutMask(_cutOutRange);
        UpdateMaskTexture(_masktexture);
    }
#endif
}
