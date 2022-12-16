using DG.Tweening;
using UnityEngine.UI;

public class UIAnimationUtil
{
    public static void GaugeAnimation(Slider slider,float currentValue, float endValue, float duration)
    {
        slider.value = currentValue;
        slider.DOValue(endValue, duration)
                     .SetEase(Ease.InOutQuad)
                     .WaitForCompletion();
    }
}
