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
        //インスタンスを初期化
        _stateMachine = new StateMachine<EventEnum, InGameCycle>(this);

        //遷移を定義
        _stateMachine.AddTransition<StartState, InGameState>(EventEnum.GameStart);
        _stateMachine.AddTransition<InGameState, ThrowState>(EventEnum.Throw);
        _stateMachine.AddTransition<ThrowState, AttackState>(EventEnum.Attack);
        _stateMachine.AddTransition<AttackState, EnemyAttackState>(EventEnum.EnemyAttack);
        _stateMachine.AddTransition<AttackState, ThrowState>(EventEnum.BallRespawn);
        _stateMachine.AddTransition<InGameState, ResultState>(EventEnum.GameOver);
        _stateMachine.AddTransition<InGameState, ResultState>(EventEnum.Pause);
        _stateMachine.AddTransition<ResultState, StartState>(EventEnum.Retry);

        //最初のStateを設定
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
            //ボールの位置座標とベクトル、回転を初期化
            Vector3 tmp = GameObject.Find("Ball").transform.position;
            GameObject.Find("Ball").transform.position = new Vector3(0, 0, 0);
            Rigidbody rigidbody = GameObject.Find("Ball").GetComponent<Rigidbody>();
            rigidbody.velocity = Vector3.zero;
            rigidbody.angularVelocity = Vector3.zero;
            rigidbody.ResetInertiaTensor();
            Debug.Log("インゲームステートに入った");
            //スタート演出の処理

            //スタート演出が終了後ThrowStateに移動
            Instance.Throw();
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

    private class ThrowState : State
    {
        protected override void OnEnter(State prevState)
        {
            //ボールの位置座標とベクトル、回転をリセット
            Vector3 tmp = GameObject.Find("Ball").transform.position;
            GameObject.Find("Ball").transform.position = new Vector3(0, 0, 0);
            Rigidbody rigidbody = GameObject.Find("Ball").GetComponent<Rigidbody>();
            rigidbody.velocity = Vector3.zero;
            rigidbody.angularVelocity = Vector3.zero;
            rigidbody.ResetInertiaTensor();
            Debug.Log("ボールスローステートに入った");
        }
        protected override void OnUpdate()
        {
            //ボールが敵に当たるとAttackStateに移動
            void OnCollisionEnter(Collision collision)
            {
                Instance.Attack();
                Debug.Log("敵にヒット！AttackStateに移動");
            }
            //ボールを飛ばして当たらなかったらターンを+1してボールの位置座標とベクトル、回転をリセット
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

            Debug.Log("ボールスローステート実行中");
        }
        protected override void OnExit(State nextState)
        {
            Debug.Log("ボールスローステートを抜けた");
        }
    }
    private class AttackState : State
    {
        protected override void OnEnter(State prevState)
        {
            Debug.Log("アタックステートに入った");
        }
        protected override void OnUpdate()
        {
            /*if 当たり判定
        ｛
	        Yse(敵は攻撃中か)

            No(BallRespawn()を呼ぶ)
         ｝
        ：敵は攻撃中か
        ｛  
	        Yse（ターゲットマーカーに当たったか？）
	        No（攻撃HIT演出＆敵の体力処理）
        ｝
        ：敵の体力は０か？
        ｛
	        Yse（敵を倒した演出＆ステージクリア演出後ResultStateに移動）
	        No(BallRespawn()を呼ぶ)
        ｝
        ：ターゲットマーカーに当たったか？
        ｛
	        Yse（通常攻撃準備だったか？）
	        No（BallRespawn()を呼ぶ）
        ｝
        ：通常攻撃準備だったか？
        ｛  
	        Yse（攻撃を阻止できたのか？）
	        No（BallRespawn()を呼ぶ）
        ｝
        ：攻撃を阻止できたのか？
        ｛
	        Yse（ガード成功演出＆必殺技ゲージ増加処理後にBallRespawn()を呼ぶ）
	        No（EnemyAttackStateに移動）
        ｝
        ：体力判定
        ｛
	        Yse（敗北演出＆敗北画面表示後にResultStateに移動）
	        No（BallRespawn()を呼ぶ）
        ｝*/
            Debug.Log("アタックステート実行中");
        }
        protected override void OnExit(State nextState)
        {
            Debug.Log("アタックステートを抜けた");
        }
    }

    private class EnemyAttackState : State
    {
        protected override void OnEnter(State prevState)
        {

            Debug.Log("エネミーアタックステートに入った");
        }
        protected override void OnUpdate()
        {
            //通常攻撃か大技かの判定(大技は今回ないみたいなので削ります)

            //通常攻撃の場合

            //プレイヤーのダメージ演出＆ダメージ処理＆体力判定

            //もしプレイヤーのHPが0の場合（敗北演出後敗北ポップアップを表示）

            //ResultStateに移動
            Instance.GameOver();
            Debug.Log("エネミーアタックステート実行中");
        }
        protected override void OnExit(State nextState)
        {
            Debug.Log("エネミーアタックステートを抜けた");
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
