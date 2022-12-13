using CriWare;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("AtomSorce")]
    [SerializeField] CriAtomSource _atomSESource;
    [SerializeField] CriAtomSource _atomBGMSource;
    [SerializeField] CriAtomSource _atomVoiceSorce;

    [Header("BGM��CueSheet")]
    [SerializeField] string _bgmCueName = "";

    [SerializeField] float _changeSpeed = 0.5f;

    const string BGMCueSheet = "BGM";
    const string VoiceCueSheet = "Voice";

    /// <summary>
    /// SE/ME���Đ�����ׂ̊֐�
    /// </summary>
    /// <param name="cueSheet"></param>
    /// <param name="cueName"></param>
    public void CriAtomPlay(CueSheet cueSheet, string cueName)
    {
        if (!_atomSESource)
        {
            Debug.Log("CriAtomSorce������܂���");
            return;
        }

        //�ݒ肵�čĐ�
        _atomSESource.cueSheet = cueSheet.ToString();
        _atomSESource.cueName = cueName;
        _atomSESource.Play();
    }

    void ChangeBGM(string cueName)
    {
        //Volume�̃t�F�[�h�A�E�g
        DOVirtual.Float(_atomBGMSource.volume, 0, _changeSpeed / 2, value => _atomBGMSource.volume = value)
            .OnComplete(() =>
            {
                //�ݒ肵�čĐ�
                _atomBGMSource.cueName = cueName;
                _atomBGMSource.Play();

                //Volume�̃t�F�[�h�C��
                DOVirtual.Float(_atomBGMSource.volume, 1, _changeSpeed / 2, value => _atomBGMSource.volume = value);
            });
    }

    /// <summary>
    /// BGM���Đ�����
    /// </summary>
    /// <param name="cueName"></param>
    public void CriAtomBGMPlay(string cueName)
    {
        if (!_atomBGMSource)
        {
            Debug.Log("CriAtomBGMSorce������܂���");
            return;
        }

        //CueSheet��BGM�Ŗ�����ΐݒ�
        if (_atomBGMSource.cueSheet != BGMCueSheet)
            _atomBGMSource.cueSheet = BGMCueSheet;

        ChangeBGM(cueName);
    }

    /// <summary>
    /// Voice���Đ�����
    /// </summary>
    /// <param name="cueName"></param>
    public void CriAtomVoicePlay(string cueName)
    {
        if (!_atomVoiceSorce)
        {
            Debug.Log("CriAtomVoiceSorce������܂���");
            return;
        }

        //CueSheet��Voice�Ŗ�����ΐݒ�
        if (_atomVoiceSorce.cueSheet != VoiceCueSheet)
            _atomVoiceSorce.cueSheet = VoiceCueSheet;

        _atomBGMSource.cueName = cueName;
        _atomBGMSource.Play();
    }

    //�ǂ����̃^�C�~���O�ŉ����~�߂�K�v���o�Ă��邩���Ȃ̂ŁA�K�v�ȂƂ��ɃR�����g�����E�R�[�h�̍X�V
    public void CriAtomStop(CriAtomSource cri)
    {
        if (!cri)
        {
            Debug.Log($"CriAtomSorce������܂���");
            return;
        }
        cri.Stop();
    }
}
public enum CueSheet
{
    SE,
    ME,
}