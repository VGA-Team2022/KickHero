using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UltimateView : MonoBehaviour
{
    [SerializeField]
    Slider _ulitimateGauge = null;
    [SerializeField]
    Image _ultimateLamp;

    public void Init(int maxValue)
    {
        //直接触れないようにする
        _ulitimateGauge.interactable = false;
    }

    /// <summary>
    /// ゲージのスライダーの値を変更する関数
    /// </summary>
    /// <param name="maxValue">最大値</param>
    /// <param name="currentValue">現在の値</param>
    public void ChangeGaugeValue(int maxValue,int currentValue)
    {
        _ulitimateGauge.maxValue = maxValue;
        _ulitimateGauge.value = Mathf.Clamp(currentValue,0,maxValue);
        if (currentValue>=maxValue)
        {
            _ultimateLamp.color = Color.red;
        }
        else
        {
            _ultimateLamp.color = Color.gray;
        }
    }
}
