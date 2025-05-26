using System.Collections.Generic;
using System.Runtime;
using UnityEngine;

public class BulletHolePool
{
    private GameObject _prefab;
    private int _poolSize;
    private float _lifetime;
    private Transform _parent;
    private MonoBehaviour _coroutineHost;

    private Queue<GameObject> _pool = new Queue<GameObject>();

    public BulletHolePool(GameObject bulletHolePrefab, int size, float lifetime, Transform parent, MonoBehaviour coroutineHost)
    {
        _prefab = bulletHolePrefab;
        _poolSize = size;
        _lifetime = lifetime;
        _parent = parent;
        _coroutineHost = coroutineHost;
            
        InitializePool();
    }

    private void InitializePool()
    {
        for (int i = 0; i < _poolSize; i++)
        {
            GameObject obj = Object.Instantiate(_prefab, _parent);
            obj.SetActive(false);
            _pool.Enqueue(obj);
        }
    }

    public void Spawn(Vector3 position, Quaternion rotation, Transform attachTo = null)
    {
        if (_pool.Count == 0)
        {
            return;
        }

        GameObject obj = _pool.Dequeue();
        obj.transform.position = position;
        obj.transform.rotation = rotation;
        obj.transform.SetParent(attachTo ?? _parent);
        obj.SetActive(true);

        _coroutineHost.StartCoroutine(DisableAfterDelay(obj));
    }

    private System.Collections.IEnumerator DisableAfterDelay(GameObject obj)
    {
        yield return new WaitForSeconds(_lifetime);
        obj.SetActive(false);
        _pool.Enqueue(obj);
    }
}
