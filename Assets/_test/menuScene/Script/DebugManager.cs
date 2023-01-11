using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugManager : MonoBehaviour
{
    [SerializeField] private bool[] _isCleared;
    [SerializeField] private Button[] _stages;

    private StageManager _stageManager;

    // Start is called before the first frame update
    void Awake()
    {
        _stageManager = new StageManager();
        _stageManager.OpenStage(_isCleared, _stages);
    }
}
