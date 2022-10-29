using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UltimatePresenter : MonoBehaviour
{
    [SerializeField]
    UltimateView _ultimateView = null;

    UltimateModel _ultimateModel = null;
    [SerializeField]
    int _maxUltimateValue = 10;
    void Start()
    {
        Init();
    }

    public void Init()
    {
        _ultimateModel = new UltimateModel(
            _maxUltimateValue,
            x =>
            {
                _ultimateView.ChangeGaugeValue(_maxUltimateValue,x);
            },
            _ultimateView.gameObject);
    }

    public void ChangeValue(int value)
    {
        _ultimateModel.ChangeUltimateValue(value);
    }
}
