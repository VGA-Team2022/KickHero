using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : ISequence
{
    EnemyPresenter _presenter;

    public Enemy(System.Action<InGameCycle.EventEnum> action)
    {
        Initialize(action);
    }
    public void Initialize(System.Action<InGameCycle.EventEnum> action)
    {
        if (!_presenter)
        {
            _presenter = GetMonoBehaviorInstansInScene<EnemyPresenter>();
        }
        _presenter.Init(action);
    }

    public void OnUpdate()
    {
        
    }
    private T GetMonoBehaviorInstansInScene<T>() where T : MonoBehaviour
    {
        T instans = GameObject.FindObjectOfType<T>();
        if (instans)
        {
            return instans;
        }
        else
        {
            GameObject go = new GameObject(nameof(T));
            return go.AddComponent<T>();
        }
    }
}
