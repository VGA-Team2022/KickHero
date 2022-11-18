using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class PlayerHPGauge : MonoBehaviour
{
    Slider _hpSlider;

    public void Init()
    {
        _hpSlider = GetComponent<Slider>();
    }

    public void SetSliderValue(int value,int maxValue)
    {
        _hpSlider.value = value;
        _hpSlider.maxValue = maxValue;
    }
}
