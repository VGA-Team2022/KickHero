using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;


public class EnemyHpGaugeAnimation : MonoBehaviour
{
    public void SetSliderValue(Slider slider,float endValue,float duration)
    {
        slider.DOValue(endValue, duration)
                     .SetEase(Ease.InOutQuad)
                     .WaitForCompletion();
    }
}
