using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

/// <summary>
/// 敵のデータと表示を使うためのスクリプト
/// </summary>
public class EnemyHPPresenter : MonoBehaviour, IDamage
{
    /// <summary>敵のデータに関してのクラス</summary>
    EnemyHPModel _enemyModel = null;

    /// <summary>敵の表示に関してのクラス</summary>
    [SerializeField]
    EnemyHPView _enemyView = null;

    /// <summary>敵の最大HP</summary>
    [SerializeField] int _enemyHp = 20;

    bool _isDead = false;
    public bool IsDead => _isDead;
    /// <summary>
    /// HPスライダーの変更
    /// </summary>
    public void Init()
    {
        _enemyModel = new EnemyHPModel(
            _enemyHp,
            x =>
            {
                _enemyView.ChangeSliderValue(_enemyHp, x);
                if (x <= 0)
                {
                    _isDead = true;
                }
            },
            _enemyView.gameObject);
    }

    /// <summary>
    /// 外部から値を参照するための関数
    /// </summary>
    /// <param name="value">与えるダメージの値</param>
    public void Damage(int value)
    {
        _enemyModel.Damage(value);
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.TryGetComponent(out BallPresenter ballPresenter))
        {
            ballPresenter.Collection();
            Damage(1);
        }
    }
}

