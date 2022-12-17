using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

/// <summary>
/// �G�̃f�[�^�ƕ\�����g�����߂̃X�N���v�g
/// </summary>
public class EnemyHPPresenter : MonoBehaviour, IDamage
{
    /// <summary>�G�̃f�[�^�Ɋւ��ẴN���X</summary>
    EnemyHPModel _enemyModel = null;

    /// <summary>�G�̕\���Ɋւ��ẴN���X</summary>
    [SerializeField]
    EnemyHPView _enemyView = null;

    /// <summary>�G�̍ő�HP</summary>
    [SerializeField] int _enemyHp = 20;

    bool _isDead = false;
    public bool IsDead => _isDead;
    /// <summary>
    /// HP�X���C�_�[�̕ύX
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
    /// �O������l���Q�Ƃ��邽�߂̊֐�
    /// </summary>
    /// <param name="value">�^����_���[�W�̒l</param>
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

