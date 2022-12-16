using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeakPoint : MonoBehaviour
{
    public bool IsTrigger { get; set; }
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out BallPresenter ballPresenter))
        {
            IsTrigger = true;
        }
    }
}
