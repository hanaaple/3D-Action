using Character;
using Data;
using Data.Play;
using Data.ViewModel;
using Save;
using UnityEngine;
using Util;

namespace Monster
{
    // 각종 효과 및 구현 (공격, 방어, 죽음 등)
    
    // AI, Mesh, Animator, 효과 및 구현, 능력치, 세이브종류(잡몹, 엘리트, 보스)
    
    
    // 몬스터 종류와 상태에 따라 Instance화 당함.
    // 몬스터는 세이브 가능한이지, 항상 세이브하면 안됨.
    
    
    public class MonsterDataManager : CharacterDataManager
    {
        public TextAsset monsterCsvData;
        
        private MonsterManager _monsterManager;
        private MonsterDataViewModel _monsterDataViewModel;
        
        // For Debug
        [SerializeField] private MonsterData monsterData;

        private void Awake()
        {
            _monsterManager = GetComponent<MonsterManager>();
            
            // LoadOrCreateData();
        }

        // public override void CreateData()
        // {
        //     monsterData = CsvReader.ReadData<MonsterData>(monsterCsvData.text);
        //     InitializeViewModel();
        // }
        //
        // // Load Or Create Data
        // public override void LoadData(ISaveData saveData)
        // {
        //     if (saveData is MonsterSaveData monsterSaveData)
        //     {
        //         monsterData = monsterSaveData.monsterData;
        //         InitializeViewModel();
        //     }
        //     else
        //     {
        //         monsterData = CsvReader.ReadData<MonsterData>(monsterCsvData.text);
        //         InitializeViewModel();
        //     }
        // }
        //
        // public override string GetLabel()
        // {
        //     // 이 녀석은 어떻게 저장되는게 옳을까.
        //     
        // }

        // Load Or Create Data
        // public override void LoadOrCreateData()
        // {
        //     if (SaveManager.IsAlreadyLoaded())
        //     {
        //         var saveData = SaveManager.Load();
        //         var data = saveData.GetSaveData(GetLabel(), GetId());
        //
        //         LoadData(data);
        //     }
        //     else
        //     {
        //         CreateData();
        //     }
        // }
        
        private void InitializeViewModel()
        {
            _monsterDataViewModel = new MonsterDataViewModel();
            _monsterDataViewModel.Initialize(monsterData);
        }

        // public override ISaveData GetSaveData()
        // {
        //     MonsterSaveData monsterSaveData = new MonsterSaveData
        //     {
        //         monsterData = monsterData
        //     };
        //     return monsterSaveData;
        // }

        public bool GetIsDead()
        {
            if (_monsterDataViewModel.HealthPoint <= 0)
            {
                return true;
            }
            
            return false;
        }
    }
}