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
            var cameraRotation = _cameraTransform.rotation;
            transform.LookAt(transform.position + cameraRotation * Vector3.back,
                cameraRotation * Vector3.up);
        }
    }
}
