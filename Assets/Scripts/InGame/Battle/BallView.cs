using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;


/// <summary>
/// �{�[���̕\�ʓI�ȏ������s���N���X
/// </summary>
[RequireComponent(typeof(SphereCollider), typeof(Rigidbody))]
public class BallView : MonoBehaviour
{
    /// <summary>�����ɓ����������ɔ��s����C�x���g</summary>
    Action<Collider> _onHitActionCollider;
    /// <summary>�����ɓ����������ɂ���RaycastHit��n���C�x���g(Position�ɂ��ړ�������)</summary>
    Action<RaycastHit> _onHitActionRaycastHit;
    /// <summary>�����ɓ����������ɂ���Collision��n���C�x���g(Rigidbody�ɂ��ړ�������)</summary>
    Action<Collision> _onHitActionCollision;

    SphereCollider _collider;
    Rigidbody _rb;
    /// <summary>�ڐG���̃R���C�_�[�̃��X�g</summary>
    List<Collider> _stayColliders = new List<Collider>();
    bool _isCollide = false;
    Vector3 _lastPosition;

    bool _isKinematic;

    public Rigidbody Rigidbody
    {
        get
        {
            if (!_rb)
            {
                _rb = GetComponent<Rigidbody>();
                if (!_rb)
                {
                    Debug.LogError($"{nameof(Rigidbody)}��������܂���");
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
        }
    }


    /// <summary>�����蔻�����邩�ۂ�</summary>
    public bool IsCollide
    {
        get => _isCollide;
        set
        {
            Debug.Log($"chenge{value}");
            _isCollide = value;
            _stayColliders.Clear();
        }
    }

    private void Start()
    {
        //Rigidbody.isKinematic = true;
        //Rigidbody.Sleep();
        _collider = GetComponent<SphereCollider>();
    }

    public void OnHit(Action<Collider> action)
    {
        _onHitActionCollider += action;
    }
    public void OnHit(Action<Collision> action)
    {
        _onHitActionCollision += action;
    }

#if false
    public void OnHit(Action<RaycastHit> action)
    {
        _onHitActionRaycastHit += action;
    }
#endif

    /// <summary>
    /// ��_�Ԃ��ړ��������̓����蔻������
    /// �����ɐڐG�������ACollider��n���C�x���g�ƁA
    /// Collision�����RaycastHit��n���C�x���g�𔭍s����
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
                        Debug.Log(_isCollide);
                        //CallOnHit(co.collider);
                        //CallOnHit(co);
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


    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(_isCollide);
        if (_isCollide)
        {
            CallOnHit(collision.collider);
            CallOnHit(collision);
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
    void CallOnHit(Collision c)
    {
        _onHitActionCollision?.Invoke(c);
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
