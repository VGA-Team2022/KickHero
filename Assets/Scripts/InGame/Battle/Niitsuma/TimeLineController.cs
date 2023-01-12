using DG.Tweening.Core.Easing;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class TimeLineController : MonoBehaviour
{
    PlayableDirector _currentDirector;

    [SerializeField] PlayableDirector _startTimeLine;
    [SerializeField] PlayableDirector _clearTimeLine;
    [SerializeField] PlayableDirector _gameOverTimeLine;
    [SerializeField] PlayableDirector _ultTimeLine;

    static private TimeLineController _instance = new TimeLineController();
    static public TimeLineController Instance => _instance;

    private void Start()
    {
        _currentDirector = GetComponent<PlayableDirector>();

        EventPlay(TimeLineState.Start);
    }

    /// <summary>
    /// �C���X�y�N�^�[�ォ��̐ݒ�p
    /// </summary>
    /// <param name="name"></param>
    public void EventPlay(string name)
    {
        TimeLineState state = (TimeLineState)Enum.Parse(typeof(TimeLineState), name);
        EventPlay(state);
    }

    public void EventPlay(TimeLineState state)
    {
        switch (state)
        {
            case TimeLineState.Start:
                _currentDirector = _startTimeLine;
                _startTimeLine.Play();
                break;
            case TimeLineState.Clear:
                _currentDirector = _clearTimeLine;
                _clearTimeLine.Play();
                break;
            case TimeLineState.GameOver:
                _currentDirector = _gameOverTimeLine;
                _gameOverTimeLine.Play();
                break;
            case TimeLineState.Ult:
                _currentDirector = _ultTimeLine;
                _ultTimeLine.Play();
                break;
            default:
                break;
        }
    }

    public void EventPause()
    {
        if (_currentDirector || _currentDirector.playableAsset) { return; }
        _currentDirector.Pause();
    }

    public void EventStop()
    {
        if (_currentDirector || _currentDirector.playableAsset) { return; }
        _currentDirector.Stop();
    }
}

public enum TimeLineState
{
    Start,
    Clear,
    GameOver,
    Ult
}
