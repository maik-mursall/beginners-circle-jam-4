using System;
using UnityEngine;
using UnityEngine.AI;

namespace Enemy
{
    public enum EnemyMovementState
    {
        Idling,
        GoingToStaticTarget,
        FollowingTarget,
    }
    
    [RequireComponent(typeof(NavMeshAgent))]
    public class EnemyMovement : MonoBehaviour
    {
        private NavMeshAgent _navMeshAgent;
        
        [SerializeField] private Transform target;
        public Transform Target => target;

        [SerializeField]
        private Vector3 staticPosition;

        [SerializeField] private float moveSpeed = 10f;
        [SerializeField] private float stoppingDistance = 10f;

        private EnemyMovementState _currentMovementState = EnemyMovementState.GoingToStaticTarget;
        public EnemyMovementState CurrentMoveState => _currentMovementState;

        public event EventHandler TargetReached;

        private bool _invokedTargetReachedEvent;

        public void SetMoveSpeed(float newSpeed) => _navMeshAgent.speed = newSpeed;
        public void ResetMoveSpeed() => _navMeshAgent.speed = moveSpeed;

        private void Start()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
            ResetMoveSpeed();
            _navMeshAgent.stoppingDistance = stoppingDistance;
            _navMeshAgent.destination = staticPosition;
        }

        private void Update()
        {
            if (_currentMovementState == EnemyMovementState.FollowingTarget)
            {
                _navMeshAgent.destination = target.position;
            }

            if (_currentMovementState == EnemyMovementState.GoingToStaticTarget)
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
                        if (_invokedTargetReachedEvent)
                        {
                            _currentMovementState = EnemyMovementState.FollowingTarget;
                        }
                        else
                        {
                            _currentMovementState = EnemyMovementState.Idling;
                            _invokedTargetReachedEvent = true;
                        }
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

        public void SetStaticPosition(Vector3 newStartingPosition)
        {
            staticPosition = newStartingPosition;
            _currentMovementState = EnemyMovementState.GoingToStaticTarget;

            if (_navMeshAgent)
            {
                _navMeshAgent.destination = staticPosition;
            }
        }

        public bool IsMoving => _navMeshAgent.velocity.magnitude > 0f;
    }
}
