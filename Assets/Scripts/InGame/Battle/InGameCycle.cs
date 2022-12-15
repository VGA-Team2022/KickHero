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

    [SerializeField,Tooltip("�f�o�b�O�p")]
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
        //�C���X�^���X��������
        _stateMachine = new StateMachine<EventEnum, InGameCycle>(this);

        //�J�ڂ��`
        _stateMachine.AddTransition<StartState, ReceptionInputState>(EventEnum.GameStart);
        _stateMachine.AddTransition<ReceptionInputState, ThrowState>(EventEnum.Throw);
        _stateMachine.AddTransition<ThrowState, ReceptionInputState>(EventEnum.BallRespawn);
        _stateMachine.AddAnyTransitionTo<ResultState>(EventEnum.GameOver);

        //�ŏ���State��ݒ�
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
            Debug.Log("�X�^�[�g�X�e�[�g�ɓ�����");
        }
        protected override void OnUpdate()
        {
            _stateMachine.Dispatch(EventEnum.GameStart);
        }
        protected override void OnExit(State nextState)
        {
            Debug.Log("�X�^�[�g�X�e�[�g�𔲂���");
        }
    }

    private class ReceptionInputState : State
    {
        protected override void OnEnter(State prevState)
        {          
            Debug.Log("���͎�t�X�e�[�g�ɓ�����");
        }
        protected override void OnUpdate()
        {
            _stateMachine.Owner._player.OnUpdate();
            _stateMachine.Owner._enemy.OnUpdate();
        }
        protected override void OnExit(State nextState)
        {
            Debug.Log("���͎�t�X�e�[�g�𔲂���");
        }
    }

    private class ThrowState : State
    {
        protected override void OnEnter(State prevState)
        {
            Debug.Log("Throw�X�e�[�g�ɓ�����");
        }
        protected override void OnExit(State nextState)
        {
            Debug.Log("Throw�X�e�[�g�𔲂���");
        }
    }

    private class ResultState : State
    {
        protected override void OnEnter(State prevState)
        {
            _stateMachine.Owner._resultPanel?.SetActive(true);
            Debug.Log("���U���g�X�e�[�g�ɓ�����");
        }
        protected override void OnUpdate()
        {
            Debug.Log("���U���g�X�e�[�g���s��");
        }
        protected override void OnExit(State nextState)
        {
            Debug.Log("���U���g�X�e�[�g�𔲂���");
        }
    }

    //�f�o�b�O�p�̊֐�

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
