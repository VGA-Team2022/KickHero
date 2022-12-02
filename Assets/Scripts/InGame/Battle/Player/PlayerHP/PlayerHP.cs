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

    public void Init(System.Action<InGameCycle.EventEnum> changeStateAction)
    {
        _hpGauge = GetComponent<PlayerHPGauge>();
        _hpGauge.Init();
        _hpModel = new PlayerHPModel
            (value =>
            {
                _hpGauge.SetSliderValue(value, _maxHP);
                if (value <=0)
                {
                    changeStateAction.Invoke(InGameCycle.EventEnum.GameOver);
                }            
            },
            this.gameObject, _initHP);
    }

    /// <summary>
    /// HPの値を変更する関数
    /// </summary>
    /// <param name="value">足される値</param>
    public void AddHPValue(int value)
    {
        _hpModel.AddPlayerHP(value);
    }
}
