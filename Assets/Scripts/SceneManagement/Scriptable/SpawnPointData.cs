using UnityEngine;
using Util;

namespace SceneManagement.Scriptable
{
    [CreateAssetMenu(fileName = "SpawnPoint", menuName = "SceneData/SpawnPoint")]
    public class SpawnPointData : ScriptableObject
    {
        public string sceneName { get; set; }
        [UniqueId] public string id;
    }
}