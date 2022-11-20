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
        if (range <= 0f)
        {
            if (enabled)
            {
                material.SetFloat("_Range", 1 - range);
                enabled = false;
            }
            return;
        }

        if (!enabled) { enabled = true; }

        material.SetFloat("_Range", 1 - range);
    }

    public void UpdateMaskTexture(Texture texture)
    {
        material.SetTexture("_MaskTex", texture);
        material.SetColor("_Color", color);
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
