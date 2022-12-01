using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    [SerializeField]
    GameObject[] _effPrefab;
    [SerializeField]
    Transform Parent;
    [SerializeField]
    int _capacitySize = 10;

    ObjectPool<EffectSetting> _effPool = new ObjectPool<EffectSetting>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
