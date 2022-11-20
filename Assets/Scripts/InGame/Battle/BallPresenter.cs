using ModestTree.Util;
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
                //Debug.LogError("_ballModel������������Ă��܂���B");
                _ballModel = new BallModel(_view.transform.position);
            }
            return _ballModel;
        }
        set => _ballModel = value;
    }

    private BallView View
    {
        get
        {
            _view = FindObjectOfType<BallView>();
            if (!_view)
            {
                Debug.LogError("BallView��������܂���ł����B");
            }
            return _view;
        }
    }

    /// <summary>�����蔻�����邩�ۂ�</summary>
    public bool IsCollision { get => View.IsCollision; set { View.IsCollision = value; Debug.Log(value); } }

    /// <summary>���ݎ��s���̓�����L�����Z������</summary>
    public void Cancel()
    {
        BallModel.Cancel();
        if (View)
        {
            View.Rigidbody.useGravity = false;
            View.Rigidbody.velocity= Vector3.zero;
        }
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
        return OnCarryEnd(action, true);
    }
    /// <summary>���[�g��H��I�������ɌĂԃA�N�V������ݒ肷��</summary>
    public BallPresenter OnCarryEnd(Action action, bool reusable)
    {
        BallModel.OnCarryEnd(action, reusable);
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

    private void Start()
    {
        Init();
    }

    private void OnValidate()
    {
        ValueSet();
    }

    public void Init(System.Action<InGameCycle.EventEnum> action)
    {
        if (View)
        {
            _ballModel = new BallModel(
            value =>
            {
                _view.Position = value;
            }, value =>
            {
                _view.Rigidbody.velocity = value;
            },
            _view.gameObject, _view.transform.position
            , action); ;
        }
        ValueSet();
    }

    public void Init()
    {
        if (View)
        {
            _ballModel = new BallModel(
            value =>
            {
                _view.Position = value;
            }, value =>
            {
                _view.Rigidbody.velocity = value;
            },
            _view.gameObject, _view.transform.position
            ).OnCarryEnd(() => { _view.Rigidbody.useGravity = true; }) ;
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