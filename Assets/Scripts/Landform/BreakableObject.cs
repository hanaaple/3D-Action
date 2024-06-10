using UnityEngine;

namespace Landform
{
    /// <summary>
    /// 플레이어와 부딪혀 부서지는 오브젝트
    /// </summary>
    public class DestructibleObject : MonoBehaviour
    {
        public GameObject shatteredVersion;

        private void OnTriggerEnter(Collider col)
        {
            Instantiate(shatteredVersion, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}