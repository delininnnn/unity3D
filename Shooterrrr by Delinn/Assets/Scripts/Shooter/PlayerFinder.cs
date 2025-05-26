using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFinder : MonoBehaviour
{
    [SerializeField] private EnemyPatrol _enemyPatrol;
    [SerializeField] private float _viewAngle = 110f;
    [SerializeField] private float _sphereRadius = 5f;
    [SerializeField] private LayerMask _targetLayer;
    [SerializeField] private LayerMask _obstacleLayer;
    private RaycastHit _hit;
    private RaycastHit _playerhit;
    private Transform _playerTransform;
    private bool _seePlayer;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        float halfAngle = _viewAngle / 2;
        Vector3 leftBoundary = Quaternion.Euler(0, -halfAngle, 0) * transform.forward;
        Vector3 rightBoundary = Quaternion.Euler(0, halfAngle, 0) * transform.forward;

        Gizmos.DrawLine(transform.position, transform.position + leftBoundary * _sphereRadius);
        Gizmos.DrawLine(transform.position, transform.position + rightBoundary * _sphereRadius);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _sphereRadius);
        if (_seePlayer)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, _playerTransform.position);
        }
    }

    void Update()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, _sphereRadius, _targetLayer);
        _seePlayer = false;
        foreach (Collider collider in colliders)
        {
            Vector3 toTarget = collider.transform.position - transform.position;
            float angleToTarget = Vector3.Angle(transform.forward, toTarget);
            float distanceToTarget = toTarget.magnitude;
            if (angleToTarget < _viewAngle / 2)
            {
                Physics.Raycast(transform.position, toTarget.normalized, out _playerhit, _sphereRadius, _targetLayer);
                if (!Physics.Raycast(transform.position, toTarget.normalized, out _hit, _playerhit.distance, _obstacleLayer))
                {
                    _playerTransform = collider.transform;
                    _seePlayer = true;
                    _enemyPatrol.SetPlayerDestonation(_playerTransform.position);
                    break;
                }
            }
        }
        if (!_seePlayer)
        {
            _enemyPatrol.SetPointDestonation();
            _playerTransform = null;
        }
    }
}
