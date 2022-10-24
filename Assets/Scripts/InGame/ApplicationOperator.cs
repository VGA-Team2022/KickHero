using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

public class ApplicationOperator
{
    SceneOperator _sceneOperator;
    public SceneOperator GetSceneOperator => _sceneOperator;

    public async UniTask SetUp()
    {
        var objs = new object[] { "Hello World" };
        _sceneOperator = new SceneOperator();
        _sceneOperator.SetUp(this, objs);
        Debug.Log($"現在のシーン{SceneManager.GetActiveScene().name}");
        await _sceneOperator.LoadScene("Menu");
        Debug.Log($"変更後のシーン{SceneManager.GetActiveScene().name}");
    }
}
