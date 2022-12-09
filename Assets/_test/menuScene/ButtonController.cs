using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : ButtonBase
{
    [SerializeField] private string _str;

    public override void Process()
    {
        base.Process();
        Debug.Log(_str);
    }
}
