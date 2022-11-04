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
        //�C���X�^���X��������
        _stateMachine = new StateMachine<EventEnum,SampleInGameCycle>(this);
        
        //�J�ڂ��`
        _stateMachine.AddTransition<StartState, InGameState>(EventEnum.GameStart);
        _stateMachine.AddTransition<InGameState, ResultState>(EventEnum.GameOver);
        _stateMachine.AddTransition<ResultState, StartState>(EventEnum.Retry);

        //�ŏ���State��ݒ�
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
            Debug.Log("�X�^�[�g�X�e�[�g�ɓ�����");
        }
        protected override void OnUpdate()
        {
            Debug.Log("�X�^�[�g�X�e�[�g���s��");
        }
        protected override void OnExit(State nextState)
        {
            Debug.Log("�X�^�[�g�X�e�[�g�𔲂���");
        }
    }
    private class InGameState : State
    {
        protected override void OnEnter(State prevState)
        {
            Debug.Log("�C���Q�[���X�e�[�g�ɓ�����");

        }
        protected override void OnUpdate()
        {
            Debug.Log("�C���Q�[�����s��");
        }
        protected override void OnExit(State nextState)
        {
            Debug.Log("�C���Q�[���X�e�[�g�𔲂���");
        }
    }

    private class ResultState : State
    {
        protected override void OnEnter(State prevState)
        {
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
}
