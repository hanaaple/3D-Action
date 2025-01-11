using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Character
{
    public class Sight : MonoBehaviour
    {
        [SerializeField] private bool isAutoUpdate;
        
        [SerializeField] protected float depth;

        [Range(0.1f, 179f)] [SerializeField] protected float fov;
        [Range(0.1f, 3f)] [SerializeField] protected float aspect;

        [Range(0, 1)] [SerializeField] private float viewPortW;
        [Range(0, 1)] [SerializeField] private float viewPortH;
        
        [SerializeField] private LayerMask targetMask;
        [SerializeField] private LayerMask obstructionMask;

        private List<GameObject> _targetObjects = new();

        protected virtual void Update()
        {
            if(isAutoUpdate)
                FieldOfViewCheck();
        }

        private void FieldOfViewCheck()
        {
            _targetObjects.Clear();
            
            var position = transform.position + Vector3.forward * depth / 2;
            var rotation = transform.rotation;
            var size = CalculateWireCubeSize();
            var hits = Physics.OverlapBox(position, size / 2, rotation, targetMask);

            if (hits.Length > 0)
            {
                foreach (var hit in hits)
                {
                    Transform target = hit.transform;
                    Vector3 directionToTarget = (target.position - transform.position).normalized;

                    if (IsInsideFrustum(hit.transform.position))
                    {
                        float distanceToTarget = Vector3.Distance(transform.position, target.position);

                        if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask))
                        {
                            _targetObjects.Add(target.gameObject);
                        }
                    }
                }
            }
        }

        private bool IsInsideFrustum(Vector3 position)
        {
            // Create a matrix that transforms from world space to frustum space
            var size = new Vector3(viewPortW, viewPortH, 1);
            Matrix4x4 frustumMatrix = Matrix4x4.TRS(transform.position, transform.rotation, size);

            // Transform the position into frustum space
            Vector3 localPosition = frustumMatrix.inverse.MultiplyPoint(position);

            // Check if the transformed position is inside the frustum
            bool insideFrustum = Mathf.Abs(localPosition.z) <= depth && Mathf.Abs(localPosition.x) <=
                                 Mathf.Abs(localPosition.z) * Mathf.Tan(fov * 0.5f * Mathf.Deg2Rad * aspect) &&
                                 Mathf.Abs(localPosition.y) <=
                                 Mathf.Abs(localPosition.z) * Mathf.Tan(fov * 0.5f * Mathf.Deg2Rad);

            return insideFrustum;
        }

        private void OnDrawGizmos()
        {
            var originalMatrix = Gizmos.matrix;

            Gizmos.color = Color.blue;
            var size = new Vector3(viewPortW, viewPortH, 1);
            Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, size);
            Gizmos.DrawFrustum(Vector3.zero, fov, depth, 0f, aspect);

            // var cubeSize = CalculateWireCubeSize();
            // Gizmos.color = Color.blue;
            // Gizmos.matrix = Matrix4x4.TRS(transform.position + Vector3.forward * depth / 2, transform.rotation,
            //     Vector3.one);
            // Gizmos.DrawWireCube(Vector3.zero, cubeSize);

            Gizmos.matrix = originalMatrix;

            if (_targetObjects != null)
            {
                Gizmos.color = Color.green;
                foreach (var targetObject in _targetObjects)
                {
                    Gizmos.DrawLine(transform.position, targetObject.transform.position);
                }
            }
        }

        private Vector3 CalculateWireCubeSize()
        {
            // 프러스텀의 높이를 계산
            float frustumHeight = 2 * Mathf.Tan(fov * 0.5f * Mathf.Deg2Rad) * depth;
            // 프러스텀의 너비 계산
            float frustumWidth = frustumHeight * aspect;

            frustumHeight *= viewPortH;
            frustumWidth *= viewPortW;
            

            Vector3 cubeSize = new Vector3(frustumWidth, frustumHeight, depth);

            return cubeSize;
        }

        public bool TryGetTargetGameObject(out List<GameObject> target)
        {
            if(!isAutoUpdate)
                FieldOfViewCheck();
            
            if (_targetObjects != null && _targetObjects.Count > 0)
            {
                target = _targetObjects;
                return true;
            }
            
            target = null;
            return false;
        }
        
        public bool TryGetNearestTargetInSight(out GameObject target)
        {
            if(!isAutoUpdate)
                FieldOfViewCheck();
            
            if (_targetObjects != null && _targetObjects.Count > 0)
            {
                var first = _targetObjects.OrderBy(item =>
                {
                    var angle = Vector3.Angle(item.transform.position - transform.position, transform.forward);
                    return angle;
                }).First();

                target = first;
                return true;
            }
            
            target = null;
            return false;
        }
    }
}