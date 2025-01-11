using System.Collections.Generic;
using System.Linq;
using Interaction.Base;
using UI.View.Play;
using UnityEngine;

namespace Interaction
{
    public enum IndexMoveType
    {
        Previous,
        Next
    }

    /// <summary>
    /// 인터랙션 UI 관리 Class
    /// </summary>
    public sealed class InteractionUIController
    {
        // 변수 및 로직이 난잡하다.
        private int _focusIndex;
        private bool _isInteractable;
        private bool _isInteractableListDirty;

        private readonly List<IInteractable> _closeInteractableList = new();
        
        private readonly Transform _playerTransform;
        private readonly InteractionBaseUIView _interactionBaseUIView;

        public InteractionUIController(Transform transform, InteractionBaseUIView interactionBaseUIView)
        {
            _playerTransform = transform;
            _interactionBaseUIView = interactionBaseUIView;
        }

        public void OnEnable()
        {
            TryUpdateOrder();
            _interactionBaseUIView.UpdateUI(GetIsInteractableExist(), _closeInteractableList.Count > 1);
            _interactionBaseUIView.UpdateTextContext(GetFocusedInteractable());
            _interactionBaseUIView.UpdateTextColor(_isInteractable);
        }

        public void Clear()
        {
            _focusIndex = 0;
            _isInteractable = false;
            _isInteractableListDirty = false;

            _closeInteractableList.Clear();
            
            _interactionBaseUIView.UpdateUI(false, false);
        }
        
        public void UpdateInteractableOrderAndIndex()
        {
            if (TryUpdateOrder())
            {
                _interactionBaseUIView.UpdateTextContext(GetFocusedInteractable());
            }
        }
        
        public void AddInteractable(IInteractable interactable)
        {
            if (_closeInteractableList.Contains(interactable))
                return;

            _isInteractableListDirty = true;
            _closeInteractableList.Add(interactable);

            OnChangeList();
        }

        public void RemoveInteractable(IInteractable interactable)
        {
            if (!_closeInteractableList.Contains(interactable)) return;

            _isInteractableListDirty = true;
            _closeInteractableList.Remove(interactable);

            OnChangeList();
        }

        public void SetFocusIndex(IndexMoveType indexMoveType)
        {
            if (_closeInteractableList.Count == 1) return;

            if (indexMoveType == IndexMoveType.Previous)
            {
                _focusIndex = (_closeInteractableList.Count + _focusIndex - 1) % _closeInteractableList.Count;
            }
            else if (indexMoveType == IndexMoveType.Next)
            {
                _focusIndex = (_focusIndex + 1) % _closeInteractableList.Count;
            }
            
            _interactionBaseUIView.UpdateTextContext(GetFocusedInteractable());
        }

        // On Try Interact -> Check Enable
        public void SetInteractionEnable(bool isInteractable)
        {
            if (_isInteractable == isInteractable) return;

            _isInteractable = isInteractable;
            
            _interactionBaseUIView.UpdateTextColor(_isInteractable);
        }

        public bool GetIsInteractableExist()
        {
            return _closeInteractableList.Count > 0;
        }

        /// <summary>
        /// 현재 display 중인 Interaction
        /// </summary>
        public IInteractable GetFocusedInteractable()
        {
            if (_closeInteractableList.Count == 0) return null;
            
            if (_closeInteractableList.Count <= _focusIndex)
            {
                Debug.LogError(
                    $"Interactable - Index OverFlow,  {string.Join(", ", _closeInteractableList.Select(item => item.GetName()))}");
                return null;
            }

            return _closeInteractableList[_focusIndex];
        }
        
        private bool TryUpdateOrder()
        {
            // 여기서 제대로 Update가 안된듯
            if (_closeInteractableList.Count <= 1)
            {
                _focusIndex = 0;
                return false;
            }
            
            // UI를 변경한 상태에서 List에 변경이 없는 경우
            if (_focusIndex != 0 && !_isInteractableListDirty) return false;
            
            // 거리에 따른 UI 초기화
            _focusIndex = 0;
            _isInteractableListDirty = false;

            _closeInteractableList.Sort((a, b) =>
            {
                var distanceA = Vector3.Distance(_playerTransform.position, a.GetPosition());
                var distanceB = Vector3.Distance(_playerTransform.position, b.GetPosition());
                return distanceA.CompareTo(distanceB);
            });
            
            return true;
        }

        // Interaction Count에 따라 Update
        // Sort Order
        private void OnChangeList()
        {
            // Debug.Log("OnChangeList");
            _interactionBaseUIView.UpdateUI(GetIsInteractableExist(), _closeInteractableList.Count > 1);
            TryUpdateOrder();
            _interactionBaseUIView.UpdateTextContext(GetFocusedInteractable());
        }
    }
}