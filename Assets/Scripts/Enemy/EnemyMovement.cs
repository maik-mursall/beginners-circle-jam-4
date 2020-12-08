using System;
using UnityEngine;
using UnityEngine.AI;

namespace Enemy
{
    public enum EnemyMovementState
    {
        Idling,
        GoingToStartPosition,
        FollowingTarget
    }
    
    [RequireComponent(typeof(NavMeshAgent))]
    public class EnemyMovement : MonoBehaviour
    {
        private NavMeshAgent _navMeshAgent;
        
        [SerializeField] private Transform target;
        [SerializeField] private Vector3 startingPosition;

        [SerializeField] private float moveSpeed = 10f;
        [SerializeField] private float stoppingDistance = 10f;

        private EnemyMovementState _currentMovementState = EnemyMovementState.GoingToStartPosition;

        public event EventHandler TargetReached;

        private void Start()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _navMeshAgent.speed = moveSpeed;
            _navMeshAgent.stoppingDistance = stoppingDistance;
            _navMeshAgent.destination = startingPosition;
        }

        private void Update()
        {
            if (_currentMovementState == EnemyMovementState.FollowingTarget)
            {
                _navMeshAgent.destination = target.position;
            }

            if (_currentMovementState == EnemyMovementState.GoingToStartPosition)
            {
                CheckIfReachedTarget();
            }
        }

        private void CheckIfReachedTarget()
        {
            if (!_navMeshAgent.pathPending)
            {
                if (_navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance)
                {
                    if (!_navMeshAgent.hasPath || _navMeshAgent.velocity.sqrMagnitude == 0f)
                    {
                        _currentMovementState = EnemyMovementState.Idling;

                        TargetReached?.Invoke(this, EventArgs.Empty);
                    }
                }
            }
        }
        
        public void SetTarget(Transform newTarget)
        {
            target = newTarget;
            stoppingDistance = 0;
            _navMeshAgent.stoppingDistance = stoppingDistance;

            _currentMovementState = EnemyMovementState.FollowingTarget;
        }

        public void SetStartingPosition(Vector3 newStartingPosition)
        {
            startingPosition = newStartingPosition;
            _currentMovementState = EnemyMovementState.GoingToStartPosition;

            if (_navMeshAgent)
            {
                _navMeshAgent.destination = startingPosition;
            }
        }

        public bool IsMoving => _navMeshAgent.velocity.magnitude > 0f;
    }
}
