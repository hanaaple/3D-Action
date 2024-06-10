using System.Collections.Generic;
using Data.Static.Scriptable;
using UnityEngine;

namespace Util
{
    // Addressable 사용 시 - 데이터 추가, 버전 관리에 매우 용이함
    // Scriptable Object 변화를 버전 관리(업데이트)해주어 따로 어플리케이션을 빌드하지 않아도 됨.
    // 직접 캐싱 (Inspector View) 해두지 않아도 Label를 통해 관리 가능 
    // 다만, 이번에는 Addressable이 주 목적이 아니기에 이를 제외함.
    
    /// <summary>
    /// Id에 따라 ScriptableObject(ItemData)를 캐싱하는 Manager
    /// Save 시, Scriptable Object를 Save하지 않으며 Id를 통해 해당 클래스를 통해 접근
    /// </summary>
    public class ScriptableObjectManager : MonoBehaviour
    {
        private static ScriptableObjectManager _instance;
        public static ScriptableObjectManager Instance;

        [SerializeField] private List<ItemData> scriptableObjects;
        private Dictionary<string, ItemData> _idMap;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(this);
            }
            else
            {
                Destroy(this);
            }

            _idMap = new Dictionary<string, ItemData>();
            foreach (var itemData in scriptableObjects)
            {
                _idMap[itemData.id] = itemData;
            }
        }

        public ItemData GetScriptableObjectById(string id)
        {
            if (_idMap.TryGetValue(id, out var obj))
            {
                return obj;
            }

            Debug.LogWarning($"ScriptableObject with Id {id} not found!");
            return null;
        }
    }
}