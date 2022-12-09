using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeakPoint : MonoBehaviour
{
    [SerializeField] List<GameObject> _jakutens = new List<GameObject>();
    [SerializeField] GameObject _cube = default;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void WeakPointCreate()
    {
        int rnd = Random.Range(0, 3);
        Vector3 position = _jakutens[rnd].transform.position;
        _cube.transform.position = position;
        Invoke("Destroy", 1);
    }
    private void Destroy()
    {
        Destroy(_cube);
    }
}
