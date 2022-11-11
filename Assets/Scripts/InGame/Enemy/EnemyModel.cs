using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

/// <summary>
/// �G�̃f�[�^�Ɋւ��ẴX�N���v�g
/// </summary>
public class EnemyModel 
{
    /// <summary>�G�L�����N�^�[��HP�̕ω����Ǘ�����ϐ�</summary>
     ReactiveProperty<int> _enemyHpProperty = new ReactiveProperty<int>();
    int _maxHP = 0;

    /// <summary>�G�̒ʏ펞�̍U����</summary>
    [SerializeField] public int _normalEnemyPower;

    /// <summary></summary>
    [SerializeField] public int _specialEnemyPower;
    float _attackTime = 0;

    public EnemyModel(int maxHp, System.Action<int>action, GameObject gameObject)
    {
        _maxHP = maxHp;
        _enemyHpProperty.Subscribe(action).AddTo(gameObject);
        _enemyHpProperty.Value = _maxHP;
    }

    /// <summary>
    /// �_���[�W�v�Z�p�̊֐�
    /// </summary>
    /// <param name="damage">�_���[�W</param>
    public void Damage(int damage)�@//����̓{�[�������Ă�
    {
        _enemyHpProperty.Value -= damage;
    }
}
