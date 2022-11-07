using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallPresenter : MonoBehaviour
{
    [SerializeField] BallView _view;
    [SerializeField] float _speed = 1;
    [SerializeField] float _acceleration = 0;
    [SerializeField] float _calculationTime = 0;
    [SerializeField] BallModel.CarryMode _mode = BallModel.CarryMode.Time;
    [SerializeField] Transform _startTransfrom = default;

    BallModel _ballModel;

    public BallModel BallModel
    {
        get
        {
            if(_ballModel == null)
            {
                Init();
            }
            return _ballModel;
        }
        set => _ballModel = value;
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
        _ballModel = new BallModel(
            value =>
            {
                _view.Position = value;
            },
            _view.gameObject);
        ValueSet();
    }

    void ValueSet()
    {
        if(_ballModel == null) { return; }
        _ballModel.Mode = _mode;
        _ballModel.Acceleration = _acceleration;
        _ballModel.Speed = _speed;
        _ballModel.StartTransform = _startTransfrom;
        _ballModel.CalculationTime = _calculationTime;
    }
}