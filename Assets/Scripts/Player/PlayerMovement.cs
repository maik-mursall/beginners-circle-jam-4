using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerMovement : MonoBehaviour
    {
        private CharacterController _characterController;

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
        }

        private void FixedUpdate()
        {
            var normalizedPlayerInput = _playerInput.normalized;
            _characterController.Move(new Vector3(normalizedPlayerInput.x, 0f, normalizedPlayerInput.y) * (moveSpeed * Time.fixedDeltaTime));
        }
    }
}
