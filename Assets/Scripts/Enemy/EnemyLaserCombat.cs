using System.Collections;
using DG.Tweening;
using Gameplay.HypeMeter;
using UnityEngine;

namespace Enemy
{
    public class EnemyLaserCombat : MonoBehaviour
    {
        [SerializeField] private LineRenderer lineRenderer;
        
        [SerializeField] private float damage = 20f;
        [SerializeField] private float attackRange = 20f;
        [SerializeField] private float attackMinCooldown = 5f;
        [SerializeField] private float attackMaxCooldown = 10f;
        [SerializeField] private float attackDuration = 5f;
        [SerializeField] private float minGoBackRange = 5f;
        [SerializeField] private float maxGoBackRange = 10f;
        
        [SerializeField] private Transform rayTransform;
        [SerializeField] private float rayRadius;
        [SerializeField] private LayerMask rayLayerMask;
        [SerializeField] private Quaternion rayStartRotation;

        private EnemyMovement _movement;

        private bool _onCooldown = true;

        private void Start()
        {
            _movement = GetComponent<EnemyMovement>();

            _movement.TargetReached += (sender, args) => StartCoroutine(StartCooldownCoroutine());
        }
        
        private void Update()
        {
            if (_movement.CurrentMoveState != EnemyMovementState.Idling && !(_movement.Target is null) && !_onCooldown && Vector3.Distance(_movement.Target.position, transform.position) <= attackRange)
            {
                StartAttacking();
            }

            if (lineRenderer.enabled)
            {
                ShootLaser();
            }
        }

        private void ShootLaser()
        {
            lineRenderer.SetPosition(0, rayTransform.position);

            if (Physics.SphereCast(rayTransform.position, rayRadius, rayTransform.forward, out RaycastHit hit,
                Mathf.Infinity, rayLayerMask))
            {
                if (hit.transform.CompareTag("Player"))
                {
                    HypeMeter.Instance.AddAbsoluteHype(-damage);
                    lineRenderer.SetPosition(1, hit.point);
                }
            }
            else
            {
                lineRenderer.SetPosition(1, rayTransform.forward * 1000f);
            }
        }

        private void StartAttacking()
        {
            _movement.SetMoveSpeed(0);
            lineRenderer.enabled = true;

            rayTransform.localRotation = rayStartRotation;
            rayTransform.DOLocalRotate(Vector3.zero, 4f);
            
            _onCooldown = true;

            StopAllCoroutines();
            StartCoroutine(StartAttackCoroutine());
        }
        
        private void StopAttacking()
        {
            _movement.ResetMoveSpeed();
            lineRenderer.enabled = false;

            var randomPosition = Random.insideUnitCircle * Random.Range(minGoBackRange, maxGoBackRange);
            _movement.SetStaticPosition(
                new Vector3(randomPosition.x, 0f, randomPosition.y)
            );
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
