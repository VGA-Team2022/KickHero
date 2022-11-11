using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject.SpaceFighter;

public class BallPresenter : MonoBehaviour
{
    [SerializeField] BallView _view;
    [SerializeField] float _speed = 1;
    [SerializeField] float _acceleration = 0;
    [SerializeField] float _calculationTime = 0;
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
                Debug.LogWarning($"{nameof(_view)}‚ªƒAƒTƒCƒ“‚³‚ê‚Ä‚¢‚Ü‚¹‚ñ");
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
            _ballModel = new BallModel(
            value =>
            {
                _view.Position = value;
            },
            _view.gameObject);
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