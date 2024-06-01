using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Model
{
    // 장비, 스탯에 따른 계산 값  ex - (default + 스탯 * 1 + 악세 * 2)
    // hp, mp, Sp, 공격력, 방어력, 장비 중량 등    
    // 계산되는 데이터

    //현재 플레이어 데이터?

    [Serializable]
    public class PlayerData : INotifyPropertyChanged
    {
        [SerializeField] private int attack;
        [SerializeField] private int defense;
        [SerializeField] private int healthPoint;
        [SerializeField] private int manaPoint;
        [SerializeField] private int staminaPoint;
        [SerializeField] private int equipWeight;

        [SerializeField] private int maxHealthPoint;
        [SerializeField] private int maxManaPoint;
        [SerializeField] private int maxStaminaPoint;
        [SerializeField] private int maxEquipWeight;

        public event PropertyChangedEventHandler PropertyChanged;

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

        public int HealthPoint
        {
            get => healthPoint;
            set => SetField(ref healthPoint, value);
        }

        public int ManaPoint
        {
            get => manaPoint;
            set => SetField(ref manaPoint, value);
        }

        public int StaminaPoint
        {
            get => staminaPoint;
            set => SetField(ref staminaPoint, value);
        }

        public int EquipWeight
        {
            get => equipWeight;
            set => SetField(ref equipWeight, value);
        }

        public int MaxHealthPoint
        {
            get => maxHealthPoint;
            set => SetField(ref maxHealthPoint, value);
        }

        public int MaxManaPoint
        {
            get => maxManaPoint;
            set => SetField(ref maxManaPoint, value);
        }

        public int MaxStaminaPoint
        {
            get => maxStaminaPoint;
            set => SetField(ref maxStaminaPoint, value);
        }

        public int MaxEquipWeight
        {
            get => maxEquipWeight;
            set => SetField(ref maxEquipWeight, value);
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