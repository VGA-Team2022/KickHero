using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    public static T Instance { get; private set; } = null;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this as T;
            OnAwake();
        }
    }

    /// <summary>
    /// îhê∂êÊópÇÃAwakeä÷êî
    /// </summary>
    protected virtual void OnAwake() { }


    private void OnDestroy()
    {
        if (Instance == this)
        {
            OnRelease();
            Instance = null;
        }
    }
    /// <summary>
    /// îhê∂êÊópÇÃOnDestroyä÷êî
    /// </summary>
    protected virtual void OnRelease() { }
}
