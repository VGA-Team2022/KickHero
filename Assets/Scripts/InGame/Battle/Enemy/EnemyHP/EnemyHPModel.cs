using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

/// <summary>
/// �G�̃f�[�^�Ɋւ��ẴX�N���v�g
/// </summary>
public class EnemyHPModel
{
    /// <summary>�G�L�����N�^�[��HP�̕ω����Ǘ�����ϐ�</summary>
    ReactiveProperty<int> _enemyHpProperty;
    int _maxHP = 0;

    public EnemyHPModel(int maxHp, System.Action<int> action, GameObject gameObject)
    {
        _maxHP = maxHp;
        _enemyHpProperty = new ReactiveProperty<int>(maxHp);
        _enemyHpProperty.Subscribe(action).AddTo(gameObject);
    }

    /// <summary>
    /// �_���[�W�v�Z�p�̊֐�
    /// </summary>
    /// <param name="damage">�_���[�W</param>
    public void Damage(int damage)�@//����̓{�[�������Ă�
    {
        int value = Mathf.Clamp(_enemyHpProperty.Value - damage, 0, _maxHP);
        _enemyHpProperty.Value = value;
    }
}
