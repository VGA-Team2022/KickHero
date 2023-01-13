using Cysharp.Threading.Tasks;
using UnityEngine.UI;
using UnityEngine;

public class MenuScene : AbstructScene
{
    [SerializeField]
    bool[] _startClearedStages;

    protected override void OnAwake()
    {
        if (_sceneOperator == null)
        {
            _sceneOperator = new SceneOperator(_startClearedStages);
        }
        SoundManagerPresenter.Instance.CriAtomBGMPlay("BGM_Stage");
    }
    public override async UniTask Load()
    {
        await UniTask.Yield();
    }

    public override void Open()
    {
        Debug.Log($"�z��{ClearedStages.Length}");
        foreach (var i in ClearedStages)
        {
            Debug.Log(i);
        }
    }

    public async UniTask LoadScene(string sceneName)
    {
        //�ŏ��Ɉ�x�C���X�^���X��������
        if (_sceneOperator == null)
        {
            _sceneOperator = new SceneOperator(_startClearedStages);
        }
        await _sceneOperator.LoadScene(sceneName);
    }
    public override async UniTask UnLoad()
    {
        await UniTask.Yield();
    }
}
