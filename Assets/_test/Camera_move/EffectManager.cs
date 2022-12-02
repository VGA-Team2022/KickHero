using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    [SerializeField, Tooltip("�G�t�F�N�g�̃v���n�u")]
    EffectSetting[] _effPrefab;
    [SerializeField,Tooltip("��������G�t�F�N�g�̐e�ƂȂ�I�u�W�F�N�g")]
    Transform Parent;
    [SerializeField, Tooltip("�ő�X�|�[����")]
    int _capacitySize = 10;

    ObjectPool<EffectSetting> _effPool = new ObjectPool<EffectSetting>();
    // Start is called before the first frame update
    void Start()
    {
        _effPool.SetBaseObj(_effPrefab[0],Parent);
        _effPool.SetCapacity(_capacitySize);
    }
    /// <summary>
    /// �G�t�F�N�g�����̏ꏊ�ɐ�������
    /// </summary>
    /// <param name="Pos"></param>
    public void InstancetiateEff(Vector3 Pos)
    {
        var eff = _effPool.Instancetiate();

        eff.gameObject.transform.position = Pos;    
    }

}
