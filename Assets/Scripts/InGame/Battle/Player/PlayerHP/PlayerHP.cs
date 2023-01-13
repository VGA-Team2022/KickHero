using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

[RequireComponent(typeof(PlayerHPGauge))]
public class PlayerHP : MonoBehaviour
{
    [SerializeField]
    int _initHP = 100;
    PlayerHPGauge _hpGauge;
    PlayerHPModel _hpModel;

    [SerializeField]
    Animator _animator;

    bool _isDead = false;
    public bool IsDead => _isDead;

    public void Init()
    {
        _hpGauge = GetComponent<PlayerHPGauge>();
        _hpGauge.Init();
        _hpModel = new PlayerHPModel
            (value =>
            {
                _hpGauge.SetSliderValue(value, _initHP);
                if (value <=0)
                {
                    _isDead = true;
                }
            },
            this.gameObject, _initHP);
        _hpGauge.SetSliderValue(_initHP, _initHP);
    }

    /// <summary>
    /// HP‚Ì’l‚ğ•ÏX‚·‚éŠÖ”
    /// </summary>
    /// <param name="value">‘«‚³‚ê‚é’l</param>
    public void AddHPValue(int value)
    {
        _animator.SetTrigger("Damage");
        _hpModel.AddPlayerHP(value);
    }
}
