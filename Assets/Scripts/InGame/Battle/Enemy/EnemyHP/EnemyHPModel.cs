using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

/// <summary>
/// 敵のデータに関してのスクリプト
/// </summary>
public class EnemyHPModel
{
    /// <summary>敵キャラクターのHPの変化を管理する変数</summary>
    ReactiveProperty<int> _enemyHpProperty;
    int _maxHP = 0;

    public EnemyHPModel(int maxHp, System.Action<int> action, GameObject gameObject)
    {
        _maxHP = maxHp;
        _enemyHpProperty = new ReactiveProperty<int>(maxHp);
        _enemyHpProperty.Subscribe(action).AddTo(gameObject);
    }

    /// <summary>
    /// ダメージ計算用の関数
    /// </summary>
    /// <param name="damage">ダメージ</param>
    public void Damage(int damage)　//これはボール側が呼ぶ
    {
        int value = Mathf.Clamp(_enemyHpProperty.Value - damage, 0, _maxHP);
        _enemyHpProperty.Value = value;
    }
}
