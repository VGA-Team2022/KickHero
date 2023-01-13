using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UniRx;

public class UltimateView : MonoBehaviour
{
    [SerializeField]
    Slider _ulitimateGauge = null;
    [SerializeField]
    Button _ultimateLamp;

    public void Init(Action onButtonClick)
    {
        //���ڐG��Ȃ��悤�ɂ���
        _ulitimateGauge.interactable = false;

        _ulitimateGauge.interactable = false;
        _ultimateLamp.onClick.AddListener(() => { onButtonClick.Invoke(); });
    }

    /// <summary>
    /// �Q�[�W�̃X���C�_�[�̒l��ύX����֐�
    /// </summary>
    /// <param name="maxValue">�ő�l</param>
    /// <param name="currentValue">���݂̒l</param>
    public void ChangeGaugeValue(int maxValue, int currentValue)
    {
        _ulitimateGauge.maxValue = maxValue;
        _ulitimateGauge.value = Mathf.Clamp(currentValue, 0, maxValue);
        if (currentValue >= maxValue)
        {
            _ulitimateGauge.interactable = true;
            SoundManagerPresenter.Instance.CriAtomVoicePlay("Voice_Charge");
            _ultimateLamp.image.color = Color.white;
        }
        else
        {
            _ultimateLamp.image.color = Color.gray;
        }
    }
}
