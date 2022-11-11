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
     ReactiveProperty<int> _enemyHpProperty = new ReactiveProperty<int>();
    int _maxHP = 0;

    /// <summary>敵の通常時の攻撃力</summary>
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
    /// ダメージ計算用の関数
    /// </summary>
    /// <param name="damage">ダメージ</param>
    public void Damage(int damage)　//これはボール側が呼ぶ
    {
        _enemyHpProperty.Value -= damage;
    }
}
