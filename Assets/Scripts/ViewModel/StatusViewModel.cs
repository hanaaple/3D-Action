using System.ComponentModel;
using System.Runtime.CompilerServices;
using Data;
using Data.Play;

namespace ViewModel
{
    // 플레이어 스탯 ViewModel
    public sealed class StatusViewModel : INotifyPropertyChanged
    {
        private StatusData _statusData;
        
        public event PropertyChangedEventHandler PropertyChanged;
        
        public int Constitution
        {
            get => _statusData.Constitution;
            set => _statusData.Constitution = value;
        }

        public int Spirit
        {
            get => _statusData.Spirit;
            set => _statusData.Spirit = value;
        }

        public int Strength
        {
            get => _statusData.Strength;
            set => _statusData.Strength = value;
        }

        public int Stamina
        {
            get => _statusData.Stamina;
            set => _statusData.Stamina = value;
        }

        public int Level
        {
            get => _statusData.Level;
            set => _statusData.Level = value;
        }

        public int ExperiencePoint
        {
            get => _statusData.ExperiencePoint;
            set => _statusData.ExperiencePoint = value;
        }
        
        public int RequiredExperiencePoint
        {
            get
            {
                if (IsLevelUpPossible())
                {
                    return DataManager.instance.LevelUpTable.GetRequiredExp(_statusData.Level);    
                }

                return -1;
            }
        }

        public void Initialize(StatusData statusData)
        {
            _statusData = statusData;
            _statusData.PropertyChanged += (send, e) => OnPropertyChanged(e.PropertyName);
        }

        public bool IsLevelUpPossible()
        {
            if (DataManager.instance.LevelUpTable == null)
                return false;
            
            return DataManager.instance.LevelUpTable.CanLevelUp(_statusData.Level);
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}