using System.Collections;
using System.Collections.Generic;
using State = StateMachine<SampleInGameCycle.EventEnum, SampleInGameCycle>.State;
using UnityEngine;

public class SampleInGameCycle : MonoBehaviour
{
    StateMachine<EventEnum,SampleInGameCycle> _stateMachine;

    public enum EventEnum
    {
        GameStart,
        GameOver,
        Retry
    }


    private void Awake()
    {
        //インスタンスを初期化
        _stateMachine = new StateMachine<EventEnum,SampleInGameCycle>(this);
        
        //遷移を定義
        _stateMachine.AddTransition<StartState, InGameState>(EventEnum.GameStart);
        _stateMachine.AddTransition<InGameState, ResultState>(EventEnum.GameOver);
        _stateMachine.AddTransition<ResultState, StartState>(EventEnum.Retry);

        //最初のStateを設定
        _stateMachine.StartSetUp<StartState>();
    }

    public void GameStart()
    {
        _stateMachine.Dispatch(EventEnum.GameStart);
    }

    public void GameOver()
    {
        _stateMachine.Dispatch(EventEnum.GameOver);
    }
    public void Retry()
    {
        _stateMachine.Dispatch(EventEnum.Retry);
    }

    private void Update()
    {
        //_stateMachine.Update();
    }

    private class StartState : State
    {
        protected override void OnEnter(State prevState)
        {
            Debug.Log("スタートステートに入った");
        }
        protected override void OnUpdate()
        {
            Debug.Log("スタートステート実行中");
        }
        protected override void OnExit(State nextState)
        {
            Debug.Log("スタートステートを抜けた");
        }
    }
    private class InGameState : State
    {
        protected override void OnEnter(State prevState)
        {
            Debug.Log("インゲームステートに入った");

        }
        protected override void OnUpdate()
        {
            Debug.Log("インゲーム実行中");
        }
        protected override void OnExit(State nextState)
        {
            Debug.Log("インゲームステートを抜けた");
        }
    }

    private class ResultState : State
    {
        protected override void OnEnter(State prevState)
        {
            Debug.Log("リザルトステートに入った");
        }
        protected override void OnUpdate()
        {
            Debug.Log("リザルトステート実行中");
        }
        protected override void OnExit(State nextState)
        {
            Debug.Log("リザルトステートを抜けた");
        }
    }
}
