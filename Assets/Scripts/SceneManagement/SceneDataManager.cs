using System.Collections.Generic;
using Cinemachine;
using Player;
using SceneManagement.Scriptable;
using UI;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SceneManagement
{
    // 처음에는 각 씬마다 SpawnPoint[]를 직접 Hierarchy창을 통해 직접 넣어줬다.
    // 이후 각 SpawnPoint에서 싱글톤 Manager에 직접 등록하게 만들었다. 그러나 싱글톤의 생성 시점으로 인해 Script Execution Order를 강제해야 하게 되었다.
    // 보완하여 Order의 제약을 없앴다.
    
    // PlaySceneManager의 컴포넌트로 작동하고, 각 SpawnPoint는 추가만 하면 되며 SceneLoad시 자동으로 등록되도록 만듦.
    public class SceneDataManager : MonoBehaviour
    {
        public GameObject defaultPlayerPrefab;
        public GameObject lockOnTarget;
        public UIManager uiManager;
        public CinemachineVirtualCamera virtualCamera;
        
        // SpawnPoint.id, SpawnPoint
        private readonly Dictionary<string, SpawnPoint> _dictionary = new ();

        public void Initialize(ActionPlayer player)
        {
            var playerInput = player.GetComponent<PlayerInput>();
            uiManager.Initialize(playerInput);
        }
        
        public SpawnPoint GetSpawnPoint(string id)
        {
            return _dictionary[id];
        }
        
        public SpawnPoint GetDefaultSpawnPoint()
        {
            if (_dictionary.Count > 0)
            {
                var enumerator = _dictionary.GetEnumerator();
                enumerator.MoveNext();
                //Debug.LogWarning($"{enumerator.Current} -  {enumerator.Current.Key} - {enumerator.Current.Value.name}");
                // LinQ로 가져오는 대신 Enumerator로 가져옴.
                return enumerator.Current.Value;
            }
            else
            {
                return null;
            }
        }

        public void AddSpawnPoint(SpawnPoint spawnPoint)
        {
            Debug.LogWarning("Add Spawn Point");
            _dictionary.Add(spawnPoint.spawnPointData.id, spawnPoint);
        }

        public void RemoveSpawnPoint(SpawnPoint spawnPoint)
        {
            _dictionary.Remove(spawnPoint.spawnPointData.id);
        }
    }
}
