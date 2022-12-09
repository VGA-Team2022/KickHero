using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;


public class EnemyHpGaugeAnimation : MonoBehaviour
{
    [SerializeField] private Slider slider = null;
    [SerializeField] private float value = 0f;
    [SerializeField] private float duration = 0f;
    [SerializeField] private GameObject obj = null;
    void Start()
    {
        DOTween.To(() => slider.value, (x) => slider.value = x, value, duration)
            .SetEase(Ease.InOutCubic)
            //.SetLink()
            .OnComplete(end);
    }
    void end()
    {
       obj.SetActive(false);
    }
}
