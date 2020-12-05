using System;
using UnityEngine;

namespace Enemy
{
    public class EnemyAnimator : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private EnemyMovement enemyMovement;
        private static readonly int IsMoving = Animator.StringToHash("isMoving");

        private void Update()
        {
            animator.SetBool(IsMoving, enemyMovement.IsMoving);
        }
    }
}
