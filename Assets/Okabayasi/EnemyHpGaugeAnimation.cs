using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class EnemyHpGaugeAnimation : MonoBehaviour
{
    [SerializeField]
    Slider slider = null;
    [SerializeField]
    Image sliderFill = null;
    [SerializeField]
    int HP = 0;

    private void Start()
    {
        StartSlider();
    }
    void UpdateUI(float maxHp) 
    {
        if (slider.value < 0.25f)
        {
            sliderFill.color = Color.red;
        }
        else if (slider.value < 0.5f)
        {
            sliderFill.color = Color.yellow;
        }
        else
        {
            sliderFill.color = Color.green;
        }
    }
    public void ChangeSliderValue(int maxHp, int currentHp)
    {
        slider.maxValue = maxHp;
        slider.value = currentHp;
    }
    public IEnumerator StartSlider()
    {
        float animHP = HP;
        yield return DOTween.To(() => animHP, (x) => animHP = x, slider.maxValue, 3f)
               .SetEase(Ease.Linear)
               .OnUpdate(()=>UpdateUI(animHP));
    }
}
