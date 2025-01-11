using UnityEngine;

namespace Player
{
    // 아이템을 밀어내는 클래스
    public class BasicRigidBodyPush : MonoBehaviour
    {
        public LayerMask pushLayers;
        public bool canPush;
        [Range(0.2f, 1.5f)] public float strength = .7f;

        private PlayerController _controller;

        private void Start()
        {
            _controller = GetComponent<PlayerController>();
        }

        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            if (canPush) PushRigidBodies(hit);
        }

        private void PushRigidBodies(ControllerColliderHit hit)
        {
            // make sure we hit a non kinematic rigidbody
            Rigidbody body = hit.collider.attachedRigidbody;
            if (body == null || body.isKinematic) return;

            // make sure we only push desired layer(s)
            var bodyLayerMask = 1 << body.gameObject.layer;
            if ((bodyLayerMask & pushLayers.value) == 0) return;

            // don't want to push object below us
            if (hit.moveDirection.y < -0.2f) return;

            // Calculate push direction from move direction, horizontal motion only
            Vector3 pushDir = new Vector3(hit.moveDirection.x, 0.3f, hit.moveDirection.z);

            // Apply the push and take strength into account
            body.AddForce(pushDir * strength * _controller.GetMoveSpeed(), ForceMode.Impulse);
        }
    }
}