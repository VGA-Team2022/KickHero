using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerHPGauge))]
public class PlayerHP : MonoBehaviour
{
    [SerializeField]
    int _initHP = 10;
    [SerializeField]
    int _maxHP = 10;
    PlayerHPGauge _hpGauge;
    PlayerHPModel _hpModel;

    private void Start()
    {
        _hpGauge = GetComponent<PlayerHPGauge>();
        _hpModel = new PlayerHPModel
            (value =>
            {
                _hpGauge.SetSliderValue(value, _maxHP);
            },
            this.gameObject, _initHP);
    }

    /// <summary>
    /// HP�̒l��ύX����֐�
    /// </summary>
    /// <param name="value">�������l</param>
    public void AddHPValue(int value)
    {
        int clampHP = Mathf.Clamp(value,0,_maxHP);
        _hpModel.AddPlayerHP(clampHP);
    }
}
