using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyPatrol : MonoBehaviour
{
    [SerializeField] private Transform[] _points;
    [SerializeField] private NavMeshAgent _enemyAgent;
    [SerializeField] private float _waitTime;
    private bool _followPlayer;
    void Start()
    {
        StartCoroutine(ChangePoint(0));
    }

    private IEnumerator ChangePoint(int point)
    {
        for (int i = point; i < _points.Length; i++)
        {
            _enemyAgent.SetDestination(_points[i].position);
            while (_enemyAgent.transform.position.x != _points[i].position.x &&
                _enemyAgent.transform.position.z != _points[i].position.z)
            {
                yield return new WaitForSeconds(0.2f);
            }
            yield return new WaitForSeconds(_waitTime);
        }
        StartCoroutine(ChangePoint(0));
    }

    public void SetPlayerDestonation(Vector3 player)
    {
        _enemyAgent.SetDestination(player);
        _followPlayer = true;
    }

    public void SetPointDestonation()
    {
        if (_followPlayer)
        {
            StartCoroutine(ChangePoint(FindClosePoint()));
            _followPlayer = false;
        }
    }

    private int FindClosePoint()
    {
        Vector3 previousPointPosition = _points[0].position;
        int closePointIndex = 0;
        for (int i = 0; i < _points.Length; i++)
        {
            if (Vector3.Distance(_enemyAgent.transform.position, previousPointPosition) > Vector3.Distance(_enemyAgent.transform.position, _points[i].position))
            {
                previousPointPosition = _points[i].position;
                closePointIndex = i;
            }
        }
        return closePointIndex;
    }
}

