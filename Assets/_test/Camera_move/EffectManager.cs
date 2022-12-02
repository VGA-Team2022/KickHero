using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    [SerializeField, Tooltip("エフェクトのプレハブ")]
    EffectSetting[] _effPrefab;
    [SerializeField,Tooltip("生成するエフェクトの親となるオブジェクト")]
    Transform Parent;
    [SerializeField, Tooltip("最大スポーン数")]
    int _capacitySize = 10;

    ObjectPool<EffectSetting> _effPool = new ObjectPool<EffectSetting>();
    // Start is called before the first frame update
    void Start()
    {
        _effPool.SetBaseObj(_effPrefab[0],Parent);
        _effPool.SetCapacity(_capacitySize);
    }
    /// <summary>
    /// エフェクトを特定の場所に生成する
    /// </summary>
    /// <param name="Pos"></param>
    public void InstancetiateEff(Vector3 Pos)
    {
        var eff = _effPool.Instancetiate();

        eff.gameObject.transform.position = Pos;    
    }

}
