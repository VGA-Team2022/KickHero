using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class FlashingAnimation : MonoBehaviour
{
    [SerializeField,Tooltip("アニメーション１再生の全体時間")] private float _duration = 1f;

    private Text _text;

    // Start is called before the first frame update
    void Start()
    {
        _text = GetComponent<Text>();
        _text.DOFade(0,_duration).SetLoops(-1,LoopType.Yoyo);
    }
}
