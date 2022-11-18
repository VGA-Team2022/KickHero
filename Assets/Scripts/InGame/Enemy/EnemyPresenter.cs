using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 敵のデータと表示を使うためのスクリプト
/// </summary>
public class EnemyPresenter : MonoBehaviour,IAttack,IDamage
{
    /// <summary>敵のデータに関してのクラス</summary>
    EnemyModel _enemyModel = null;

    /// <summary>敵の表示に関してのクラス</summary>
    EnemyView _enemyView = null;

    /// <summary>敵の最大HP</summary>
    [SerializeField] int _enemyHp = 20;

    /// <summary>通常攻撃かどうか判定するフラグ</summary>
    bool _normalAttack = true;

    /// <summary>攻撃の感覚</summary>
    [SerializeField] float _attackTime = 3;

    float _time = 0;

    int _count = 0;

    /// <summary>大技攻撃を繰り出す感覚</summary>
    [SerializeField] int _specialAttackNum = 4;

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    /// <summary>
    /// HPスライダーの変更
    /// </summary>
    void Init()
    {
        _enemyModel = new EnemyModel(
            _enemyHp,
            x =>
            {
                _enemyView.ChangeSliderValue(_enemyHp, x);

                if(x <= 0)
                {
                    _enemyView.DeathMove();
                }
            },
            _enemyView.gameObject);
    }

    private void Update()
    {
        _time += Time.deltaTime;

        if(_time > _attackTime)
        {
            if(_count == _specialAttackNum)
            {
                _normalAttack = false;
                Attack();
            }
            else
            {
                _count++;
                Attack();
            }
        }
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
            _time = 0;
        }
        else
        {
            //プレイヤーのダメージ関数を呼ぶ
            //Damage(_enemyModel._specialEnemyPower);
            _enemyView.SpecialAttackMove();
            _normalAttack = true;
            _count = 0;
            _time = 0;
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
}

