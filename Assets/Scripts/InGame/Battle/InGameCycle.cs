using State = StateMachine<InGameCycle.EventEnum, InGameCycle>.State;
using UnityEngine;
using System;
using Cysharp.Threading.Tasks;
using System.Threading;

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
        _player = new Player();
        _enemy = new Enemy( _player);
        _player.InitUltimate(_enemy);

        _resultPanel = GameObject.Find("ResultCanvas");
        _resultPanel?.SetActive(false);       
    }

    private void Start()
    {
        
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

    public void SetCurrentStage(int index)
    {
        throw new NotImplementedException();
    }

    private class StartState : State
    {
        protected override async void OnEnter(State prevState)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(3f));
            _stateMachine.Owner._enemy.Threat();
            SoundManagerPresenter.Instance.CriAtomBGMPlay("BGM_Battle");
            SoundManagerPresenter.Instance.CriAtomVoicePlay("Voice_Start");
            _stateMachine.Dispatch(EventEnum.GameStart);
            Debug.Log("�X�^�[�g�X�e�[�g�ɓ�����");
        }
        protected override void OnUpdate()
        {
            
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
            if (_stateMachine.Owner._player.IsUltimate)
            {
                return;
            }
            _stateMachine.Owner._player.OnUpdate();
            if (_stateMachine.Owner._enemy.IsDead)
            {
                _stateMachine.Dispatch(EventEnum.GameOver);
            }
        }
        protected override void OnExit(State nextState)
        {
            Debug.Log("Idle�X�e�[�g�𔲂���");
        }
    }

    private class NormalAttackChargeState : State
    {
        protected override void OnEnter(State prevState)
        {
            Debug.Log("NormalAttackChargeState�ɓ�����");
            _stateMachine.Owner._enemy.Charge();        
        }

        protected override async void OnUpdate()
        {
            if(_stateMachine.Owner._player.IsUltimate)
            {
                return;
            }
            if (_stateMachine.Owner._enemy.IsDead)
            {
                _stateMachine.Dispatch(EventEnum.GameOver);
            }
            else if(_stateMachine.Owner._enemy.IsTriggerWeakPoint())
            {
                SoundManagerPresenter.Instance.CriAtomSEPlay("SE_Hit");
                await _stateMachine.Owner._enemy.Damage();
                _stateMachine.Owner._player.AddUltimateGauge(InGameConst.ALTIMATE_GAUGE_POINT);
                _stateMachine.Dispatch(EventEnum.Idle);
            }
            else if(_stateMachine.Owner._enemy.IsChargeTimeUp())
            {
                _stateMachine.Dispatch(EventEnum.Attack);
            }
            else
            {
                _stateMachine.Owner._player.OnUpdate();
            }
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
            if (_stateMachine.Owner._player.IsDead)
            {
                _stateMachine.Dispatch(EventEnum.GameOver);
            }
            else
            {
                _stateMachine.Dispatch(EventEnum.Idle);
            }
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
        protected override void OnUpdate()
        {
            if (_stateMachine.Owner._enemy.IsDead)
            {
                _stateMachine.Dispatch(EventEnum.GameOver);
            }
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
            if (_stateMachine.Owner._player.IsDead)
            {
                _stateMachine.Dispatch(EventEnum.GameOver);
            }
            else
            {
                _stateMachine.Dispatch(EventEnum.Idle);
            }
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
