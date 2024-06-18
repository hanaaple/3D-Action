using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Interaction.Base;
using UnityEngine;

namespace Interaction
{
    public sealed class InteractionUIViewModel : INotifyPropertyChanged
    {
        private InteractionUIModel _interactionUIModel;
        private Transform _playerTransform;

        public event PropertyChangedEventHandler PropertyChanged;

        public void Initialize(Transform transform)
        {
            _interactionUIModel = new InteractionUIModel();
            _playerTransform = transform;

            _interactionUIModel.PropertyChanged += (sender, e) => OnPropertyChanged(e.PropertyName);
        }
        
        public void AddInteractable(IInteractable interactable)
        {
            _interactionUIModel.AddInteractable(interactable);
            UpdateInteractableOrderAndIndex();
        }

        public bool TryRemove(IInteractable interactable)
        {
            if (_interactionUIModel.CloseInteractables.Contains(interactable))
            {
                _interactionUIModel.RemoveInteractable(interactable);
                UpdateInteractableOrderAndIndex();
                return true;
            }

            return false;
        }
        
        public void SetFocusIndexPrevious()
        {
            _interactionUIModel.FocusIndex =
                (_interactionUIModel.CloseInteractables.Count + _interactionUIModel.FocusIndex - 1) % _interactionUIModel.CloseInteractables.Count;
        }

        public void SetFocusIndexNext()
        {
            _interactionUIModel.FocusIndex =
                (_interactionUIModel.FocusIndex + 1) % _interactionUIModel.CloseInteractables.Count;
        }
        
        public int GetInteractableCount()
        {
            return _interactionUIModel.CloseInteractables.Count;
        }

        public bool IsInteractableExist()
        {
            return _interactionUIModel.CloseInteractables.Count > 0;
        }

        public bool GetInteractionEnable()
        {
            return _interactionUIModel.IsInteractable;
        }
        
        public void SetInteractionEnable(bool isInteractable)
        {
            _interactionUIModel.IsInteractable = isInteractable;
        }
        
        public IInteractable GetFocusedInteractable()
        {
            if (_interactionUIModel.CloseInteractables.Count <= _interactionUIModel.FocusIndex)
            {
                Debug.LogError($"Interactable - Index OverFlow,  {string.Join(", ", _interactionUIModel.CloseInteractables.Select(item => item.GetName()))}");
                return null;
            }
            
            return _interactionUIModel.CloseInteractables[_interactionUIModel.FocusIndex];
        }

        public void UpdateInteractableOrderAndIndex()
        {
            if (_interactionUIModel.CloseInteractables.Count <= 1) return;
            
            _interactionUIModel.SortInteractable((a, b) =>
            {
                var distanceA = Vector3.Distance(_playerTransform.position, a.GetPosition());
                var distanceB = Vector3.Distance(_playerTransform.position, b.GetPosition());
                return distanceA.CompareTo(distanceB);
            });
        }
        
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}