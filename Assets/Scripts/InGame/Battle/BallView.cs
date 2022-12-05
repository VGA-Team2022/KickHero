using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

[RequireComponent(typeof(SphereCollider))]
public class BallView : MonoBehaviour
{
    Action<Collider> _onHitActionCollider;
    Action<RaycastHit> _onHitActionRaycastHit;

    SphereCollider _collider;
    List<Collider> _stayColliders = new List<Collider>();
    bool _isCollision = false;


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
            if (_isCollision)
            {
                HitDetermine(pos, value, Collider.radius);
            }
        }
    }


    /// <summary>�����蔻�����邩�ۂ�</summary>
    public bool IsCollision
    {
        get => _isCollision;
        set
        {
            _isCollision = value;
            _stayColliders.Clear();
        }
    }

    public void OnHit(Action<Collider> action)
    {
        _onHitActionCollider += action;
    }
    public void OnHit(Action<RaycastHit> action)
    {
        _onHitActionRaycastHit += action;
    }

    void HitDetermine(Vector3 start, Vector3 end, float radius)
    {
        //var hits = Physics.OverlapCapsule(start, end, radius, Physics.AllLayers, QueryTriggerInteraction.Collide);
        //if (hits.Length > 0)
        //{
        //    foreach (Collider c in hits)
        //    {
        //        if (!Physics.GetIgnoreLayerCollision(Collider.gameObject.layer, c.gameObject.layer))
        //        {
        //            if (!_stayColliders.Contains(c))
        //            {
        //                CallOnHit(c);
        //                _stayColliders.Add(c);
        //            }
        //            for (int i = 0; i < _stayColliders.Count; i++)
        //            {
        //                if ((hits.Where(p => p == _stayColliders[i]).Count() == 0))
        //                {
        //                    _stayColliders.RemoveAt(i);
        //                    i--;
        //                }
        //            }
        //        }
        //        else
        //        {
        //            _stayColliders.Remove(c);
        //        }
        //    }
        //}
        //else
        //{
        //    _stayColliders.Clear();
        //}

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
                        if ((pHits.Where(p => p.collider == _stayColliders[i]).Count() == 0))
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
