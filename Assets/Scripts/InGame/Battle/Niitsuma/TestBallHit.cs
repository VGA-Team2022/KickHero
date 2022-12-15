using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    public void TestSceneChange(string name)
    {
        SceneManager.LoadScene(name);
    }

    IEnumerator OnAttackHit()
    {
        // ƒ‚[ƒVƒ‡ƒ“‚ðŽ~‚ß‚é
        Time.timeScale = _timeScale;
        yield return new WaitForSecondsRealtime(_hitTime);
        Time.timeScale = 1f;
    }
}
