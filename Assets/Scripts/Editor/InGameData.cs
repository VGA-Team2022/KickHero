using System;
using UnityEngine;

[Serializable]
public class InGameData : ScriptableObject
{
    [SerializeField]
    private int _sampleIntValue;

    [SerializeField]
    private string _name ="default";

    public int SampleIntValue
    {
        get { return _sampleIntValue; }
#if UNITY_EDITOR
        set { _sampleIntValue = Mathf.Clamp(value, 0, int.MaxValue); }
#endif
    }

    public string Name
    {
        get { return _name; }
#if UNITY_EDITOR
        set { _name = value; }
#endif
    }
}
