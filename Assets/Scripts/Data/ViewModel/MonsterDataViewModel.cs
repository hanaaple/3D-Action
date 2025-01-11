using System.ComponentModel;
using System.Runtime.CompilerServices;
using Data.Play;

namespace Data.ViewModel
{
    public class MonsterDataViewModel : INotifyPropertyChanged
    {
        // attack, defense, healthPoint, poiseHealthPoint

        private MonsterData _monsterData;

        public event PropertyChangedEventHandler PropertyChanged;

        public int Attack
        {
            get => _monsterData.Attack;
            set => _monsterData.Attack = value;
        }

        public int Defense
        {
            get => _monsterData.Defense;
            set => _monsterData.Defense = value;
        }

        public int Poise
        {
            get => _monsterData.Poise;
            set => _monsterData.Poise = value;
        }
        
        public int HealthPoint
        {
            get => _monsterData.HealthPoint;
            set => _monsterData.HealthPoint = value;
        }

        public int MaxHealthPoint
        {
            get => _monsterData.MaxHealthPoint;
            set => _monsterData.MaxHealthPoint = value;
        }

        public int PoiseHealthPoint
        {
            get => _monsterData.PoiseHealthPoint;
            set => _monsterData.PoiseHealthPoint = value;
        }

        public int MaxPoiseHealthPoint
        {
            get => _monsterData.MaxPoiseHealthPoint;
            set => _monsterData.MaxPoiseHealthPoint = value;
        }
        
        public void Initialize(MonsterData monsterData)
        {
            _monsterData = monsterData;
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}