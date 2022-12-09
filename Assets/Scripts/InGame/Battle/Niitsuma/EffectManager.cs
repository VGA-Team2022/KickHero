using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PoolEffect
{
    [Tooltip("エフェクトのプレハブ")]
    public EffectSetting Prefab;
    [Tooltip("生成するエフェクトの親となるTransform(オブジェクト)")]
    public Transform Parent;
    [Tooltip("最大生成数")]
    public int CapacitySize = 10;

    public ObjectPool<EffectSetting> EffPool = new ObjectPool<EffectSetting>();
}

public class EffectManager : MonoBehaviour
{
    [SerializeField, Tooltip("生成するエフェクト")]
    PoolEffect[] Effects;

    
    // Start is called before the first frame update
    void Start()
    {
        foreach (var eff in Effects)
        {
            eff.EffPool.SetBaseObj(eff.Prefab, eff.Parent);
            eff.EffPool.SetCapacity(eff.CapacitySize);
        }
    }

    /// <summary>
    /// 指定したIDのエフェクトを特定の場所に生成する
    /// </summary>
    /// <param name="Pos">生成時のPostion</param>
    /// <param name="ID">インスペクター上で設定した配列の要素数</param>
    public void InstancetiateEff(Vector3 Pos ,int ID)
    {
        if(ID < 0 || ID >= Effects.Length ) { return; }

        var eff = Effects[ID].EffPool.Instancetiate();

        eff.gameObject.transform.position = Pos;
    }
}
