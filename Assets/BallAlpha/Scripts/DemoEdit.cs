using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DemoEdit : MonoBehaviour
{
    [SerializeField] BallMove _ball;
    [SerializeField] InputField _acceleration;
    [SerializeField] InputField _speed;
    [SerializeField] Text _mode;
    [SerializeField] Text _speedText;
    [SerializeField] Text _speedText2;

    private void Start()
    {
        if (_ball.Mode == BallMove.CarryMode.Distance)
        {
            _mode.text = $"Mode : {nameof(BallMove.CarryMode.Distance)}";
            _speedText.text = "m/s";
            _speedText.text = "‘¬“x";
        }
        else
        {
            _mode.text = $"Mode : {nameof(BallMove.CarryMode.Time)}";
            _speedText.text = "”{";
            _speedText.text = "”{—¦";
        }
        _acceleration.text = _ball.Acceleration.ToString();
        _speed.text = _ball.Speed.ToString();
    }


    public void Acceleration()
    {
        if (_ball && _acceleration)
        {
            _ball.Acceleration = float.Parse(_acceleration.text);
        }
    }
    public void Speed()
    {
        if (_ball && _speed)
        {
            _ball.Speed = float.Parse(_speed.text);
        }
    }

    public void Mode()
    {
        if (_ball && _mode)
        {
            if (_ball.Mode == BallMove.CarryMode.Distance)
            {
                _mode.text = $"Mode : {nameof(BallMove.CarryMode.Time)}";
                _ball.Mode = BallMove.CarryMode.Time;
                _speedText.text = "”{";
                _speedText2.text = "”{—¦";
            }
            else
            {
                _mode.text = $"Mode : {nameof(BallMove.CarryMode.Distance)}";
                _ball.Mode = BallMove.CarryMode.Distance;
                _speedText.text = "m/s";
                _speedText2.text = "‘¬“x";
            }
        }
    }
}
