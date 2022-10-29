using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class UltimateModel
{
    ReactiveProperty<int> _currentUltimateProperty = new ReactiveProperty<int>();
    int _maxUltimateValue = 0;

    public UltimateModel(int maxValue, System.Action<int> action, GameObject addToObject)
    {
        _maxUltimateValue = maxValue;
        _currentUltimateProperty.Subscribe(action).AddTo(addToObject);
        _currentUltimateProperty.Value = 0;
    }

    public void ChangeUltimateValue(int diff)
    {
        _currentUltimateProperty.Value = Mathf.Clamp(_currentUltimateProperty.Value+=diff,0,_maxUltimateValue);
    }
}
