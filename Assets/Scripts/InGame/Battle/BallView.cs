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

    Action<Collider> _onHitAction;

    SphereCollider _collider;
    List<Collider> _stayColliders = new List<Collider>();
    bool _isCollision = false;


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

    public Vector3 Position
    {
        get => transform.position;
        set
        {
            if (_isCollision)
            {
                Vector3 scale = transform.lossyScale;
                HitDetermine(Position, value, Collider.radius * Mathf.Max(Mathf.Max(scale.x, scale.y), scale.z));
            }
            transform.position = value;
        }
    }


    /// <summary>“–‚½‚è”»’è‚ðŽæ‚é‚©”Û‚©</summary>
    public bool IsCollision { get => _isCollision; set { _isCollision = value;} }

    public void OnHit(Action<Collider> action)
    {
        _onHitAction += action;
    }

    void HitDetermine(Vector3 start, Vector3 end, float radius)
    {
        var hits = Physics.OverlapCapsule(start, end, radius, Physics.AllLayers, QueryTriggerInteraction.Collide);
        if (hits.Length > 0)
        {
            foreach (Collider c in hits)
            {
                if (!Physics.GetIgnoreLayerCollision(Collider.gameObject.layer, c.gameObject.layer))
                {
                    if (!_stayColliders.Contains(c))
                    {
                        CallOnHit(c);
                        _stayColliders.Add(c);
                    }
                    for (int i = 0; i < _stayColliders.Count; i++)
                    {
                        if ((hits.Where(p => p == _stayColliders[i]).Count() == 0))
                        {
                            _stayColliders.RemoveAt(i);
                            i--;
                        }
                    }
                }
                else
                {
                    _stayColliders.Remove(c);
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

    private void Update()
    {
        if (!Application.isPlaying)
        {

        }
    }
}
