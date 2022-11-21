using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHpTextAnimation : MonoBehaviour
{
    [SerializeField] private Text Text= default;

    private void Start()
    {
        Text.DOCounter(0, 1000, 3f)
        .SetEase(Ease.InOutCubic)
        //.SetLink(gameObject)
        .Play();
    }
}
