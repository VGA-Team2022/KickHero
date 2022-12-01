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
    [Header("�f�o�b�O�p����")]
    [Tooltip("���Z�b�g")]
    [SerializeField] bool _reset = false;

    BallModel _ballModel;
    bool _isPositionSubscribe = false;
    bool IsPositionSubscribe
    {
        get
        {
            Debug.Log($"get{_isPositionSubscribe}");
            return _isPositionSubscribe;
        }
        set
        {
            Debug.Log($"set{_isPositionSubscribe}");
            _isPositionSubscribe = value;
        }
    }

    private BallModel BallModel
    {
        get
        {
            if (!IsPositionSubscribe)
            {
                ViewSubscribe();
            }
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
            if (!_view)
            {
                _view = FindObjectOfType<BallView>();
                if (!_view)
                {
                    Debug.LogError("BallView��������܂���ł����B");
                    return null;
                }
            }
            if (!IsPositionSubscribe)
            {
                ViewSubscribe();
            }
            return _view;
        }
    }

    /// <summary>�����蔻�����邩�ۂ�</summary>
    public bool IsCollision { get => View.IsCollision; set { View.IsCollision = value; } }

    /// <summary>�{�[���̏����ʒu</summary>
    public Vector3 StartPosition { get => BallModel.StartPosition; }

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
    public BallPresenter Shoot()
    {
        BallModel.Shoot();
        return this;
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

    public void Init(System.Action<InGameCycle.EventEnum> action)
    {
        if (View)
        {
            BallModel = new BallModel(value => View.Position = value, View.gameObject, View.transform.position, action); ;
        }
        ValueSet();
    }

    public void Init()
    {
        if (View)
        {
            BallModel = new BallModel(value => View.Position = value, View.gameObject, View.transform.position);
        }
        ValueSet();
    }

    void ValueSet()
    {
        if (BallModel == null) { return; }
        BallModel.Mode = _mode;
        BallModel.Acceleration = _acceleration;
        BallModel.Speed = _speed;
        BallModel.CalculationTime = _calculationTime;
    }

    bool ViewSubscribe()
    {
        if (_view)
        {
            IsPositionSubscribe = true;
            _view.PositionSubscribe(value =>
                    {
                        Debug.Log(1);
                        if (!Application.isPlaying)
                        {
                            BallModel.StartPosition = value;
                        }
                    }, this);
            return true;
        }
        return false;
    }


#if UNITY_EDITOR

    private void OnValidate()
    {
        ValueSet();
        if (_reset)
        {
            Reset();
        }
    }
    private void Reset()
    {
        //_view.
    }

#endif
}