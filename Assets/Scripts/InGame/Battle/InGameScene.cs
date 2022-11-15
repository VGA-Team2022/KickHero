using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using Zenject;

public class InGameScene : AbstructScene
{
    [SerializeField]
    bool[] _startClearedStages;
    IReceivableGameData _receivableGameData;
    [Inject]
    void Construct(IReceivableGameData receivableGameData)
    {
        _receivableGameData= receivableGameData;
    }
    protected override void OnAwake()
    {
        if (_sceneOperator ==null)
        {
            _sceneOperator = new SceneOperator(_startClearedStages);
        }
        _receivableGameData.SetClearedStage(_sceneOperator.IsClearedStages);
    }
    public override async UniTask Load()
    {
        await UniTask.Yield();
    }

    /// <summary>
    /// シーンがロードされた後に呼ばれる
    /// </summary>
    public override void Open()
    {

    }

    public override async UniTask UnLoad()
    {
        await UniTask.Yield();
    }
}

