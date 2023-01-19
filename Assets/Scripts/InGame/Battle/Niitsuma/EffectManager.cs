using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PoolEffect
{
    [Tooltip("�G�t�F�N�g�̃v���n�u")]
    public EffectSetting Prefab;
    [Tooltip("��������G�t�F�N�g�̐e�ƂȂ�Transform(�I�u�W�F�N�g)")]
    public Transform Parent;
    [Tooltip("�ő吶����")]
    public int CapacitySize = 10;
    [Tooltip("�g�p����G�t�F�N�g�̎��")]
    public Effects Name;

    public ObjectPool<EffectSetting> EffPool = new ObjectPool<EffectSetting>();
}

public class EffectManager : Singleton<EffectManager>
{
    [SerializeField, Tooltip("��������G�t�F�N�g")]
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
    /// �w�肵��ID�̃G�t�F�N�g�����̏ꏊ�ɐ�������
    /// </summary>
    /// <param name="Pos">��������Postion</param>
    /// <param name="ID">�C���X�y�N�^�[��Őݒ肵���z��̗v�f��</param>
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
