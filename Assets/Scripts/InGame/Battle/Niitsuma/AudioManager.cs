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

    [SerializeField] float _changeSpeed = 0.5f;

    ///<summary>
    ///仮SEを再生する為の関数
    /// </summary>
    public void CriAtomSEPlay(string cueName)
    {
        CriAtomPlay("CueSheet_0",cueName);
    }
    /// <summary>
    /// SE/MEを再生する為の関数
    /// </summary>
    /// <param name="cueSheet"></param>
    /// <param name="cueName"></param>
    public void CriAtomPlay(string cueSheet, string cueName)
    {
        if (!_atomSESource)
        {
            Debug.Log("CriAtomSorceがありません");
            return;
        }

        //設定して再生
        _atomSESource.cueSheet = cueSheet.ToString();
        _atomSESource.cueName = cueName;
        _atomSESource.Play();
    }

    void ChangeBGM(string cueName)
    {
        //Volumeのフェードアウト
        DOVirtual.Float(_atomBGMSource.volume, 0, _changeSpeed / 2, value => _atomBGMSource.volume = value)
            .OnComplete(() =>
            {
                //設定して再生
                _atomBGMSource.cueName = cueName;
                _atomBGMSource.Play();

                //Volumeのフェードイン
                DOVirtual.Float(_atomBGMSource.volume, 1, _changeSpeed / 2, value => _atomBGMSource.volume = value);
            });
    }

    /// <summary>
    /// BGMを再生する
    /// </summary>
    /// <param name="cueName"></param>
    public void CriAtomBGMPlay(string cueName)
    {
        if (!_atomBGMSource)
        {
            Debug.Log("CriAtomBGMSorceがありません");
            return;
        }

        //CueSheetがBGMで無ければ設定
        //if (_atomBGMSource.cueSheet != BGMCueSheet)
        //    _atomBGMSource.cueSheet = BGMCueSheet;

        ChangeBGM(cueName);
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
