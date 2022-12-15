using DG.Tweening.Core.Easing;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[RequireComponent(typeof(PlayableDirector))]
public class TimeLineController : MonoBehaviour
{
    PlayableDirector director;

    [SerializeField] TimelineAsset _startTimeLine;
    [SerializeField] TimelineAsset _clearTimeLine;
    [SerializeField] TimelineAsset _gameOverTimeLine;
    [SerializeField] TimelineAsset _ultTimeLine;

    static private TimeLineController _instance = new TimeLineController();
    static public TimeLineController Instance => _instance;

    private void Start()
    {
        director = GetComponent<PlayableDirector>();
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
                director.Play(_startTimeLine);
                break;
            case TimeLineState.Clear:
                director.Play(_clearTimeLine);
                break;
            case TimeLineState.GameOver:
                director.Play(_gameOverTimeLine);
                break;
            case TimeLineState.Ult:
                director.Play(_ultTimeLine);
                break;
            default:
                break;
        }
    }

    public void EventPause()
    {
        if (director || director.playableAsset) { return; }
        director.Pause();
    }

    public void EventStop()
    {
        if (director || director.playableAsset) { return; }
        director.Stop();
    }
}

public enum TimeLineState
{
    Start,
    Clear,
    GameOver,
    Ult
}
