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
    [Tooltip("使用するエフェクトの種類")]
    public Effects Name;

    public ObjectPool<EffectSetting> EffPool = new ObjectPool<EffectSetting>();
}

public class EffectManager : Singleton<EffectManager>
{
    [SerializeField, Tooltip("生成するエフェクト")]
    PoolEffect[] Effects;
    Dictionary<Effects, PoolEffect> _effDict = new Dictionary<Effects, PoolEffect>();


    // Start is called before the first frame update
    void Start()
    {
        foreach (var eff in Effects)
        {
            eff.EffPool.SetBaseObj(eff.Prefab, eff.Parent);
            eff.EffPool.SetCapacity(eff.CapacitySize);

            _effDict.Add(eff.Name, eff);
        }
    }

    /// <summary>
    /// 指定したIDのエフェクトを特定の場所に生成する
    /// </summary>
    /// <param name="Pos">生成時のPostion</param>
    /// <param name="ID">インスペクター上で設定した配列の要素数</param>
    public void InstancetiateEff(Vector3 Pos , Effects ID)
    {
        if (Pos == null) { return; }
        var obj = _effDict[ID].EffPool.Instancetiate();
        obj.gameObject.transform.position = Pos;
    }
}

public enum Effects
{
    Hit_01,
    Hit_02,
    Beem,
    Bound,
    BoxOpenShine,
    Burst,
    Heal,
    Resurrection,
    StageClear,
    Vanish,
    Guard,
    Charge_01,
    Charge_02,
}
