using System;
using UnityEngine;
using UnityEngine.AI;

namespace Enemy
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class EnemyMovement : MonoBehaviour
    {
        private NavMeshAgent _navMeshAgent;
        
        [SerializeField] private Transform target;

        [SerializeField] private float moveSpeed = 10f;

        private void Start()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _navMeshAgent.speed = moveSpeed;
        }

        private void Update()
        {
            if (target)
            {
                _navMeshAgent.destination = target.position;
            }
        }
    }
}
