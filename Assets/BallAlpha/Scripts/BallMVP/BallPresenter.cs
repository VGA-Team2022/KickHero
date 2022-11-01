using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallPresenter : MonoBehaviour
{
    [SerializeField] BallView _view;
    [SerializeField] float _speed = 1;
    [SerializeField] float _acceleration = 0;
    [SerializeField] BallModel.CarryMode _mode = BallModel.CarryMode.Time;


    // Start is called before the first frame update
    void Start()
    {
        BallModel ballModel = new BallModel(
            value =>
            {
                _view.Position = value;
            },
            _view.gameObject);
        ballModel.Mode = _mode;
        ballModel.Acceleration = _acceleration;
        ballModel.Speed = _speed;
    }
}
