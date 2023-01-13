using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class UltimatePresenter : MonoBehaviour
{
    [SerializeField,Tooltip("ビュークラスのインスタンス")]
    UltimateView _ultimateView = null;

    /// <summary>モデルのインスタンス</summary>
    UltimateModel _ultimateModel = null;
    [SerializeField,Tooltip("アルティメットの最大値")]
    int _maxUltimateValue = 10;
    [SerializeField, Tooltip("アルティメットの攻撃力")]
    int _ultimateDamage = 10;
    bool _isUltimate = false;
    public bool IsUltimate => _isUltimate;

    public bool IsClearTimeline { get => _isClearTimeline; set => _isClearTimeline = value; }

    IDamage _enemy;

    public void Init(IDamage enemy)
    {
        _enemy = enemy;
        //インスタンス生成、引数は(最大値,Action<int>,GameObject)
        _ultimateModel = new UltimateModel(
            _maxUltimateValue,
            x =>
            {
                _ultimateView.ChangeGaugeValue(_maxUltimateValue,x);
            },
            _ultimateView.gameObject);

        _ultimateView.Init(() => {
            TimeLineController.Instance.EventPlay(TimeLineState.Ult);
            SoundManagerPresenter.Instance.CriAtomVoicePlay("Voice_Push_001");
            _isUltimate=true;
        });
    }

    public void StopUltimate()
    {
        _enemy.Damage(_ultimateDamage);
        _isUltimate = false;
    }

    /// <summary>
    /// 値を外部から変更する関数
    /// </summary>
    public void ChangeValue(int value)
    {
        _ultimateModel.ChangeUltimateValue(value);
    }

    bool _isClearTimeline = false;


    public void EndClearTimeLine()
    {
        IsClearTimeline = false;
    }
}
