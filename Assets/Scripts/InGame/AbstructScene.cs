using UnityEngine;
using System.Threading;
using Cysharp.Threading.Tasks;

public abstract class AbstructScene : MonoBehaviour
{
    protected SceneOperator _sceneOperator;

    private void Awake()
    {
        OnAwake();
    }
    protected virtual void OnAwake() { }

    public async void LoadScene(string sceneName,string message)
    {
        //ç≈èâÇÃÇ›èâä˙âª
        if (_sceneOperator == null)
        {
            _sceneOperator = new SceneOperator(message);
        }
        await _sceneOperator.LoadScene(sceneName);
    }
    public void SetOperator(SceneOperator appOperator)
    {
        _sceneOperator = appOperator;
    }
    public abstract UniTask Load(string message);
    public abstract void Open();
    public abstract UniTask UnLoad();
}
