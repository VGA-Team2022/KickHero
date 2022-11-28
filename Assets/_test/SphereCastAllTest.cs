using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereCastAllTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Ray ray = new Ray(Vector3.back * 10, Vector3.forward);
        RaycastHit[] hits = Physics.SphereCastAll(ray, 0.5f, 20f, Physics.AllLayers);
        foreach (RaycastHit hit in hits)
        {
            Debug.Log($"{hit.collider.name}, {Time.frameCount}");
        }
    }
}
