using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerHPGauge))]
public class PlayerHP : MonoBehaviour,IDamage
{
    [SerializeField]
    int _initHP = 10;
    [SerializeField]
    int _maxHP = 10;
    PlayerHPGauge _hpGauge;
    PlayerHPModel _hpModel;

    public void Init()
    {
        _hpGauge = GetComponent<PlayerHPGauge>();
        _hpGauge.Init();
        _hpModel = new PlayerHPModel
            (value =>
            {
                _hpGauge.SetSliderValue(value, _maxHP);
            },
            this.gameObject, _initHP);
    }

    public void Damage(int value)
    {
        _hpModel.AddPlayerHP(-value);
    }
}
