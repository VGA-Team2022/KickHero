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
        //���ڐG��Ȃ��悤�ɂ���
        _ulitimateGauge.interactable = false;
    }

    /// <summary>
    /// �Q�[�W�̃X���C�_�[�̒l��ύX����֐�
    /// </summary>
    /// <param name="maxValue">�ő�l</param>
    /// <param name="currentValue">���݂̒l</param>
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
