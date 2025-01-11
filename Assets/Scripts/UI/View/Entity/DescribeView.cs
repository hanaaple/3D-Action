using System.Collections.Generic;
using System.ComponentModel;
using Data.Item.Base;
using UI.View.Describe;
using UnityEngine;

namespace UI.View.Entity
{
    // DescribeView 클래스를 추가하는 경우 Enum 추가 후 구현.
    public enum DescribeViewType
    {
        Weapon,
        Armor,
        Accessory,
        Tool,
    }

    public class DescribeView : MonoBehaviour
    {
        public BaseViewState[] describeViews;
        
        private BaseViewState _currentView;
        
        private readonly Dictionary<DescribeViewType, BaseViewState> _stateMachine = new();

        private void Awake()
        {
            foreach (var describeView in describeViews)
            {
                _stateMachine.Add(describeView.describeViewType, describeView);
            }
        }

        private void OnEnable()
        {
            var selectedUiViewModel = PrimitiveUIManager.instance.selectedUiViewModel;
            selectedUiViewModel.PropertyChanged += UpdateUI;

            UpdateUI(null, null);
        }

        private void OnDisable()
        {
            var selectedUiViewModel = PrimitiveUIManager.instance.selectedUiViewModel;
            selectedUiViewModel.PropertyChanged -= UpdateUI;
        }

        private void UpdateUI(object sender, PropertyChangedEventArgs e)
        {
            var describeViewModel = PrimitiveUIManager.instance.selectedUiViewModel;
            var selectedSlot = describeViewModel.selectedItemSlot;
            if(selectedSlot == null) return;

            var targetItem = selectedSlot.GetItem();
            if (targetItem.IsNullOrEmpty())
            {
                ChangeState(selectedSlot.describeType);
            }
            else
            {
                // targetItem.DescribeType는 Default가 있고 특수가 있는 경우 바뀜 (내부적으로)
                ChangeState(targetItem.describeType);
            }

            _currentView.UpdateSelect(targetItem);
        }

        private void ChangeState(DescribeViewType describeViewType)
        {
            if (_currentView == _stateMachine[describeViewType])
                return;
            
            _currentView?.OnStateExit();
            _currentView = _stateMachine[describeViewType];
            _currentView.OnStateEnter();
        }
    }
}