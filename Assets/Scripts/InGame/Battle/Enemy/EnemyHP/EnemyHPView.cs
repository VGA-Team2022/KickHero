using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

/// <summary>
/// 敵の表示に関してのスクリプト
/// </summary>
public class EnemyHPView : MonoBehaviour
{
    [SerializeField]
    Slider _slider = null;
    [SerializeField]
    float _sliderDuration = 1f;
    /// <summary>
    /// HPスライダーの表示変更用関数
    /// </summary>
    /// <param name="maxHp">最大HP</param>
    /// <param name="currentHp">変更後のHP</param>
    public void ChangeSliderValue(int maxHp, int currentHp)
    {
        _slider.maxValue = maxHp;
        UIAnimationUtil.GaugeAnimation(_slider,_slider.value,currentHp,_sliderDuration);
    }
}
