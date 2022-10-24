using Cysharp.Threading.Tasks;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class InGameManager : AbstructScene
{
    public override async UniTask Load(object[] objects)
    {
        foreach (var obj in objects)
        {
            Debug.Log($"LoadíÜ{obj}");
        }
        
        await UniTask.DelayFrame(1000);        
    }

    public override void Open()
    {
        Debug.Log("Open");
    }

    public override async UniTask UnLoad()
    {
        Debug.Log($"ÉAÉìÉçÅ[Éh{UnityEngine.SceneManagement.SceneManager.GetActiveScene().name}");
        await UniTask.Yield();        
    }

    public void LoadScene(string sceneName)
    {

    }
}
