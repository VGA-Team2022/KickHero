using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TestCutScene : MonoBehaviour
{
    [SerializeField]
    float _time = 1f;
    [SerializeField]
    PlayableDirector _director;

    FadeSystem fadeSystem;
    private void Awake()
    {
        fadeSystem = GetComponent<FadeSystem>();
    }
    public void CutScene()
    {
        StartCoroutine(fadeSystem.StartFadeOut(_time, () => _director.Play()));
    }
}
