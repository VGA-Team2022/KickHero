using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Zenject.SpaceFighter;

public class BallPresenter : MonoBehaviour
{
    [Tooltip("�{�[����View")]
    [SerializeField] BallView _view;
    [Tooltip("�{�[���̑���")]
    [SerializeField] float _speed = 1;
    [Tooltip("�{�[���̉����x")]
    [SerializeField] float _acceleration = 0;
    [Tooltip("�Ō�Ƀ{�[�����΂����x�̎Z�o�Ɏg������")]
    [SerializeField] float _calculationTime = 0;
    [Tooltip("�{�[���̑��x���[�h")]
    [SerializeField] BallModel.CarryMode _mode = BallModel.CarryMode.Time;

    BallModel _ballModel;

    private BallModel BallModel
    {
        get
        {
            if (_ballModel == null)
            {
                Init();
            }
            return _ballModel;
        }
        set => _ballModel = value;
    }

    private BallView View
    {
        get
        {
            if (!_view)
            {
                Debug.LogWarning($"{nameof(_view)}���A�T�C������Ă��܂���");
            }
            return _view;
        }
    }
    /// <summary>���ݎ��s���̓�����L�����Z������</summary>
    public void Cancel()
    {
        BallModel.Cancel();
    }

    /// <summary>�����ʒu�ɖ߂�</summary>
    public void Collection()
    {
        BallModel.Collection();
    }

    /// <summary>���[�g�̐ݒ�����݂�
    /// �{�[���̏󋵂ɂ���Ă͐ݒ�ł��Ȃ�</summary>
    public bool TryRouteSet(BallRoute route)
    {
        return BallModel.TryRouteSet(route);
    }

    /// <summary>�{�[���𔭎˂���</summary>
    public void Shoot()
    {
        BallModel.Shoot();
    }

    /// <summary>���[�g��H��I�������ɌĂԃA�N�V������ݒ肷��</summary>
    public BallPresenter OnCarryEnd(Action action)
    {
        BallModel.OnCarryEnd(action);
        return this;
    }

    /// <summary>�{�[�����q�b�g�������ɌĂ΂��A�N�V������ݒ肷��</summary>
    public BallPresenter OnHit(Action<Collider> action)
    {
        View?.OnHit(action);
        return this;
    }

    /// <summary>�{�[�����B��</summary>
    public void Hide()
    {
        View?.Hide();
    }

    /// <summary>�{�[�����o��</summary>
    public void Display()
    {
        View?.Display();
    }


    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    private void OnValidate()
    {
        ValueSet();
    }

    void Init()
    {
        if (View)
        {
            GameObject go = Instantiate(_view.transform).gameObject;
            go.name = "BallStartPosition";
            _ballModel = new BallModel(
            value =>
            {
                _view.Position = value;
            },
            _view.gameObject, go.transform);
        }
        ValueSet();
    }

    void ValueSet()
    {
        if (_ballModel == null) { return; }
        _ballModel.Mode = _mode;
        _ballModel.Acceleration = _acceleration;
        _ballModel.Speed = _speed;
        _ballModel.CalculationTime = _calculationTime;
    }
}