using System;
using System.ComponentModel;
using Data.Item.Base;
using Manager;
using Player;
using TMPro;
using UI.Base;
using UI.Selectable.Container;
using UI.Selectable.Container.Item;
using UnityEngine;
using UnityEngine.UI;

namespace UI.View.Inventory
{
    // Equip Change
    // 인벤토리 (Weapon, Armor(Helmet, Shoes, ...) -> 1종류씩만   (아이템 종류에 따라 줄 나눔)
    
    // Npc merchant (Sale, Upgrade)
    // Upgrade -> 기본 인벤토리 View(Weapon, ...) -> Upgrade 가능한 아이템을 Display
    // Sale -> 기본 인벤토리 View -> 판매 가능한 아이템
    
    
    public enum IndexingDirection
    {
        Previous,
        Next
    }
    
    /// <summary>
    /// 역할: Container Input, Indexing
    /// 추가 구현 필요
    /// 1. Describe View(매번 다름)
    /// 2. 슬라이드 여부
    /// 3-1. 각 Container에서 Display를 어떻게 할 것인가
    /// 3-2. 각 Slot에 대한 Action을 어떻게 할 것인가
    ///
    /// UI Input (Container Indexing)을 관리한다.
    /// </summary>
    
    // View의 책임
    // 상단 -> Static View 이름 및 Icon
    // 각 Container를 생성, 관리 (+ 슬라이드 아이콘을 넣던 zx 입력키만 넣던)
    
    // ???의 것
    // 각 Slot Item의 이름
    // 각 Slot을 어떻게 Display할건지 (개수, 가능 불가능, 착용 중, 가격 등)
    // 각 Slot을 생성, 관리
    // 각 Slot을 눌렀을 때 해야되는 Action
    
    // Container의 책임
    // -- Container 이름
    // 각 Item을 나누는 기준
    // 마우스 휠
    // -- 현재 Select 중인 Item의 이름
    
    // ???의 책임
    // 현재 Select 중인 Slot의 Describe View (아이템 존재 여부와 상관없이 Slot에 따라 변경)
    
    public abstract class BaseItemContainerView : UIContainerEntity
    {
        [SerializeField] protected TMP_Text containerNameText;
        [SerializeField] protected TMP_Text itemNameText;
        
        [SerializeField] private GameObject describeViewPanel;
        
        [Header("Container")]
        [SerializeField] protected GameObject containerPrefab;
        [SerializeField] protected Transform containerParent;
        
        //[Header("Slide")]
        //[SerializeField] private Slider containerSlider;
        // Container와 연동.
        // 1. 마우스 휠 사용 시, 슬라이딩 된다.
        // 2. 마우스 드래그시 -> ??
        // 3. 슬라이드에 자동으로 동기화.
        
        protected int ContainerIndex;
        
        // StateMachine으로 사용?
        protected abstract BaseItemContainer[] GetItemContainers();

        protected override void Awake()
        {
            base.Awake();
            
            // Awake 시 아이템을 채우고, Awake ~ Destroy까지 Observe한다.
            //Debug.LogWarning("Awake check !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
            PlaySceneManager.instance.playerDataManager.ownedItemViewModel.PropertyChanged += UpdateInventoryView;
            PlaySceneManager.instance.BindPlayerData(ViewModelType.Equip, UpdateEquippedSlotView);
            PrimitiveUIManager.instance.selectedUiViewModel.PropertyChanged += OnSelectedItemChange;
        }

        private void OnDestroy()
        {
            PlaySceneManager.instance.playerDataManager.ownedItemViewModel.PropertyChanged -= UpdateInventoryView;
            PlaySceneManager.instance.UnBindPlayerData(ViewModelType.Equip, UpdateEquippedSlotView);
            PrimitiveUIManager.instance.selectedUiViewModel.PropertyChanged -= OnSelectedItemChange;
        }

        private void OnSelectedItemChange(object o, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            if (PrimitiveUIManager.instance.selectedUiViewModel.selectedItemSlot == null) return;
            
            itemNameText.text = PrimitiveUIManager.instance.selectedUiViewModel.selectedItemSlot.GetItemName();
        }

        protected virtual void Start()
        {
            var uiInput = GetUIInput();
            uiInput.SlotLeft = () => IndexingContainer(IndexingDirection.Previous);
            uiInput.SlotRight = () => IndexingContainer(IndexingDirection.Next);
        }

        protected override SelectableSlotContainer GetCurrentContainer()
        {
            //Debug.Log($"{GetItemContainers().Length}  {ContainerIndex}");
            return GetItemContainers()[ContainerIndex];
        }
        
        // Disable인 상태에서 AddItem이 되면? -> 이거다!
        
        // OnAwake -> Init & Observe
        // OnDestroy -> 
        
        // 아니면 킬때마다 각 UI Container에서 아이템을 추가, 삭제하고    아이템 장착 여부를 Check함? 
        private void UpdateInventoryView(BaseItem item, bool isAdded)
        {
            // View에서 Inventory를 업데이트
            
            foreach (var inventoryContainer in GetItemContainers())
            {
                if (!inventoryContainer.IsItemInsertable(item)) continue;
                
                if (isAdded)
                {
                    inventoryContainer.AddItem(item);
                }
                else
                {
                    inventoryContainer.RemoveItem(item);
                }
            }
        }
        
        // 성능에 큰 문제가 발생하지 않아, 전체 순회
        private void UpdateEquippedSlotView(object s, PropertyChangedEventArgs e)
        {
            var equipViewModel = PlaySceneManager.instance.playerDataManager.equipViewModel;
            var equippedItems = equipViewModel.GetAllEquippedItems();
            
            foreach (var inventoryContainer in GetItemContainers())
            {
                inventoryContainer.CheckAllNone();

                foreach (var equippedItem in equippedItems)
                {
                    if (!inventoryContainer.IsItemInsertable(equippedItem.Item)) continue;
                    
                    inventoryContainer.UpdateCheck(equippedItem, true);
                }
            }
        }

        public override void OpenOrLoad()
        {
            base.OpenOrLoad();
            
            describeViewPanel.SetActive(true);
            
            // TODO: Load Each Container x, y position?
        }

        public override void Close(bool isSelectClear = true)
        {
            GetCurrentContainer().gameObject.SetActive(false);
            describeViewPanel.SetActive(false);
            
            ContainerIndex = 0;
            
            base.Close(isSelectClear);
        }

        protected virtual void IndexingContainer(IndexingDirection indexingDirection)
        {
            var inventoryContainers = GetItemContainers();
            
            var nextIndex = indexingDirection switch
            {
                IndexingDirection.Previous => (ContainerIndex - 1 + inventoryContainers.Length) % inventoryContainers.Length,
                IndexingDirection.Next => (ContainerIndex + 1) % inventoryContainers.Length,
                _ => ContainerIndex
            };
            
            SetIndex(nextIndex);
        }

        // 마우스 올려서 Container를 바꿀때도 이거로 -> 마우스를 올림. 해당 Index로 변경.
        // Select가 될때 슬라이드를 미는 것은 자동으로 되도록.
        // EquipChange를 한번 연다음 아이템을 열고 다시 EquipChange를 열면 아이템이 안보임.
        
        protected virtual void SetIndex(int nextIndex)
        {
            var inventoryContainers = GetItemContainers();
            
            inventoryContainers[ContainerIndex].gameObject.SetActive(false);
            ContainerIndex = nextIndex;
            inventoryContainers[ContainerIndex].gameObject.SetActive(true);
            
            // 인덱싱 말고도 
            containerNameText.text = inventoryContainers[ContainerIndex].GetContainerName();
        }
    }
}