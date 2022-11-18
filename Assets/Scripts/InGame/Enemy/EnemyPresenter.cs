using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// �G�̃f�[�^�ƕ\�����g�����߂̃X�N���v�g
/// </summary>
public class EnemyPresenter : MonoBehaviour,IAttack,IDamage
{
    /// <summary>�G�̃f�[�^�Ɋւ��ẴN���X</summary>
    EnemyModel _enemyModel = null;

    /// <summary>�G�̕\���Ɋւ��ẴN���X</summary>
    EnemyView _enemyView = null;

    /// <summary>�G�̍ő�HP</summary>
    [SerializeField] int _enemyHp = 20;

    /// <summary>�ʏ�U�����ǂ������肷��t���O</summary>
    bool _normalAttack = true;

    /// <summary>�U���̊��o</summary>
    [SerializeField] float _attackTime = 3;

    float _time = 0;

    int _count = 0;

    /// <summary>��Z�U�����J��o�����o</summary>
    [SerializeField] int _specialAttackNum = 4;

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    /// <summary>
    /// HP�X���C�_�[�̕ύX
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
    ///     �U������Ƃ��ɂ�΂��֐�
    /// </summary>
    public void Attack()
    {
        if(_normalAttack == true)
        {
            //�v���C���[�̃_���[�W�֐����Ă�
            //Damage(_enemyModel._enemyPower);
            _enemyView.NormalAttackMove();
            _time = 0;
        }
        else
        {
            //�v���C���[�̃_���[�W�֐����Ă�
            //Damage(_enemyModel._specialEnemyPower);
            _enemyView.SpecialAttackMove();
            _normalAttack = true;
            _count = 0;
            _time = 0;
        }

    }

    /// <summary>
    /// �O������l���Q�Ƃ��邽�߂̊֐�
    /// </summary>
    /// <param name="value">�^����_���[�W�̒l</param>
    public void Damage(int value)
    {
        _enemyModel.Damage(value);
        _enemyView.DamageMove();
    }
}

