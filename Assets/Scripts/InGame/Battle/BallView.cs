using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

[RequireComponent(typeof(SphereCollider), typeof(Rigidbody))]
public class BallView : MonoBehaviour
{
    Action<Collider> _onHitAction;

    SphereCollider _collider;
    List<Collider> _stayColliders = new List<Collider>();
    bool _isCollision = false;
    Rigidbody _rb;

    List<Vector3> points = new List<Vector3>();

    private SphereCollider Collider
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

    public Rigidbody Rigidbody
    {
        get
        {
            if (!_rb)
            {
                _rb = GetComponent<Rigidbody>();
            }
            return _rb;
        }
    }

    public Vector3 Position
    {
        get => transform.position;
        set
        {
            if (_isCollision)
            {
                Vector3 scale = transform.lossyScale;
                HitDetermine(Position, value, Collider.radius * Mathf.Max(Mathf.Max(scale.x, scale.y), scale.z));
                points.Add(value);
            }
            transform.position = value;
        }
    }

    private void Update()
    {
        for (int i = 1; i < points.Count; i++)
        {
            Debug.DrawLine(points[i - 1], points[i]);
        }
    }

    /// <summary>“–‚½‚è”»’è‚ðŽæ‚é‚©”Û‚©</summary>
    public bool IsCollision { get => _isCollision; set { _isCollision = value; if (value == true) points.Clear(); } }

    public void OnHit(Action<Collider> action)
    {
        _onHitAction += action;
    }

    void HitDetermine(Vector3 start, Vector3 end, float radius)
    {
        //var hits = Physics.OverlapCapsule(start, end, radius, Physics.AllLayers, QueryTriggerInteraction.Collide);
        Ray ray = new Ray(start, (start - end).normalized);
        var pHits = Physics.SphereCastAll(ray, radius, Vector3.Distance(start, end), Physics.AllLayers);
        //if (hits.Length > 0)
        //{
        //    foreach (Collider c in hits)
        //    {
        //        if (!Physics.GetIgnoreLayerCollision(Collider.gameObject.layer, c.gameObject.layer))
        //        {
        //            if (!_stayColliders.Contains(c))
        //            {
        //                CallOnHit(c);
        //                Debug.Log(c.name);
        //                _stayColliders.Add(c);
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
        if (pHits.Length > 0)
        {
            foreach (RaycastHit h in pHits)
            {
                if (!Physics.GetIgnoreLayerCollision(Collider.gameObject.layer, h.collider.gameObject.layer))
                {
                    if (!_stayColliders.Contains(h.collider))
                    {
                        CallOnHit(h.collider);
                        Debug.Log($"{h.collider.name}, {Time.frameCount}");
                        _stayColliders.Add(h.collider);
                    }
                }
                else
                {
                    _stayColliders.Remove(h.collider);
                }
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
            _stayColliders.Clear();
        }
    }

    void CallOnHit(Collider c)
    {
        _onHitAction?.Invoke(c);
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
