using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Data.Play;
using UnityEngine;

namespace Data.ViewModel
{
    public enum StaminaUseType
    {
        Roll,
        Run,
        Attack
    }
    
    // 플레이어 데이터(HP, MP, 공격력 등) ViewModel
    public sealed class CharacterDataViewModel : INotifyPropertyChanged
    {
        private CharacterData _characterData;
        
        public event PropertyChangedEventHandler PropertyChanged;
        
        public float Attack
        {
            get => _characterData.attack;
            set => SetField(ref _characterData.attack, value);
        }

        public float Defense
        {
            get => _characterData.defense;
            set => SetField(ref _characterData.defense, value);
        }

        public float HealthPoint
        {
            get => _characterData.healthPoint;
            set
            {
                value = Mathf.Clamp(value, 0, MaxHealthPoint);
                SetField(ref _characterData.healthPoint, value);
            }
        }

        public float ManaPoint
        {
            get => _characterData.manaPoint;
            set
            {
                value = Mathf.Clamp(value, 0, MaxManaPoint);
                SetField(ref _characterData.manaPoint, value);
            }
        }

        // public void IncreaseStaminaPoint(float decreaseStaminaPoint)
        // {
        //     
        // }
        //
        public void DecreaseStaminaPoint(float decreaseStaminaPoint, StaminaUseType staminaUseType)
        {
            StaminaPoint -= decreaseStaminaPoint;
        }

        public float StaminaPoint
        {
            get => _characterData.staminaPoint;
            set
            {
                value = Mathf.Clamp(value, 0, MaxStaminaPoint);
                SetField(ref _characterData.staminaPoint, value);
            }
        }

        public float EquipWeight
        {
            get => _characterData.equipWeight;
            set => SetField(ref _characterData.equipWeight, value);
        }

        public float MaxHealthPoint
        {
            get => _characterData.maxHealthPoint;
            set
            {
                SetField(ref _characterData.maxHealthPoint, value);
                
                if (HealthPoint > MaxHealthPoint)
                    HealthPoint = MaxHealthPoint;
            }
        }

        public float MaxManaPoint
        {
            get => _characterData.maxManaPoint;
            set
            {
                SetField(ref _characterData.maxManaPoint, value);
                if (ManaPoint > MaxManaPoint)
                    ManaPoint = MaxManaPoint;
            }
        }

        public float MaxStaminaPoint
        {
            get => _characterData.maxStaminaPoint;
            set
            {
                SetField(ref _characterData.maxStaminaPoint, value);
                if (StaminaPoint > MaxStaminaPoint)
                    StaminaPoint = MaxStaminaPoint;
            }
        }

        public float MaxEquipWeight
        {
            get => _characterData.maxEquipWeight;
            set => SetField(ref _characterData.maxEquipWeight, value);
        }
        
        public float PoiseHealthPoint
        {
            get => _characterData.poiseHealthPoint;
            set
            {
                value = Mathf.Clamp(value, 0, _characterData.poiseHealthPoint);
                SetField(ref _characterData.poiseHealthPoint, value);
            }
        }

        public float MaxPoiseHealthPoint
        {
            get => _characterData.maxPoiseHealthPoint;
            set
            {
                SetField(ref _characterData.maxPoiseHealthPoint, value);
                if (PoiseHealthPoint > MaxPoiseHealthPoint)
                    PoiseHealthPoint = MaxPoiseHealthPoint;
            }
        }

        public float StaminaRecoveryWeight
        {
            get => _characterData.staminaRecoveryWeight;
            set => SetField(ref _characterData.staminaRecoveryWeight, value);
        }

        public float PoiseHealthRecoveryWeight
        {
            get => _characterData.poiseHealthRecoveryWeight;
            set => SetField(ref _characterData.poiseHealthRecoveryWeight, value);
        }

        public CharacterData playerData => _characterData;

        public CharacterDataViewModel(CharacterData characterData, EquipViewModel equipViewModel, StatusViewModel statusViewModel)
        {
            _characterData = characterData;

            equipViewModel.PropertyChanged += UpdateData;
            statusViewModel.PropertyChanged += UpdateData;
        }
        
        public void RecoveryAll()
        {
            UpdateData(null, null);
            
            HealthPoint = MaxHealthPoint;
            ManaPoint = MaxManaPoint;
            StaminaPoint = MaxStaminaPoint;
            PoiseHealthPoint = MaxPoiseHealthPoint;
        }

        internal void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        private void UpdateData(object sender, PropertyChangedEventArgs e)
        {
            // TODO: 
            // Update의 비용이 생각보다 큼.
            // 데이터를 분리해서 옵저빙 해야될 필요가 있음.
            
            
            if (PlayDataManager.instance == null) return;
            
            // var statusViewModel = DataManager.instance.playerStatusViewModel;
            // var playerEquipViewModel = DataManager.instance.playerEquipViewModel;
            //
            // var statusTable = DataManager.instance.statusTable;
            // var playerDefaultData = DataManager.instance.playerDefaultData;
            //
            // var statusTuple = new[]
            // {
            //     (statusViewModel.Strength, "strength"), (statusViewModel.Constitution, "constitution"),
            //     (statusViewModel.Spirit, "spirit"), (statusViewModel.Stamina, "stamina")
            // };
            //
            // attack = playerDefaultData.Data["attack"];
            // defense = playerDefaultData.Data["defense"];
            // maxHealthPoint = playerDefaultData.Data["maxHealthPoint"];
            // maxManaPoint = playerDefaultData.Data["maxManaPoint"];
            // maxStaminaPoint = playerDefaultData.Data["maxStaminaPoint"];
            // maxEquipWeight = playerDefaultData.Data["maxEquipWeight"];
            // maxPoiseHealthPoint = playerDefaultData.Data["maxPoiseWeight"];
            //
            // staminaRecoveryWeight = playerDefaultData.Data["staminaRecoveryWeight"];
            // poiseHealthRecoveryWeight = playerDefaultData.Data["poiseHealthRecoveryWeight"];
            //
            //
            // Debug.Log($"Update Character Data, {playerDefaultData.Data["maxHealthPoint"]}");
            //
            //
            // foreach (var valueTuple in statusTuple)
            // {
            //     attack += valueTuple.Item1 * statusTable.Table["attack"][valueTuple.Item2];
            // }
            //
            // foreach (var valueTuple in statusTuple)
            // {
            //     maxHealthPoint += valueTuple.Item1 * statusTable.Table["maxHealthPoint"][valueTuple.Item2];
            // }
            //
            // foreach (var valueTuple in statusTuple)
            // {
            //     maxManaPoint += valueTuple.Item1 * statusTable.Table["maxManaPoint"][valueTuple.Item2];
            // }
            //
            // foreach (var valueTuple in statusTuple)
            // {
            //     maxStaminaPoint += valueTuple.Item1 * statusTable.Table["maxStaminaPoint"][valueTuple.Item2];
            // }
            //
            // foreach (var valueTuple in statusTuple)
            // {
            //     maxEquipWeight += valueTuple.Item1 * statusTable.Table["maxEquipWeight"][valueTuple.Item2];
            // }
            //
            // foreach (var valueTuple in statusTuple)
            // {
            //     maxPoiseHealthPoint += valueTuple.Item1 * statusTable.Table["maxPoiseWeight"][valueTuple.Item2];
            // }
            //
            // var helmetStaticData = (ArmorStaticData)playerEquipViewModel.helmet.GetItemData();
            // var breastplateStaticData = (ArmorStaticData)playerEquipViewModel.breastplate.GetItemData();
            // var leggingsStaticData = (ArmorStaticData)playerEquipViewModel.leggings.GetItemData();
            // var shoesStaticData = (ArmorStaticData)playerEquipViewModel.shoes.GetItemData();
            //
            // defense += helmetStaticData.defense
            //            + breastplateStaticData.defense
            //            + leggingsStaticData.defense
            //            + shoesStaticData.defense;
            //
            // maxPoiseHealthPoint += helmetStaticData.poise +
            //                        breastplateStaticData.poise +
            //                        leggingsStaticData.poise +
            //                        shoesStaticData.poise;
            //
            // equipWeight += helmetStaticData.weight +
            //               breastplateStaticData.weight +
            //               leggingsStaticData.weight +
            //               shoesStaticData.weight;

            OnPropertyChanged();
        }
    }
}