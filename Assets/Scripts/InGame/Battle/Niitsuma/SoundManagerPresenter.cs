using CriWare;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class SoundManagerPresenter : DDOLSingleton<SoundManagerPresenter>
{
    [Header("View")]
    [SerializeField] SoundManagerView _view;

    [Header("AtomSorce")]
    [SerializeField] CriAtomSource _atomSESource;
    [SerializeField] CriAtomSource _atomBGMSource;
    [SerializeField] CriAtomSource _atomVoiceSorce;

    [SerializeField] float _changeSpeed = 0.5f;

    ReactiveProperty<InGameCycle.EventEnum> _eventProperty;

    SoundManagerModel _model = new SoundManagerModel();

    private void Start()
    {
        Init();
    }
    public void Init(System.Action<InGameCycle.EventEnum> eventAction)
    {
        //シーケンスの遷移を指定。
        _eventProperty = new ReactiveProperty<InGameCycle.EventEnum>(InGameCycle.EventEnum.None);
        _eventProperty.Subscribe(eventAction).AddTo(this.gameObject);
        ValueSet();
    }

    public void Init()
    {
        ValueSet();
    }

    public void CriAtomSEPlay(string cueName)
    {
        _model.CriAtomPlay(CueSheet.SE, cueName);
    }
    public void CriAtomMEPlay(string cueName)
    {
        _model.CriAtomPlay(CueSheet.ME, cueName);
    }
    public void CriAtomBGMPlay(string cueName)
    {
        _model.CriAtomBGMPlay(cueName);
    }
    public void CriAtomVoicePlay(string cueName)
    {
        _model.CriAtomVoicePlay(cueName);
    }
    public void CriAtomStop(CriAtomSource cri)
    {
        _model.CriAtomStop(cri);
    }

    public void SetVolume(CriAtomSource cri)
    {
        if (_model == null || _view == null) { return; }

        if (cri == _model.AtomBGMSource) { _model.SetVolume(_model.AtomBGMSource, _view.BGMSlider.value); }
        else if (cri == _model.AtomSESource) { _model.SetVolume(_model.AtomSESource, _view.SESlider.value); }
        else { _model.SetVolume(_model.AtomVoiceSorce, _view.VoiceSlider.value); }
    }
    void ValueSet()
    {
        _model.AtomBGMSource = _atomBGMSource;
        _model.AtomSESource = _atomSESource;
        _model.AtomVoiceSorce = _atomVoiceSorce;
        _model.ChangeSpeed = _changeSpeed;
    }
}
