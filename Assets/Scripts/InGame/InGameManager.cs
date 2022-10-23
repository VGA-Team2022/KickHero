using Cysharp.Threading.Tasks;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameManager : AbstructScene
{
    public override async UniTask Load(object[] objects)
    {
        var token = this.GetCancellationTokenOnDestroy();
        await UniTask.DelayFrame(1000, cancellationToken: token);
        Debug.Log("Load��"+objects);
    }

    public override void Open()
    {
        
    }

    public override async UniTask UnLoad()
    {
        await UniTask.Yield();
        Debug.Log("�A�����[�h");
    }

    public void LoadScene(string sceneName)
    {

    }
}
