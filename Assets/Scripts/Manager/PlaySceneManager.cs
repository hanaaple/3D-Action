using System.Collections.Generic;
using System.ComponentModel;
using Player;
using SceneManagement;
using SceneManagement.Scriptable;
using UI.ToastMessage;
using UnityEngine;
using Util;

namespace Manager
{
    // GameManager
    public class PlaySceneManager : MonoBehaviour
    {
        private static PlaySceneManager _instance;
        public static PlaySceneManager instance => _instance;
    
        // 너무 깊숙이 들어가면 데메테르의 법칙 깨지게 됨.
        public ActionPlayer player { get; private set; } 
        public PlayerDataManager playerDataManager => player?.playerDataManager;
        
        [SerializeField] private SceneDataManager sceneDataManager;
        public ToastMessageManager toastMessageManager;
        public EquipmentInstanceManager equipmentInstanceManager;
    
        // int saveIndex
        private SpawnPointData _lastSpawnPointData;
        private readonly Dictionary<ViewModelType, PropertyChangedEventHandler> _playerDataBindEvents = new();
        
        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(this);
            }
            else
            {
                DestroyImmediate(gameObject);
            }
        }

        public void Release()
        {
            ReleasePlayer();
        }

        private void ReleasePlayer()
        {
            DestroyImmediate(player.gameObject);
            player = null;
        }
 
        // Player
        // Instantiate (What) -> Player.LoadData() & Data, Scene Transform, Position
    
        // Monster
        // Instantiate -> LoadData -> Work
    
        // Npc
        // 정적 Npc (특정 조건을 맞춰야 되는 Npc 존재)
        // 동적 Npc -> 각종 퀘스트, 스토리에 따라 상태(위치, 상태)가 변함. -> 
        // 기본 존재 -> LoadData (Load What -> 상점 상태?, 퀘스트 현황?
    
    
    
        // 퀘스트는 따로 리스트가 있다.
    
        // 퀘스트에서 시작 Npc, 중간 단계, 임무,
        // 시작 -> 중간 -> 끝 (자동 or 퀘스트 가능해짐 -> Observe & Notify로 상태 Update)
    
    
        // Npc는 기본으로 존재함. -> 일부 퀘스트에 의해 Npc가 이동함. -> 

    
        public void InstantiatePlayer()
        {
            Debug.Log("Instantiating player");
            InstantiatePlayer(sceneDataManager.defaultPlayerPrefab);
        }
    
        public void InstantiatePlayer(GameObject playerPrefab)
        {
            player = Instantiate(playerPrefab).GetComponent<ActionPlayer>();
            player.Initialize(sceneDataManager.lockOnTarget, sceneDataManager.virtualCamera, sceneDataManager.uiManager);
            
            sceneDataManager.Initialize(player);
            
            foreach (var (viewModelType, eventHandler) in _playerDataBindEvents)
            {
                playerDataManager.AddPropertyChange(viewModelType, eventHandler);
            }
            
            _playerDataBindEvents.Clear();
        }

        public void Teleport(string portalId)
        {
            var spawnPoint = string.IsNullOrEmpty(portalId) ? sceneDataManager.GetDefaultSpawnPoint() : sceneDataManager.GetSpawnPoint(portalId);

            _lastSpawnPointData = spawnPoint.spawnPointData;

            player.Teleport(spawnPoint.transform);
        }

        public void Respawn()
        {
            SceneLoader.Instance.LoadScene(_lastSpawnPointData, SceneLoadType.Continue);
        }
    
        public void AddSpawnPoint(SpawnPoint spawnPoint)
        {
            sceneDataManager.AddSpawnPoint(spawnPoint);
        }

        public void RemoveSpawnPoint(SpawnPoint spawnPoint)
        {
            sceneDataManager.RemoveSpawnPoint(spawnPoint);
        }
        
        public void BindPlayerData(ViewModelType viewModelType, PropertyChangedEventHandler eventHandler)
        {
            if (playerDataManager == null)
            {
                _playerDataBindEvents.TryAdd(viewModelType, null);

                _playerDataBindEvents[viewModelType] += eventHandler;
            }
            else
                playerDataManager.AddPropertyChange(viewModelType, eventHandler);
        }

        public void UnBindPlayerData(ViewModelType viewModelType, PropertyChangedEventHandler eventHandler)
        {
            playerDataManager?.RemovePropertyChange(viewModelType, eventHandler);
        }
    }
}