using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

[RequireComponent(typeof(SphereCollider), typeof(Rigidbody))]
public class BallView : MonoBehaviour
{
    Action<Collider> _onHitAction;

    SphereCollider _collider;
    List<Collider> _stayColliders = new List<Collider>();

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
            HitDetermine(Position, value);
            transform.position = value;
        }
    }

    public void OnHit(Action<Collider> action)
    {
        _onHitAction += action;
    }

    void HitDetermine(Vector3 start, Vector3 end)
    {
        var hits = Physics.OverlapCapsule(start, end, Collider.radius, Physics.AllLayers, QueryTriggerInteraction.Collide);
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
}
