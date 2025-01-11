using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Data.Play;

namespace Data.ViewModel
{
    // 플레이어 스탯 ViewModel
    public sealed class StatusViewModel : INotifyPropertyChanged
    {
        private StatusData _statusData;
        public event PropertyChangedEventHandler PropertyChanged;
      
        public int Level
        {
            get => _statusData.level;
            set => SetField(ref _statusData.level, value);
        }

        public int ExperiencePoint
        {
            get => _statusData.experiencePoint;
            set => SetField(ref _statusData.experiencePoint, value);
        }
        
        public int Vitality
        {
            get => _statusData.vitality;
            set => SetField(ref _statusData.vitality, value);
        }

        public int Spirit
        {
            get => _statusData.spirit;
            set => SetField(ref _statusData.spirit, value);
        }
        
        public int Endurance
        {
            get => _statusData.endurance;
            set => SetField(ref _statusData.endurance, value);
        }

        public int Strength
        {
            get => _statusData.strength;
            set => SetField(ref _statusData.strength, value);
        }
        
        public int Workmanship
        {
            get => _statusData.workmanship;
            set => SetField(ref _statusData.workmanship, value);
        }
        
        public int Intellect
        {
            get => _statusData.intellect;
            set => SetField(ref _statusData.intellect, value);
        }
        
        public int RequiredExperiencePoint
        {
            get
            {
                if (IsLevelUpPossible())
                {
                    return StaticDataCollector.instance.LevelUpTable.GetRequiredExp(_statusData.level);    
                }

                return -1;
            }
        }

        public StatusData statusData => _statusData;

        public StatusViewModel(StatusData statusData)
        {
            _statusData = statusData;
        }

        public bool IsLevelUpPossible()
        {
            if (StaticDataCollector.instance.LevelUpTable == null)
                return false;
            
            return StaticDataCollector.instance.LevelUpTable.CanLevelUp(_statusData.level);
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
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
    }
}