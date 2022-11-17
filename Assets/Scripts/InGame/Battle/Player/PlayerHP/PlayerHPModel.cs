using UnityEngine;
using UniRx;

public class PlayerHPModel
{
    ReactiveProperty<int> _playerHPProperty = new ReactiveProperty<int>();

    public PlayerHPModel(System.Action<int> action,GameObject addtoObject,int initValue)
    {
        _playerHPProperty.Subscribe(action).AddTo(addtoObject);

        AddPlayerHP(initValue);
    }

    public void AddPlayerHP(int value)
    {
        _playerHPProperty.Value += value;
    }
}
