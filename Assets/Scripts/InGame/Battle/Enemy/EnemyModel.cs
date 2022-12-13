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
     ReactiveProperty<int> _enemyHpProperty;
    int _maxHP = 0;

    /// <summary>�G�̒ʏ펞�̍U����</summary>
    int _normalAttack;

    int _specialAttack;
    float _timer = 0f;
    float _attackInterval = 0f;

    public EnemyModel(int maxHp,int normalAttack,int specialAttack, System.Action<int>action, GameObject gameObject)
    {
        _maxHP = maxHp;
        _normalAttack = normalAttack;
        _specialAttack = specialAttack;
        _enemyHpProperty = new ReactiveProperty<int>(maxHp);     
        _enemyHpProperty.Subscribe(action).AddTo(gameObject);
    }

    public bool IsAttack(float deltaTime)
    {
        if (_timer<_attackInterval)
        {
            _timer += deltaTime;          
        }
        else
        {
            _timer = 0f;
        }
        return _timer < _attackInterval;
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
