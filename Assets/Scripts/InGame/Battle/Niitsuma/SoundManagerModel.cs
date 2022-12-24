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
    /// SE/MEを再生する為の関数
    /// </summary>
    /// <param name="cueSheet"></param>
    /// <param name="cueName"></param>
    public void CriAtomPlay(CueSheet cueSheet, string cueName)
    {
        if (!AtomSESource)
        {
            Debug.Log("CriAtomSorceがありません");
            return;
        }

        //設定して再生
        AtomSESource.cueSheet = cueSheet.ToString();
        AtomSESource.cueName = cueName;
        AtomSESource.Play();
    }

    void ChangeBGM(string cueName)
    {
        //Volumeのフェードアウト
        DOVirtual.Float(AtomBGMSource.volume, 0, ChangeSpeed / 2, value => AtomBGMSource.volume = value)
            .OnComplete(() =>
            {
                //設定して再生
                AtomBGMSource.cueName = cueName;
                AtomBGMSource.Play();

                //Volumeのフェードイン
                DOVirtual.Float(AtomBGMSource.volume, 1, ChangeSpeed / 2, value => AtomBGMSource.volume = value);
            });
    }

    /// <summary>
    /// BGMを再生する
    /// </summary>
    /// <param name="cueName"></param>
    public void CriAtomBGMPlay(string cueName)
    {
        if (!AtomBGMSource)
        {
            Debug.Log("CriAtomBGMSorceがありません");
            return;
        }

        //CueSheetがBGMで無ければ設定
        if (AtomBGMSource.cueSheet != BGMCueSheet)
            AtomBGMSource.cueSheet = BGMCueSheet;

        ChangeBGM(cueName);
    }

    /// <summary>
    /// Voiceを再生する
    /// </summary>
    /// <param name="cueName"></param>
    public void CriAtomVoicePlay(string cueName)
    {
        if (!AtomVoiceSorce)
        {
            Debug.Log("CriAtomVoiceSorceがありません");
            return;
        }

        //CueSheetがVoiceで無ければ設定
        if (AtomVoiceSorce.cueSheet != VoiceCueSheet)
            AtomVoiceSorce.cueSheet = VoiceCueSheet;

        AtomVoiceSorce.cueName = cueName;
        AtomVoiceSorce.Play();
    }

    //どこかのタイミングで音を止める必要が出てくるかもなので、必要なときにコメント解除・コードの更新
    public void CriAtomStop(CriAtomSource cri)
    {
        if (!cri)
        {
            Debug.Log($"CriAtomSorceがありません");
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