using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class UltimatePresenter : MonoBehaviour
{
    [SerializeField,Tooltip("�r���[�N���X�̃C���X�^���X")]
    UltimateView _ultimateView = null;

    /// <summary>���f���̃C���X�^���X</summary>
    UltimateModel _ultimateModel = null;
    [SerializeField,Tooltip("�A���e�B���b�g�̍ő�l")]
    int _maxUltimateValue = 10;
    [SerializeField, Tooltip("�A���e�B���b�g�̍U����")]
    int _ultimateDamage = 10;
    bool _isUltimate = false;
    public bool IsUltimate => _isUltimate;

    public bool IsClearTimeline { get => _isClearTimeline; set => _isClearTimeline = value; }

    IDamage _enemy;

    public void Init(IDamage enemy)
    {
        _enemy = enemy;
        //�C���X�^���X�����A������(�ő�l,Action<int>,GameObject)
        _ultimateModel = new UltimateModel(
            _maxUltimateValue,
            x =>
            {
                _ultimateView.ChangeGaugeValue(_maxUltimateValue,x);
            },
            _ultimateView.gameObject);

        _ultimateView.Init(() => {
            TimeLineController.Instance.EventPlay(TimeLineState.Ult);
            SoundManagerPresenter.Instance.CriAtomVoicePlay("Voice_Push_001");
            _isUltimate=true;
        });
    }

    public void StopUltimate()
    {
        _enemy.Damage(_ultimateDamage);
        _isUltimate = false;
    }

    /// <summary>
    /// �l���O������ύX����֐�
    /// </summary>
    public void ChangeValue(int value)
    {
        _ultimateModel.ChangeUltimateValue(value);
    }

    bool _isClearTimeline = false;


    public void EndClearTimeLine()
    {
        IsClearTimeline = false;
    }
}
