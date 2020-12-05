using UnityEngine;

namespace Utils
{
    public class LookAtCamera : MonoBehaviour
    {
        private Transform _cameraTransform;

        private void Start()
        {
            _cameraTransform = Camera.main.transform;
        }

        private void LateUpdate()
        {
            transform.rotation = _cameraTransform.rotation * Quaternion.Euler(0f, 180f, 0f);
        }
    }
}
