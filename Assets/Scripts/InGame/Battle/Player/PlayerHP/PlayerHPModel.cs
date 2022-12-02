using UnityEngine;
using UniRx;

public class PlayerHPModel
{
    ReactiveProperty<int> _playerHPProperty;

    public PlayerHPModel(System.Action<int> action,GameObject addtoObject,int initValue)
    {
        _playerHPProperty = new ReactiveProperty<int>(initValue);
        _playerHPProperty.Subscribe(action).AddTo(addtoObject);
    }

    public void AddPlayerHP(int value)
    {
        _playerHPProperty.Value += value;
    }
}
