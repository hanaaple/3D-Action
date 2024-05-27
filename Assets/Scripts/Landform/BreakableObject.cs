using UnityEngine;

namespace Landform
{
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