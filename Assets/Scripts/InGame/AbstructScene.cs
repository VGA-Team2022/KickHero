using UnityEngine;
using System.Threading;
using Cysharp.Threading.Tasks;

public abstract class AbstructScene : MonoBehaviour
{
    protected ApplicationOperator _applicationOperator = null;
    public void SetOperator(ApplicationOperator appOperator)
    {
        _applicationOperator = appOperator;
    }
    public abstract UniTask Load(object[] objects);
    public abstract void Open();
    public abstract UniTask UnLoad();
}
