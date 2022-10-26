using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class ApplicationOperator
{
    SceneOperator _sceneOperator;
    public SceneOperator GetSceneOperator => _sceneOperator;

    public ApplicationOperator()
    {
        var objs = new object[] { "Hello World" };
        _sceneOperator = new SceneOperator();
        _sceneOperator.SetUp(this, objs);
    }

    public async UniTask LoadScene(string sceneName)
    {
        await _sceneOperator.LoadScene(sceneName);
    }
}
