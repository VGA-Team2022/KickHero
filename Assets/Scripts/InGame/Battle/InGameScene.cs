using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;

public class InGameScene : AbstructScene
{
    protected override void OnAwake()
    {
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

