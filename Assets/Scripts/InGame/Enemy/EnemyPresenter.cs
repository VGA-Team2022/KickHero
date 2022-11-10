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
        }
        else
        {
            //�v���C���[�̃_���[�W�֐����Ă�
            //Damage(_enemyModel._specialEnemyPower);
            _enemyView.SpecialAttackMove();
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

