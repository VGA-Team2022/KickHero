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

        _ultimateView.Init(() => { TimeLineController.Instance.EventPlay(TimeLineState.Ult); });
    }

    /// <summary>
    /// �l���O������ύX����֐�
    /// </summary>
    public void ChangeValue(int value)
    {
        _ultimateModel.ChangeUltimateValue(value);
    }
}
