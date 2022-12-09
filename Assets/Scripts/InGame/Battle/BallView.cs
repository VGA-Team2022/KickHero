using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;


/// <summary>
/// ボールの表面的な処理を行うクラス
/// </summary>
[RequireComponent(typeof(SphereCollider), typeof(Rigidbody))]
public class BallView : MonoBehaviour
{
    /// <summary>何かに当たった時に発行するイベント</summary>
    Action<Collider> _onHitActionCollider;
    /// <summary>何かに当たった時にそのRaycastHitを渡すイベント</summary>
    Action<RaycastHit> _onHitActionRaycastHit;

    SphereCollider _collider;
    Rigidbody _rb;
    /// <summary>接触中のコライダーのリスト</summary>
    List<Collider> _stayColliders = new List<Collider>();
    bool _isCollide = false;

    public Rigidbody Rigidbody
    {
        get
        {
            if (!_rb)
            {
                _rb = GetComponent<Rigidbody>();
                if (!_rb)
                {
                    Debug.LogError($"{nameof(Rigidbody)}が見つかりません");
                }
            }

            return _rb;
        }
    }


    public SphereCollider Collider
    {
        get
        {
            if (_collider == null)
            {
                _collider = GetComponent<SphereCollider>();
            }
            return _collider;
        }
    }

    public Vector3 Position
    {
        get => transform.position;
        set
        {
            Vector3 pos = transform.position;
            transform.position = value;
            if (_isCollide)
            {
                HitDetermine(pos, value, Collider.radius);
            }
        }
    }


    /// <summary>当たり判定を取るか否か</summary>
    public bool IsCollide
    {
        get => _isCollide;
        set
        {
            _isCollide = value;
            _stayColliders.Clear();
        }
    }

    private void Start()
    {
        Rigidbody.isKinematic = true;
        _collider = GetComponent<SphereCollider>();
    }

    public void OnHit(Action<Collider> action)
    {
        _onHitActionCollider += action;
    }
    public void OnHit(Action<RaycastHit> action)
    {
        _onHitActionRaycastHit += action;
    }

    /// <summary>
    /// 二点間を移動した時の当たり判定を取る
    /// 何かに接触した時、Colliderを渡すイベントと、
    /// Collision代わりのRaycastHitを渡すイベントを発行する
    /// </summary>
    /// <param name="start"></param>
    /// <param name="end"></param>
    /// <param name="radius"></param>
    void HitDetermine(Vector3 start, Vector3 end, float radius)
    {
        Ray ray = new Ray(start, (end - start).normalized);
        var pHits = Physics.SphereCastAll(ray, radius, Vector3.Distance(end, start), Physics.AllLayers, QueryTriggerInteraction.Collide);
        if (pHits.Length != 0)
        {
            foreach (RaycastHit co in pHits)
            {
                if (!Physics.GetIgnoreLayerCollision(Collider.gameObject.layer, co.collider.gameObject.layer))
                {
                    if (!_stayColliders.Contains(co.collider))
                    {
                        Debug.Log(co.collider.name);
                        CallOnHit(co.collider);
                        CallOnHit(co);
                        _stayColliders.Add(co.collider);
                    }
                    for (int i = 0; i < _stayColliders.Count; i++)
                    {
                        if (pHits.Where(p => p.collider == _stayColliders[i]).Count() == 0)
                        {
                            _stayColliders.RemoveAt(i);
                            i--;
                        }
                    }
                }
                else
                {
                    _stayColliders.Remove(co.collider);
                }
            }
        }
    }

    void CallOnHit(Collider c)
    {
        _onHitActionCollider?.Invoke(c);
    }
    void CallOnHit(RaycastHit r)
    {
        _onHitActionRaycastHit?.Invoke(r);
    }

    public void Hide()
    {
        Renderer renderer = GetComponent<Renderer>();
        if (renderer)
        {
            renderer.enabled = false;
        }
    }

    public void Display()
    {
        Renderer renderer = GetComponent<Renderer>();
        if (renderer)
        {
            renderer.enabled = false;
        }
    }
}
