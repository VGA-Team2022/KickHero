using UnityEngine;

public class Player : ISequence
{
    [Header("コンポーネント")]
    [SerializeField]
    PlayerHP _playerHP;
    [SerializeField]
    UltimatePresenter _ultimatePresenter;
    [SerializeField]
    BallPresenter _ballPresenter;
    [SerializeField]
    LineReader _lineReader;

    public Player(System.Action<InGameCycle.EventEnum> action)
    {
        Initialize(action);
    }

    public void Initialize(System.Action<InGameCycle.EventEnum> action)
    {
        if (!_playerHP)
        {
            _playerHP = GetMonoBehaviorInstansInScene<PlayerHP>();
        }
        if (!_ultimatePresenter)
        {
            _ultimatePresenter = GetMonoBehaviorInstansInScene<UltimatePresenter>();
        }
        if (!_ballPresenter)
        {
            _ballPresenter = GetMonoBehaviorInstansInScene<BallPresenter>();
        }
        if (!_lineReader)
        {
            _lineReader = GetMonoBehaviorInstansInScene<LineReader>();
        }
        _playerHP.Init();
        _ultimatePresenter.Init();
        _ballPresenter.Init(action);
        _lineReader.Init();
    }

    public void OnUpdate()
    {
        _lineReader.OnUpdate(_ballPresenter);
    }

    public void Damage(int value)
    {
        _playerHP.Damage(value);
    }
    private T GetMonoBehaviorInstansInScene<T>() where T : MonoBehaviour
    {
        T instans = GameObject.FindObjectOfType<T>();
        if (instans)
        {
            return instans;
        }
        else
        {
            GameObject go = new GameObject(nameof(T));
            return go.AddComponent<T>();
        }
    }
}
