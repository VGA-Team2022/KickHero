using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallPresenter : MonoBehaviour
{
    [SerializeField] BallView _view;
    [SerializeField] float _speed = 1;
    [SerializeField] float _acceleration = 0;
    [SerializeField] BallModel.CarryMode _mode = BallModel.CarryMode.Time;

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

    void Init()
    {
        _ballModel = new BallModel(
            value =>
            {
                _view.Position = value;
            },
            _view.gameObject);
        _ballModel.Mode = _mode;
        _ballModel.Acceleration = _acceleration;
        _ballModel.Speed = _speed;
    }
}
