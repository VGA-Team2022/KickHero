using Cysharp.Threading.Tasks;
using UnityEngine.UI;
using UnityEngine;

public class MenuScene : AbstructScene
{
    [SerializeField]
    bool[] _startClearedStages;
    [Header("デバッグ用")]
    [SerializeField,]
    Button _button;
    [SerializeField]
    string _sceneName = "";

    protected override void OnAwake()
    {
        if (_sceneOperator == null)
        {
            _sceneOperator = new SceneOperator(_startClearedStages);
        }
        _button.onClick.AddListener(async () => { await LoadScene(_sceneName, _sceneOperator.IsClearedStages); });
    }
    public override async UniTask Load()
    {
        await UniTask.Yield();
    }

    public override void Open()
    {
        Debug.Log($"配列長{ClearedStages.Length}");
        foreach (var i in ClearedStages)
        {
            Debug.Log(i);
        }
    }

    public override async UniTask UnLoad()
    {
        await UniTask.Yield();
    }
}
