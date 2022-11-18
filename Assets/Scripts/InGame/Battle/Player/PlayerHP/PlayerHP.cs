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

    /// <summary>
    /// HP‚Ì’l‚ğ•ÏX‚·‚éŠÖ”
    /// </summary>
    /// <param name="value">‘«‚³‚ê‚é’l</param>
    public void AddHPValue(int value)
    {
        _hpModel.AddPlayerHP(value);
    }
}
