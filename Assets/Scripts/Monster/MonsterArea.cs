using UnityEngine;

namespace Monster
{
    public class MonsterArea : MonoBehaviour
    {
        [SerializeField] private int radius;

        private void OnDrawGizmos()
        {
            Gizmos.color = new Color(1f, 0.5f, 0f, 1f);
            
            Gizmos.DrawWireSphere(transform.position, radius);
        }

        public bool IsInArea(Vector3 target)
        {
            if (Vector3.Distance(transform.position, target) > radius)
            {
                return false;
            }

            return true;
        }
    }
}
