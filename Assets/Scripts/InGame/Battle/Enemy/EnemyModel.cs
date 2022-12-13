using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

/// <summary>
/// 敵のデータに関してのスクリプト
/// </summary>
public class EnemyModel 
{
    /// <summary>敵キャラクターのHPの変化を管理する変数</summary>
     ReactiveProperty<int> _enemyHpProperty;
    int _maxHP = 0;

    /// <summary>敵の通常時の攻撃力</summary>
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
    /// ダメージ計算用の関数
    /// </summary>
    /// <param name="damage">ダメージ</param>
    public void Damage(int damage)　//これはボール側が呼ぶ
    {
        _enemyHpProperty.Value -= damage;
    }
}
