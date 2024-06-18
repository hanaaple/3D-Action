using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Interaction.Base;

namespace Interaction
{
    public sealed class InteractionUIModel
    {
        private bool isInteractable;

        private int focusIndex;
        
        private List<IInteractable> closeInteractables = new();

        public bool IsInteractable
        {
            get => isInteractable;
            set
            {
                if (isInteractable == value) return;
                isInteractable = value;
                OnPropertyChanged();
            }
        }
        
        public int FocusIndex
        {
            get => focusIndex;
            set
            {
                if (focusIndex == value) return;
                focusIndex = value;
                OnPropertyChanged();
            }
        }

        public IReadOnlyList<IInteractable> CloseInteractables
        {
            get => closeInteractables;
        }

        public void AddInteractable(IInteractable interactable)
        {
            closeInteractables.Add(interactable);
            OnPropertyChanged();
        }
        
        public void RemoveInteractable(IInteractable interactable)
        {
            closeInteractables.Remove(interactable);
            OnPropertyChanged();
        }

        public void SortInteractable(Comparison<IInteractable> comparison)
        {
            closeInteractables.Sort(comparison);
            OnPropertyChanged();
        }
        
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}