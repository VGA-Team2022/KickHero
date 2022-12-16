using State = StateMachine<InGameCycle.EventEnum, InGameCycle>.State;
using UnityEngine;
using System;
using Cysharp.Threading.Tasks;

public class InGameCycle : MonoBehaviour, IReceivableGameData
{
    StateMachine<EventEnum, InGameCycle> _stateMachine;

    Player _player;
    Enemy _enemy;
    bool[] _isClearedStage;

    [SerializeField, Tooltip("デバッグ用")]
    GameObject _resultPanel = null;

    public enum EventEnum
    {
        GameStart,
        NormalCharge,
        SpecialCharge,
        Attack,
        Idle,
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

        _stateMachine.AddTransition<IdleState, NormalAttackChargeState>(EventEnum.NormalCharge);
        _stateMachine.AddTransition<IdleState, SpecialAttackChargeState>(EventEnum.SpecialCharge);

        _stateMachine.AddTransition<NormalAttackChargeState, NormalAttackState>(EventEnum.Attack);
        _stateMachine.AddTransition<SpecialAttackChargeState, SpecialAttackState>(EventEnum.Attack);

        _stateMachine.AddTransition<NormalAttackChargeState, IdleState>(EventEnum.Idle);

        _stateMachine.AddTransition<NormalAttackState, IdleState>(EventEnum.Idle);
        _stateMachine.AddTransition<SpecialAttackState, IdleState>(EventEnum.Idle);

        _stateMachine.AddAnyTransitionTo<ResultState>(EventEnum.GameOver);

        //最初のStateを設定
        _stateMachine.StartSetUp<StartState>();
        _player = new Player(ChangeState);
        _enemy = new Enemy(ChangeState, _player);

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
        protected override async void OnEnter(State prevState)
        {
            Debug.Log("Idleステートに入った");
            await UniTask.Delay(TimeSpan.FromSeconds(2f));
            _stateMachine.Dispatch(EventEnum.NormalCharge);
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

    private class NormalAttackChargeState : State
    {
        protected override async void OnEnter(State prevState)
        {
            Debug.Log("チャージ中");
            bool isTrigger = await _stateMachine.Owner._enemy.Charge();
            Debug.Log("チャージ終わり");
            if (isTrigger)
            {
                await _stateMachine.Owner._enemy.Damage();
                _stateMachine.Dispatch(EventEnum.Idle);
            }
            else
            {
                _stateMachine.Dispatch(EventEnum.Attack);
            }         
        }

        protected override void OnUpdate()
        {
            _stateMachine.Owner._player.OnUpdate();
        }
        protected override void OnExit(State nextState)
        {
            Debug.Log("NormalAttackChargeステートを抜けた");
        }
    }

    private class NormalAttackState : State
    {
        protected override async void OnEnter(State prevState)
        {
            Debug.Log("NormalAttackステートに入った");
            await _stateMachine.Owner._enemy.Attack(_stateMachine.Owner._player);
            _stateMachine.Dispatch(EventEnum.Idle);
        }

        protected override void OnUpdate()
        {
        }
        protected override void OnExit(State nextState)
        {

        }
    }

    private class SpecialAttackChargeState : State
    {
        protected override void OnEnter(State prevState)
        {
            Debug.Log("SpecialAttackChargeステートに入った");
        }
        protected override void OnExit(State nextState)
        {
            Debug.Log("SpecialAttackChargeステートを抜けた");
        }
    }

    private class SpecialAttackState : State
    {
        protected override void OnEnter(State prevState)
        {
            Debug.Log("SpecialAttackステートに入った");
        }
        protected override void OnExit(State nextState)
        {
            Debug.Log("SpecialAttackステートを抜けた");
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
    public void GameOver()
    {
        _stateMachine.Dispatch(EventEnum.GameOver);
    }
    public void Pause()
    {
        _stateMachine.Dispatch(EventEnum.Pause);
    }
}
