using System.Collections;
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
        [SerializeField] private float attackMinCooldown = 5f;
        [SerializeField] private float attackMaxCooldown = 10f;
        [SerializeField] private float attackDuration = 5f;
        [SerializeField] private float minGoBackRange = 5f;
        [SerializeField] private float maxGoBackRange = 10f;

        private EnemyMovement _movement;

        private bool _onCooldown = true;

        private void Start()
        {
            _movement = GetComponent<EnemyMovement>();

            _movement.TargetReached += (sender, args) => StartCoroutine(StartCooldownCoroutine());
        }
        
        private static readonly int IsAttacking = Animator.StringToHash("isAttacking");

        private void Update()
        {
            if (_movement.CurrentMoveState != EnemyMovementState.Idling && !(_movement.Target is null) && !_onCooldown && Vector3.Distance(_movement.Target.position, transform.position) >= attackRange)
            {
                StartAttacking();
            }
        }

        private void StartAttacking()
        {
            animator.SetBool(IsAttacking, true);
            _movement.SetMoveSpeed(attackSpeed);
            attackCollider.enabled = true;
            _onCooldown = true;

            StopAllCoroutines();
            StartCoroutine(StartAttackCoroutine());
        }
        
        private void StopAttacking()
        {
            animator.SetBool(IsAttacking, false);
            _movement.ResetMoveSpeed();

            var randomPosition = Random.insideUnitCircle * Random.Range(minGoBackRange, maxGoBackRange);
            _movement.SetStaticPosition(
                new Vector3(randomPosition.x, 0f, randomPosition.y)
            );
            
            attackCollider.enabled = false;
        }
        
        private IEnumerator StartAttackCoroutine()
        {
            yield return new WaitForSeconds(attackDuration);
            
            StopAttacking();
        }

        private IEnumerator StartCooldownCoroutine()
        {
            _onCooldown = true;

            yield return new WaitForSeconds(Random.Range(attackMinCooldown, attackMaxCooldown));
            
            _onCooldown = false;
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
