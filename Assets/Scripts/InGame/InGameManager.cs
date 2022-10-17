using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameManager : AbstructScene
{
    public override async UniTask Load(object[] objects)
    {
        await UniTask.Yield();
        Debug.Log("LoadíÜ"+objects);
    }

    public override void Open()
    {
        
    }

    public override async UniTask UnLoad()
    {
        await UniTask.Yield();
        Debug.Log("ÉAÉìÉçÅ[Éh");
    }

    public void LoadScene(string sceneName)
    {

    }
}
