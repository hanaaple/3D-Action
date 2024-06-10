using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Data.Play
{
    /// <summary>
    /// 저장되는 데이터
    /// 근력, 지구력, 체력, 정신력, 레벨 및 exp
    /// 추가 체력, 추가 mp 등
    /// </summary>
    [Serializable]
    public class StatusData : INotifyPropertyChanged
    {
        [SerializeField] private int constitution;
        [SerializeField] private int spirit;
        [SerializeField] private int strength;
        [SerializeField] private int stamina;
        [SerializeField] private int level;
        [SerializeField] private int experiencePoint;

        [field: NonSerialized] public event PropertyChangedEventHandler PropertyChanged;
        
        public int Constitution
        {
            get => constitution;
            set => SetField(ref constitution, value);
        }

        public int Spirit
        {
            get => spirit;
            set => SetField(ref spirit, value);
        }

        public int Strength
        {
            get => strength;
            set => SetField(ref strength, value);
        }

        public int Stamina
        {
            get => stamina;
            set => SetField(ref stamina, value);
        }

        public int Level
        {
            get => level;
            set => SetField(ref level, value);
        }

        public int ExperiencePoint
        {
            get => experiencePoint;
            set => SetField(ref experiencePoint, value);
        }

        public virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
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
    }
}