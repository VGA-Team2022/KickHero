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

    public ObjectPool<EffectSetting> EffPool = new ObjectPool<EffectSetting>();
}

public class EffectManager : MonoBehaviour
{
    [SerializeField, Tooltip("��������G�t�F�N�g")]
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
    /// �w�肵��ID�̃G�t�F�N�g�����̏ꏊ�ɐ�������
    /// </summary>
    /// <param name="Pos">��������Postion</param>
    /// <param name="ID">�C���X�y�N�^�[��Őݒ肵���z��̗v�f��</param>
    public void InstancetiateEff(Vector3 Pos ,int ID)
    {
        if(ID < 0 || ID >= Effects.Length ) { return; }

        var eff = Effects[ID].EffPool.Instancetiate();

        eff.gameObject.transform.position = Pos;
    }
}
