using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UltimatePresenter : MonoBehaviour
{
    [SerializeField,Tooltip("ビュークラスのインスタンス")]
    UltimateView _ultimateView = null;

    /// <summary>モデルのインスタンス</summary>
    UltimateModel _ultimateModel = null;
    [SerializeField,Tooltip("アルティメットの最大値")]
    int _maxUltimateValue = 10;
    void Start()
    {
        Init();
    }

    public void Init()
    {
        //インスタンス生成、引数は(最大値,Action<int>,GameObject)
        _ultimateModel = new UltimateModel(
            _maxUltimateValue,
            x =>
            {
                _ultimateView.ChangeGaugeValue(_maxUltimateValue,x);
            },
            _ultimateView.gameObject);
    }

    /// <summary>
    /// 値を外部から変更する関数
    /// </summary>
    public void ChangeValue(int value)
    {
        _ultimateModel.ChangeUltimateValue(value);
    }
}
