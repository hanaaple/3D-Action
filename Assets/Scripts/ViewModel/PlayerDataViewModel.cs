using System.ComponentModel;
using System.Runtime.CompilerServices;
using Model;

namespace ViewModel
{
    // 플레이어 데이터(HP, MP, 공격력 등) ViewModel
    public class PlayerDataViewModel : INotifyPropertyChanged
    {
        private PlayerData _playerData;
        private EquippedItemData _equippedItemData;
        private StatusData _statusData;
        
        public event PropertyChangedEventHandler PropertyChanged;
        
        public int Attack
        {
            get => _playerData.Attack;
            set => _playerData.Attack = value;
        }

        public int Defense
        {
            get => _playerData.Defense;
            set => _playerData.Defense = value;
        }

        public int HealthPoint
        {
            get => _playerData.HealthPoint;
            set => _playerData.HealthPoint = value;
        }

        public int ManaPoint
        {
            get => _playerData.ManaPoint;
            set => _playerData.ManaPoint= value;
        }

        public int StaminaPoint
        {
            get => _playerData.StaminaPoint;
            set => _playerData.StaminaPoint = value;
        }

        public int EquipWeight
        {
            get => _playerData.EquipWeight;
            set => _playerData.EquipWeight = value;
        }

        public int MaxHealthPoint
        {
            get => _playerData.MaxHealthPoint;
            set => _playerData.MaxHealthPoint = value;
        }

        public int MaxManaPoint
        {
            get => _playerData.MaxManaPoint;
            set => _playerData.MaxManaPoint = value;
        }

        public int MaxStaminaPoint
        {
            get => _playerData.MaxStaminaPoint;
            set => _playerData.MaxStaminaPoint = value;
        }

        public int MaxEquipWeight
        {
            get => _playerData.MaxEquipWeight;
            set => _playerData.MaxEquipWeight = value;
        }
        
        // EquippedItemData -> PlayerDataChange
        // StatusDataChange -> PlayerDataChange
        public void Initialize(PlayerData playerData, EquippedItemData equippedItemData, StatusData statusData)
        {
            _playerData = playerData;
            _equippedItemData = equippedItemData;
            _statusData = statusData;
            
            _equippedItemData.PropertyChanged += (send, e) => OnPropertyChanged(e.PropertyName);
            _statusData.PropertyChanged += (send, e) => OnPropertyChanged(e.PropertyName);

            _equippedItemData.PropertyChanged += (send, e) => UpdatePlayerData();

            statusData.PropertyChanged += (send, e) => UpdatePlayerData();
        }

        private void UpdatePlayerData()
        {
            // Attack = _equippedItemData.rights[_equippedItemData.RightIndex].attack + _statusData.Strength * 1;
            // Stamina = 
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}