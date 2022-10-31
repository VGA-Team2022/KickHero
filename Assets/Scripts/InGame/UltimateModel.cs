using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class UltimateModel
{
    ReactiveProperty<int> _currentUltimateProperty = new ReactiveProperty<int>();
    int _maxUltimateValue = 0;

    /// <summary>
    /// �����������邽�߂̃R���X�g���N�^
    /// </summary>
    /// <param name="maxValue">�ő�l</param>
    /// <param name="action">���݂̃A���e�B���b�g�̒l��ReactiveProperty�ɓo�^����Action(int)</param>
    /// <param name="addToObject">Addto���\�b�h�Ɏg�p���邽�߂�GameObject</param>
    public UltimateModel(int maxValue, System.Action<int> action, GameObject addToObject)
    {
        _maxUltimateValue = maxValue;
        _currentUltimateProperty.Subscribe(action).AddTo(addToObject);
        _currentUltimateProperty.Value = 0;
    }

    /// <summary>
    /// �A���e�B���b�g�̌��ݒl��ύX����֐�
    /// </summary>
    /// <param name="diff">�������l</param>
    public void ChangeUltimateValue(int diff)
    {
        _currentUltimateProperty.Value = Mathf.Clamp(_currentUltimateProperty.Value+=diff,0,_maxUltimateValue);
    }
}
