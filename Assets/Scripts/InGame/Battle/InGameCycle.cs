using System.Collections;
using System.Collections.Generic;
using State = StateMachine<InGameCycle.EventEnum, InGameCycle>.State;
using UnityEngine;
using Unity.VisualScripting;
using Zenject;

public class InGameCycle : MonoBehaviour, IReceivableGameData
{
    StateMachine<EventEnum, InGameCycle> _stateMachine;

    Player _player;
    Enemy _enemy;
    bool[] _isClearedStage;

    [SerializeField,Tooltip("デバッグ用")]
    GameObject _resultPanel = null;

    public enum EventEnum
    {
        GameStart,
        Throw,
        BallRespawn,
        GameOver,
        Pause,
        None
    }

    private void Awake()
    {
        //インスタンスを初期化
        _stateMachine = new StateMachine<EventEnum, InGameCycle>(this);

        //遷移を定義
        _stateMachine.AddTransition<StartState, IdleState>(EventEnum.GameStart);
        _stateMachine.AddTransition<IdleState, AttackChargeState>(EventEnum.Throw);
        _stateMachine.AddTransition<AttackChargeState, IdleState>(EventEnum.BallRespawn);
        _stateMachine.AddAnyTransitionTo<ResultState>(EventEnum.GameOver);

        //最初のStateを設定
        _stateMachine.StartSetUp<StartState>();
        _player = new Player(ChangeState);
        _enemy = new Enemy(ChangeState);

        _resultPanel = GameObject.Find("ResultPanel");
 
        _stateMachine.Owner._resultPanel?.SetActive(false);
    }

    private void Update()
    {
        _stateMachine.Update();
    }

    public void ChangeState(EventEnum eventEnum)
    {
        _stateMachine.Dispatch(eventEnum);
    }

    public void SetClearedStage(bool[] clearedStage)
    {
        _isClearedStage = clearedStage;
    }

    private class StartState : State
    {
        protected override void OnEnter(State prevState)
        {
            Debug.Log("スタートステートに入った");
        }
        protected override void OnUpdate()
        {
            _stateMachine.Dispatch(EventEnum.GameStart);
        }
        protected override void OnExit(State nextState)
        {
            Debug.Log("スタートステートを抜けた");
        }
    }

    private class IdleState : State
    {
        protected override void OnEnter(State prevState)
        {          
            Debug.Log("Idleステートに入った");
        }
        protected override void OnUpdate()
        {
            _stateMachine.Owner._player.OnUpdate();
        }
        protected override void OnExit(State nextState)
        {
            Debug.Log("Idleステートを抜けた");
        }
    }

    private class AttackChargeState : State
    {
        protected override void OnEnter(State prevState)
        {
            Debug.Log("AttackChargeステートに入った");
        }

        protected override void OnUpdate()
        {
            _stateMachine.Owner._player.OnUpdate();
        }
        protected override void OnExit(State nextState)
        {
            Debug.Log("AttackChargeステートを抜けた");
        }
    }

    private class AttackState : State
    {
        protected override void OnEnter(State prevState)
        {
            Debug.Log("こうげきステートに入った");
        }

        protected override void OnExit(State nextState)
        {
            Debug.Log("こうげきステートを抜けた");
        }
    }

    private class UltimateChargeState : State
    {
        protected override void OnEnter(State prevState)
        {
            Debug.Log("必殺技待機ステートに入った");
        }
        protected override void OnUpdate()
        {
            _stateMachine.Owner._player.OnUpdate();
        }
        protected override void OnExit(State nextState)
        {
            Debug.Log("必殺技待機ステートを抜けた");
        }
    }

    private class UltimateAttackState : State
    {
        protected override void OnEnter(State prevState)
        {
            Debug.Log("必殺技ステートに入った");
        }
        protected override void OnExit(State nextState)
        {
            Debug.Log("必殺技ステートを抜けた");
        }
    }

    private class ResultState : State
    {
        protected override void OnEnter(State prevState)
        {
            _stateMachine.Owner._resultPanel?.SetActive(true);
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

    //デバッグ用の関数

    public void GameStart()
    {
        _stateMachine.Dispatch(EventEnum.GameStart);
    }
    public void Throw()
    {
        _stateMachine.Dispatch(EventEnum.Throw);
    }
    public void BallRespawn()
    {
        _stateMachine.Dispatch(EventEnum.BallRespawn);
    }
    public void GameOver()
    {
        _stateMachine.Dispatch(EventEnum.GameOver);
    }
    public void Pause()
    {
        _stateMachine.Dispatch(EventEnum.Pause);
    }
}
