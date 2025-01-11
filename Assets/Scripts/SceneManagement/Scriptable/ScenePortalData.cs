using UnityEngine;

namespace SceneManagement.Scriptable
{
    // Data를 관리하기 위해 사용, 기능적인 이유 X
    // 포탈 외 추가 가능성 O
    [CreateAssetMenu(fileName = "SceneData", menuName = "SceneData/SceneData")]
    public class ScenePortalData : ScriptableObject
    {
        public string sceneName;
    
        [Space(10)]
        [SerializeField] private SpawnPointData[] spawnPoints;

        private void OnValidate()
        {
            foreach (var spawnPoint in spawnPoints)
            {
                spawnPoint.sceneName = sceneName;
            }
        }
    }
}