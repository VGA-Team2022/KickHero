using CriWare;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class SoundManagerModel
{
    public CriAtomSource AtomSESource;
    public CriAtomSource AtomBGMSource;
    public CriAtomSource AtomVoiceSorce;

    public float ChangeSpeed = 0.5f;

    const string BGMCueSheet = "BGM";
    const string VoiceCueSheet = "Voice";

    public void SetVolume(CriAtomSource cri , float volume)
    {
        cri.volume = volume;
    }

    /// <summary>
    /// SE/ME���Đ�����ׂ̊֐�
    /// </summary>
    /// <param name="cueSheet"></param>
    /// <param name="cueName"></param>
    public void CriAtomPlay(CueSheet cueSheet, string cueName)
    {
        if (!AtomSESource)
        {
            Debug.Log("CriAtomSorce������܂���");
            return;
        }

        //�ݒ肵�čĐ�
        AtomSESource.cueSheet = cueSheet.ToString();
        AtomSESource.cueName = cueName;
        AtomSESource.Play();
    }

    void ChangeBGM(string cueName)
    {
        //Volume�̃t�F�[�h�A�E�g
        DOVirtual.Float(AtomBGMSource.volume, 0, ChangeSpeed / 2, value => AtomBGMSource.volume = value)
            .OnComplete(() =>
            {
                //�ݒ肵�čĐ�
                AtomBGMSource.cueName = cueName;
                AtomBGMSource.Play();

                //Volume�̃t�F�[�h�C��
                DOVirtual.Float(AtomBGMSource.volume, 1, ChangeSpeed / 2, value => AtomBGMSource.volume = value);
            });
    }

    /// <summary>
    /// BGM���Đ�����
    /// </summary>
    /// <param name="cueName"></param>
    public void CriAtomBGMPlay(string cueName)
    {
        if (!AtomBGMSource)
        {
            Debug.Log("CriAtomBGMSorce������܂���");
            return;
        }

        //CueSheet��BGM�Ŗ�����ΐݒ�
        if (AtomBGMSource.cueSheet != BGMCueSheet)
            AtomBGMSource.cueSheet = BGMCueSheet;

        ChangeBGM(cueName);
    }

    /// <summary>
    /// Voice���Đ�����
    /// </summary>
    /// <param name="cueName"></param>
    public void CriAtomVoicePlay(string cueName)
    {
        if (!AtomVoiceSorce)
        {
            Debug.Log("CriAtomVoiceSorce������܂���");
            return;
        }

        //CueSheet��Voice�Ŗ�����ΐݒ�
        if (AtomVoiceSorce.cueSheet != VoiceCueSheet)
            AtomVoiceSorce.cueSheet = VoiceCueSheet;

        AtomVoiceSorce.cueName = cueName;
        AtomVoiceSorce.Play();
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