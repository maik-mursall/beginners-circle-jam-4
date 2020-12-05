using Combat;
using Gameplay;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerMovement : MonoBehaviour
    {
        private CharacterController _characterController;

        [SerializeField] private WeaponHandler weaponHandler;

        private Vector2 _playerInput = Vector2.zero;

        [SerializeField] private float moveSpeed = 10f;
        private void Start()
        {
            _characterController = GetComponent<CharacterController>();
        }

        private void Update()
        {
            _playerInput.x = Input.GetAxisRaw("Horizontal");
            _playerInput.y = Input.GetAxisRaw("Vertical");

            _playerInput *= weaponHandler.PlayerCanMove ? 1f : 0f;
        }

        private void FixedUpdate()
        {
            if (!GameManager.Instance.IsGameRunning) return;

            var normalizedPlayerInput = _playerInput.normalized;
            _characterController.Move(new Vector3(normalizedPlayerInput.x, 0f, normalizedPlayerInput.y) * (moveSpeed * Time.fixedDeltaTime));
        }
    }
}
