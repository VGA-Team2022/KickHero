using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallModelTest : MonoBehaviour
{
    [SerializeField] Transform _start;
    [SerializeField] Transform _end;
    [SerializeField] int _segment;
    [SerializeField] BallPresenter _presenter;
    // Start is called before the first frame update
    void Start()
    {
        BallRoute route = new BallRoute();
        for(int i = 0; i < _segment; i++)
        {
            float time = 1f / _segment * i;
            route.AddNode(Vector3.Lerp(_start.position, _end.position, time), time);
        }
        _presenter.TryRouteSet(route);
        _presenter.Shoot();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
