using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> where T : UnityEngine.Object, IObjectPool
{
    T _baseObject = null;
    Transform _parent = null;
    List<T> _pool = new List<T>();
    int _index = 0;
    public List<T> _getPool { get; private set; }

    public void SetBaseObj(T obj, Transform parent)
    {
        _baseObject = obj;
        _parent = parent;
    }

    public void Pooling(T obj)
    {
        obj.InactiveInstantiate();
        _pool.Add(obj);
    }

    public void SetCapacity(int size)
    {
        if (size < _pool.Count) { return; }

        for (int i = 0; i < size; i++)
        {
            T obj = default(T);

            if (_parent)
            {
                obj = GameObject.Instantiate(_baseObject, _parent);
            }
            else
            {
                obj = GameObject.Instantiate(_baseObject);
            }
            Pooling(obj);
        }
    }

    public T Instancetiate()
    {
        T ret = null;

        for (int i = 0; i < _pool.Count; i++)
        {
            _index = i;

            if (_pool[_index].IsActive) { continue; }
            _pool[_index].Create();
            ret = _pool[_index];
            break;
        }

        return ret;
    }
}
