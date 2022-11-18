using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using Zenject;

public class InGameScene : AbstructScene
{
    [SerializeField]
    bool[] _startClearedStages;

    [Header("�f�o�b�O�p")]
    [SerializeField,]
    Button _button;
    [SerializeField]
    string _sceneName = "";

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

        _button.onClick.AddListener(() => { LoadScene(_sceneName,_sceneOperator.IsClearedStages); });
    }
    public override async UniTask Load()
    {
        await UniTask.Yield();
    }

    /// <summary>
    /// �V�[�������[�h���ꂽ��ɌĂ΂��
    /// </summary>
    public override void Open()
    {

    }

    public override async UniTask UnLoad()
    {
        await UniTask.Yield();
    }
}

