using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectSetting : MonoBehaviour, IObjectPool
{
    bool _isActive = false;
    public bool IsActive => _isActive;

    public void InactiveInstantiate()
    {
        gameObject.SetActive(false);
        _isActive = false;
    }
    public void Create()
    {
        gameObject.SetActive(true);
        _isActive = true;
    }
    public void Destroy()
    {
        gameObject.SetActive(false);
        _isActive = false;
    }
}
