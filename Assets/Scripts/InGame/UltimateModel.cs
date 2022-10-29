using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class UltimateModel
{
    ReactiveProperty<int> _currentUltimateProperty = new ReactiveProperty<int>();
    int _maxUltimateValue = 0;

    /// <summary>
    /// 初期化をするためのコンストラクタ
    /// </summary>
    /// <param name="maxValue">最大値</param>
    /// <param name="action">現在のアルティメットの値のReactivePropertyに登録するAction(int)</param>
    /// <param name="addToObject">Addtoメソッドに使用するためのGameObject</param>
    public UltimateModel(int maxValue, System.Action<int> action, GameObject addToObject)
    {
        _maxUltimateValue = maxValue;
        _currentUltimateProperty.Subscribe(action).AddTo(addToObject);
        _currentUltimateProperty.Value = 0;
    }

    /// <summary>
    /// アルティメットの現在値を変更する関数
    /// </summary>
    /// <param name="diff">足される値</param>
    public void ChangeUltimateValue(int diff)
    {
        _currentUltimateProperty.Value = Mathf.Clamp(_currentUltimateProperty.Value+=diff,0,_maxUltimateValue);
    }
}
