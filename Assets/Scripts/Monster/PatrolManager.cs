using UnityEngine;
using Util;
using Random = UnityEngine.Random;

namespace Monster
{
    public enum PatrolType
    {
        Random,
        Sequence
    }

    public class PatrolManager : MonoBehaviour
    {
        [SerializeField] private Transform[] patrolTargets;
        [SerializeField] private PatrolType patrolType;

        [ConditionalHideInInspector("patrolType", PatrolType.Sequence)]
        [SerializeField] private bool isRepeat;
        
        [SerializeField] private int _patrolIndex;

        public Transform GetPatrolTransform()
        {
            if (patrolTargets.Length > 0)
            {
                return patrolTargets[_patrolIndex];
            }
            else
            {
                return transform;
            }
        }

        public Transform GetNextPatrolTransform()
        {
            if (patrolTargets.Length > 0)
            {
                return patrolTargets[GetNextPatrolIndex()];
            }
            else
            {
                return transform;
            }
        }
        
        public void ChangePatrolIndex()
        {
            _patrolIndex = GetNextPatrolIndex();
        }
        
        private int GetNextPatrolIndex()
        {
            int nextIndex = 0;
            if (patrolType == PatrolType.Random)
            {
                nextIndex = Random.Range(0, patrolTargets.Length);
            }
            else if (patrolType == PatrolType.Sequence)
            {
                if (isRepeat)
                {
                    nextIndex = (_patrolIndex + 1) % patrolTargets.Length;
                }
                else
                {
                    nextIndex = Mathf.Clamp(_patrolIndex + 1, 0, patrolTargets.Length - 1);
                }
            }

            return nextIndex;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            foreach (var patrolTarget in patrolTargets)
            {
                Gizmos.DrawSphere(patrolTarget.position, 1f);
            }
        }
    }
}