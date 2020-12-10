using System.Collections;
using Combat;
using Gameplay.HypeMeter;
using UnityEngine;

namespace Enemy
{
    public class EnemyMeleeCombat : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private SphereCollider attackCollider;
        
        [SerializeField] private float damage = 20f;
        [SerializeField] private float attackRange = 20f;
        [SerializeField] private float attackSpeed = 20f;
        [SerializeField] private float attackDuration = 5f;

        private EnemyMovement _movement;

        public bool shouldAttack;

        private void Start()
        {
            _movement = GetComponent<EnemyMovement>();
        }
        
        private static readonly int IsAttacking = Animator.StringToHash("isAttacking");

        private void Update()
        {
            if (shouldAttack && Vector3.Distance(_movement.Target.position, transform.position) >= attackRange)
            {
                StartAttacking();
            }
        }

        private void StartAttacking()
        {
            animator.SetBool(IsAttacking, true);
            _movement.SetMoveSpeed(attackSpeed);
            attackCollider.enabled = true;

            StopAllCoroutines();
            StartCoroutine(StartAttackCoroutine());
        }
        
        private void StopAttacking()
        {
            animator.SetBool(IsAttacking, false);
            _movement.ResetMoveSpeed();
            attackCollider.enabled = false;
            shouldAttack = false;
        }
        
        private IEnumerator StartAttackCoroutine()
        {
            yield return new WaitForSeconds(attackDuration);
            
            StopAttacking();
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                HypeMeter.Instance.AddAbsoluteHype(-damage);
                StopAttacking();
            }
        }
    }
}
