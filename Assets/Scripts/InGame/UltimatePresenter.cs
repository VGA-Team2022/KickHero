using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UltimatePresenter : MonoBehaviour
{
    [SerializeField,Tooltip("�r���[�N���X�̃C���X�^���X")]
    UltimateView _ultimateView = null;

    /// <summary>���f���̃C���X�^���X</summary>
    UltimateModel _ultimateModel = null;
    [SerializeField,Tooltip("�A���e�B���b�g�̍ő�l")]
    int _maxUltimateValue = 10;
    void Start()
    {
        Init();
    }

    public void Init()
    {
        //�C���X�^���X�����A������(�ő�l,Action<int>,GameObject)
        _ultimateModel = new UltimateModel(
            _maxUltimateValue,
            x =>
            {
                _ultimateView.ChangeGaugeValue(_maxUltimateValue,x);
            },
            _ultimateView.gameObject);
    }

    /// <summary>
    /// �l���O������ύX����֐�
    /// </summary>
    public void ChangeValue(int value)
    {
        _ultimateModel.ChangeUltimateValue(value);
    }
}
