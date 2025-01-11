using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Data.Play
{
    [Serializable]
    public class MonsterData: INotifyPropertyChanged
    {
        [SerializeField] private int attack;
        [SerializeField] private int defense;
        [SerializeField] private int poise;
        
        [SerializeField] private int healthPoint;
        [SerializeField] private int poiseHealthPoint;

        [SerializeField] private int maxHealthPoint;
        [SerializeField] private int maxPoiseHealthPoint;
        
        
        [field: NonSerialized] public event PropertyChangedEventHandler PropertyChanged;

        public int Attack
        {
            get => attack;
            set => SetField(ref attack, value);
        }

        public int Defense
        {
            get => defense;
            set => SetField(ref defense, value);
        }
        
        public int Poise
        {
            get => poise;
            set => SetField(ref poise, value);
        }
        
        public int HealthPoint
        {
            get => healthPoint;
            set => SetField(ref healthPoint, value);
        }
        
        public int MaxHealthPoint
        {
            get => maxHealthPoint;
            set => SetField(ref maxHealthPoint, value);
        }

        public int PoiseHealthPoint
        {
            get => poiseHealthPoint;
            set => SetField(ref poiseHealthPoint, value);
        }
        
        public int MaxPoiseHealthPoint
        {
            get => maxPoiseHealthPoint;
            set => SetField(ref maxPoiseHealthPoint, value);
        }
        
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        public void UpdateData(object sender, PropertyChangedEventArgs e)
        {
            // DataManager.instance.statusData 기반으로
            // 스탯 계산
            OnPropertyChanged();
        }
    }
}