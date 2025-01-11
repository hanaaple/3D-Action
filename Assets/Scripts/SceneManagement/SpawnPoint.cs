using System;
using Manager;
using SceneManagement.Scriptable;
using UnityEngine;

namespace SceneManagement
{
    public class SpawnPoint : MonoBehaviour
    {
        public SpawnPointData spawnPointData;

        private void Awake()
        {
            PlaySceneManager.instance.AddSpawnPoint(this);
        }

        private void OnDestroy()
        {
            PlaySceneManager.instance.RemoveSpawnPoint(this);
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawCube(transform.position, Vector3.one);
        }
    }
}