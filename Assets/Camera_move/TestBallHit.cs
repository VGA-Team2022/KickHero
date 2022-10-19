using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBallHit : MonoBehaviour
{
    [SerializeField]
    GameObject _prefab;
    [SerializeField]
    float _hitTime = 1f;
    [SerializeField, RangeAttribute(0,1)]
    float _timeScale = 0.2f;
    [SerializeField]
    LayerMask _layerMask;
    private void OnCollisionEnter(Collision collision)
    {
        if (((1 << collision.gameObject.layer) & _layerMask) != 0)
        {
            Instantiate(_prefab, transform.position, Quaternion.identity);
            StartCoroutine(OnAttackHit());
        }
    }

    IEnumerator OnAttackHit()
    {
        // モーションを止める
        Time.timeScale = _timeScale;
        yield return new WaitForSecondsRealtime(_hitTime);
        Time.timeScale = 1f;
    }
}
