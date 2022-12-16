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

    [SerializeField, Tooltip("�f�o�b�O�p")]
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
        //�C���X�^���X��������
        _stateMachine = new StateMachine<EventEnum, InGameCycle>(this);

        //�J�ڂ��`
        _stateMachine.AddTransition<StartState, IdleState>(EventEnum.GameStart);

        _stateMachine.AddTransition<IdleState, NormalAttackChargeState>(EventEnum.NormalCharge);
        _stateMachine.AddTransition<IdleState, SpecialAttackChargeState>(EventEnum.SpecialCharge);

        _stateMachine.AddTransition<NormalAttackChargeState, NormalAttackState>(EventEnum.Attack);
        _stateMachine.AddTransition<SpecialAttackChargeState, SpecialAttackState>(EventEnum.Attack);

        _stateMachine.AddTransition<NormalAttackChargeState, IdleState>(EventEnum.Idle);

        _stateMachine.AddTransition<NormalAttackState, IdleState>(EventEnum.Idle);
        _stateMachine.AddTransition<SpecialAttackState, IdleState>(EventEnum.Idle);

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

    private class IdleState : State
    {
        protected override async void OnEnter(State prevState)
        {
            Debug.Log("Idle�X�e�[�g�ɓ�����");
            await UniTask.Delay(TimeSpan.FromSeconds(2f));
            _stateMachine.Dispatch(EventEnum.NormalCharge);
        }
        protected override void OnUpdate()
        {
            _stateMachine.Owner._player.OnUpdate();
        }
        protected override void OnExit(State nextState)
        {
            Debug.Log("Idle�X�e�[�g�𔲂���");
        }
    }

    private class NormalAttackChargeState : State
    {
        protected override async void OnEnter(State prevState)
        {
            Debug.Log("�`���[�W��");
            bool isTrigger = await _stateMachine.Owner._enemy.Charge();
            Debug.Log("�`���[�W�I���");
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
            Debug.Log("NormalAttackCharge�X�e�[�g�𔲂���");
        }
    }

    private class NormalAttackState : State
    {
        protected override async void OnEnter(State prevState)
        {
            Debug.Log("NormalAttack�X�e�[�g�ɓ�����");
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
            Debug.Log("SpecialAttackCharge�X�e�[�g�ɓ�����");
        }
        protected override void OnExit(State nextState)
        {
            Debug.Log("SpecialAttackCharge�X�e�[�g�𔲂���");
        }
    }

    private class SpecialAttackState : State
    {
        protected override void OnEnter(State prevState)
        {
            Debug.Log("SpecialAttack�X�e�[�g�ɓ�����");
        }
        protected override void OnExit(State nextState)
        {
            Debug.Log("SpecialAttack�X�e�[�g�𔲂���");
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
    public void GameOver()
    {
        _stateMachine.Dispatch(EventEnum.GameOver);
    }
    public void Pause()
    {
        _stateMachine.Dispatch(EventEnum.Pause);
    }
}
