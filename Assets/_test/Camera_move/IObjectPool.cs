using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IObjectPool
{
    bool IsActive { get; }
    void InactiveInstantiate();
    void Create();
    void Destroy();
}
