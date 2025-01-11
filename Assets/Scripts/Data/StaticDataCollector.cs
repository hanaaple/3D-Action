using Data.Static;
using UnityEngine;

namespace Data
{
    /// <summary>
    /// 
    /// </summary>
    public class StaticDataCollector : MonoBehaviour
    {
        private static StaticDataCollector _instance;
        public static StaticDataCollector instance => _instance;
    
        [SerializeField] private TextAsset levelUpTableCsv;
        [SerializeField] private TextAsset statusPerWeightTableCsv;
        [SerializeField] private TextAsset playerDefaultDataCsv;

        public LevelUpTable LevelUpTable;
        public StatusTable StatusTable;
        public PlayerDefaultData PlayerDefaultData;

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(_instance);
            }
            else
            {
                DestroyImmediate(_instance);
            }
        
            Initialize();
        }

        public void Initialize()
        {
            LevelUpTable = new LevelUpTable(levelUpTableCsv.text);
            StatusTable = new StatusTable(statusPerWeightTableCsv.text);
            PlayerDefaultData = new PlayerDefaultData(playerDefaultDataCsv.text);
        }
    }
}