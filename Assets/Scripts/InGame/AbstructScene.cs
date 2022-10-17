using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstructScene : MonoBehaviour
{
    protected ApplicationOperator _applicationOperator = null;
    public void SetOperator(ApplicationOperator appOperator)
    {
        _applicationOperator = appOperator;
    }
    public abstract void Load();
    public abstract void Open();
    public abstract void UnLoad();
}
