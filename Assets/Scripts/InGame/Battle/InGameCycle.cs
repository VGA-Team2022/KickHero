using System.Collections;
using System.Collections.Generic;
using State = StateMachine<InGameCycle.EventEnum, InGameCycle>.State;
using UnityEngine;
using Unity.VisualScripting;
using Zenject;

public class InGameCycle : MonoBehaviour
{
    StateMachine<EventEnum, InGameCycle> _stateMachine;
    public static InGameCycle Instance;
    public enum EventEnum
    {
        GameStart,
        Throw,
        Attack,
        EnemyAttack,
        BallRespawn,
        GameOver,
        Pause,
        Retry
    }


    private void Awake()
    {
        //�C���X�^���X��������
        _stateMachine = new StateMachine<EventEnum, InGameCycle>(this);

        //�J�ڂ��`
        _stateMachine.AddTransition<StartState, InGameState>(EventEnum.GameStart);
        _stateMachine.AddTransition<InGameState, ThrowState>(EventEnum.Throw);
        _stateMachine.AddTransition<ThrowState, AttackState>(EventEnum.Attack);
        _stateMachine.AddTransition<AttackState, EnemyAttackState>(EventEnum.EnemyAttack);
        _stateMachine.AddTransition<AttackState, ThrowState>(EventEnum.BallRespawn);
        _stateMachine.AddTransition<InGameState, ResultState>(EventEnum.GameOver);
        _stateMachine.AddTransition<InGameState, ResultState>(EventEnum.Pause);
        _stateMachine.AddTransition<ResultState, StartState>(EventEnum.Retry);

        //�ŏ���State��ݒ�
        _stateMachine.StartSetUp<StartState>();

        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void GameStart()
    {
        _stateMachine.Dispatch(EventEnum.GameStart);
    }
    public void Throw()
    {
        _stateMachine.Dispatch(EventEnum.Throw);
    }
    public void Attack()
    {
        _stateMachine.Dispatch(EventEnum.Attack);
    }

    public void EnemyAttack()
    {
        _stateMachine.Dispatch(EventEnum.EnemyAttack);
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
            //�{�[���̈ʒu���W�ƃx�N�g���A��]��������
            Vector3 tmp = GameObject.Find("Ball").transform.position;
            GameObject.Find("Ball").transform.position = new Vector3(0, 0, 0);
            Rigidbody rigidbody = GameObject.Find("Ball").GetComponent<Rigidbody>();
            rigidbody.velocity = Vector3.zero;
            rigidbody.angularVelocity = Vector3.zero;
            rigidbody.ResetInertiaTensor();
            Debug.Log("�C���Q�[���X�e�[�g�ɓ�����");
            //�X�^�[�g���o�̏���

            //�X�^�[�g���o���I����ThrowState�Ɉړ�
            Instance.Throw();
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

    private class ThrowState : State
    {
        protected override void OnEnter(State prevState)
        {
            //�{�[���̈ʒu���W�ƃx�N�g���A��]�����Z�b�g
            Vector3 tmp = GameObject.Find("Ball").transform.position;
            GameObject.Find("Ball").transform.position = new Vector3(0, 0, 0);
            Rigidbody rigidbody = GameObject.Find("Ball").GetComponent<Rigidbody>();
            rigidbody.velocity = Vector3.zero;
            rigidbody.angularVelocity = Vector3.zero;
            rigidbody.ResetInertiaTensor();
            Debug.Log("�{�[���X���[�X�e�[�g�ɓ�����");
        }
        protected override void OnUpdate()
        {
            //�{�[�����G�ɓ������AttackState�Ɉړ�
            void OnCollisionEnter(Collision collision)
            {
                Instance.Attack();
                Debug.Log("�G�Ƀq�b�g�IAttackState�Ɉړ�");
            }
            //�{�[�����΂��ē�����Ȃ�������^�[����+1���ă{�[���̈ʒu���W�ƃx�N�g���A��]�����Z�b�g
            void OnTriggerEnter(Collider other)
            {
                //Instance.trun++;
                Vector3 tmp = GameObject.Find("Ball").transform.position;
                GameObject.Find("Ball").transform.position = new Vector3(0, 0, 0);
                Rigidbody rigidbody = GameObject.Find("Ball").GetComponent<Rigidbody>();
                rigidbody.velocity = Vector3.zero;
                rigidbody.angularVelocity = Vector3.zero;
                rigidbody.ResetInertiaTensor();
            }

            Debug.Log("�{�[���X���[�X�e�[�g���s��");
        }
        protected override void OnExit(State nextState)
        {
            Debug.Log("�{�[���X���[�X�e�[�g�𔲂���");
        }
    }
    private class AttackState : State
    {
        protected override void OnEnter(State prevState)
        {
            Debug.Log("�A�^�b�N�X�e�[�g�ɓ�����");
        }
        protected override void OnUpdate()
        {
            /*if �����蔻��
        �o
	        Yse(�G�͍U������)

            No(BallRespawn()���Ă�)
         �p
        �F�G�͍U������
        �o  
	        Yse�i�^�[�Q�b�g�}�[�J�[�ɓ����������H�j
	        No�i�U��HIT���o���G�̗̑͏����j
        �p
        �F�G�̗̑͂͂O���H
        �o
	        Yse�i�G��|�������o���X�e�[�W�N���A���o��ResultState�Ɉړ��j
	        No(BallRespawn()���Ă�)
        �p
        �F�^�[�Q�b�g�}�[�J�[�ɓ����������H
        �o
	        Yse�i�ʏ�U���������������H�j
	        No�iBallRespawn()���Ăԁj
        �p
        �F�ʏ�U���������������H
        �o  
	        Yse�i�U����j�~�ł����̂��H�j
	        No�iBallRespawn()���Ăԁj
        �p
        �F�U����j�~�ł����̂��H
        �o
	        Yse�i�K�[�h�������o���K�E�Z�Q�[�W�����������BallRespawn()���Ăԁj
	        No�iEnemyAttackState�Ɉړ��j
        �p
        �F�̗͔���
        �o
	        Yse�i�s�k���o���s�k��ʕ\�����ResultState�Ɉړ��j
	        No�iBallRespawn()���Ăԁj
        �p*/
            Debug.Log("�A�^�b�N�X�e�[�g���s��");
        }
        protected override void OnExit(State nextState)
        {
            Debug.Log("�A�^�b�N�X�e�[�g�𔲂���");
        }
    }

    private class EnemyAttackState : State
    {
        protected override void OnEnter(State prevState)
        {

            Debug.Log("�G�l�~�[�A�^�b�N�X�e�[�g�ɓ�����");
        }
        protected override void OnUpdate()
        {
            //�ʏ�U������Z���̔���(��Z�͍���Ȃ��݂����Ȃ̂ō��܂�)

            //�ʏ�U���̏ꍇ

            //�v���C���[�̃_���[�W���o���_���[�W�������̗͔���

            //�����v���C���[��HP��0�̏ꍇ�i�s�k���o��s�k�|�b�v�A�b�v��\���j

            //ResultState�Ɉړ�
            Instance.GameOver();
            Debug.Log("�G�l�~�[�A�^�b�N�X�e�[�g���s��");
        }
        protected override void OnExit(State nextState)
        {
            Debug.Log("�G�l�~�[�A�^�b�N�X�e�[�g�𔲂���");
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
