using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

/// <summary>
/// 敵のデータと表示を使うためのスクリプト
/// </summary>
public class EnemyPresenter : MonoBehaviour,IAttack,IDamage
{
    /// <summary>敵のデータに関してのクラス</summary>
    EnemyModel _enemyModel = null;

    /// <summary>敵の表示に関してのクラス</summary>
    [SerializeField]
    EnemyView _enemyView = null;

    /// <summary>敵の最大HP</summary>
    [SerializeField] int _enemyHp = 20;

    /// <summary>通常攻撃かどうか判定するフラグ</summary>
    bool _normalAttack = true;

    /// <summary>シーケンスに返すイベント</summary>
    ReactiveProperty<InGameCycle.EventEnum>  _eventEnumProperty;

    /// <summary>
    /// HPスライダーの変更
    /// </summary>
    public void Init(System.Action<InGameCycle.EventEnum> changeStateAction)
    {
        _eventEnumProperty = new ReactiveProperty<InGameCycle.EventEnum>(InGameCycle.EventEnum.None);
        _eventEnumProperty.Subscribe(changeStateAction).AddTo(this.gameObject);

        _enemyModel = new EnemyModel(
            _enemyHp,
            x =>
            {
                _enemyView.ChangeSliderValue(_enemyHp, x);

                if(x <= 0)
                {
                    _eventEnumProperty.Value = InGameCycle.EventEnum.GameOver;
                    _enemyView.DeathMove();
                }
            },
            _enemyView.gameObject);
    }


    /// <summary>
    ///     攻撃するときによばれる関数
    /// </summary>
    public void Attack()
    {
        if(_normalAttack == true)
        {
            //プレイヤーのダメージ関数を呼ぶ
            //Damage(_enemyModel._enemyPower);
            _enemyView.NormalAttackMove();
        }
        else
        {
            //プレイヤーのダメージ関数を呼ぶ
            //Damage(_enemyModel._specialEnemyPower);
            _enemyView.SpecialAttackMove();
        }

    }

    /// <summary>
    /// 外部から値を参照するための関数
    /// </summary>
    /// <param name="value">与えるダメージの値</param>
    public void Damage(int value)
    {
        _enemyModel.Damage(value);
        _enemyView.DamageMove();
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.TryGetComponent(out BallView ballView))
        {
            _eventEnumProperty.Value = InGameCycle.EventEnum.BallRespawn;
            Damage(1);
        }
    }
}

