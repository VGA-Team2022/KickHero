using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DDOLSingleton<T> : MonoBehaviour where T : DDOLSingleton<T>
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
            DontDestroyOnLoad(gameObject);
        }
    }

    /// <summary>
    /// �h����p��Awake�֐�
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
    /// �h����p��OnDestroy�֐�
    /// </summary>
    protected virtual void OnRelease() { }
}