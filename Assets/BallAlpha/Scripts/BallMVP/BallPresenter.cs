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
                Debug.LogError("_ballModel������������Ă��܂���B");
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

    public void Cancel()
    {
        BallModel.Cancel();
    }

    public void Collection()
    {
        BallModel.Collection();
    }

    public bool TryRouteSet(BallRoute route)
    {
        return BallModel.TryRouteSet(route);
    }

    public void Shoot()
    {
        BallModel.Shoot();
    }

    public BallPresenter OnCarryEnd(Action action)
    {
        BallModel.OnCarryEnd(action);
        return this;
    }

    public BallPresenter OnHit(Action<Collider> action)
    {
        View?.OnHit(action);
        return this;
    }

    public void Hide()
    {
        View?.Hide();
    }

    public void Display()
    {
        View?.Display();
    }

    private void OnValidate()
    {
        ValueSet();
    }

    public void Init(System.Action<InGameCycle.EventEnum> action)
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
            _view.gameObject, go.transform
            ,action);
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