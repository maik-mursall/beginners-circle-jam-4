using UnityEngine;

namespace Utils
{
    public class FollowTransform : MonoBehaviour
    {
        [SerializeField] private Transform transformToFollow;

        private void Update()
        {
            if (transformToFollow)
            {
                var ownTransform = transform;

                ownTransform.position = transformToFollow.position;
                ownTransform.rotation = transformToFollow.rotation;
            }
        }
    }
}
