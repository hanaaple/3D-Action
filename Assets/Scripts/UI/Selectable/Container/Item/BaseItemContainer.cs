using System;
using System.Collections.Generic;
using System.Linq;
using Data.Item.Base;
using Data.ViewModel;
using Manager;
using UI.Selectable.Slot;
using UI.View.Entity;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UI.Selectable.Container.Item
{
    public class ContainerRegion
    {
        public ContainerRegion(int width, int capacity, DescribeViewType itemDescribeType)
        {
            ItemDescribeType = itemDescribeType;
            StartIndex = -1;
            Width = width;
            Capacity = capacity;

            _items = new List<BaseItem>();
        }
        
        public DescribeViewType ItemDescribeType;
        public int StartIndex;
        public bool IsDirty;

        public int EndIndex => StartIndex + height - 1;
        public int Capacity { get; private set; }
        
        public readonly int Width;
        private readonly List<BaseItem> _items;
        
        public int count => _items.Count;
        public Vector2Int nextIndex => new(count % Width, StartIndex + count / Width);
        public int height => Capacity / Width;

        public Vector2Int GetIndex(int index) => new(index % Width, StartIndex + index / Width);
        
        public void Expand()
        {
            IsDirty = true;
            Capacity += Width;
        }
        
        public void Reduct()
        {
            IsDirty = true;
            Capacity -= Width;
        }
        
        public bool IsContainerFull()
        {
            return Capacity == count;
        }

        public bool IsContainerTooMuch()
        {
            return Capacity - count >= Width;
        }

        public bool IsEmpty()
        {
            return count == 0;
        }

        public List<BaseItem> GetAllItems()
        {
            return _items;
        }

        public void AddItem(BaseItem item)
        {
            _items.Add(item);
        }

        public void RemoveItem(BaseItem item)
        {
            _items.Remove(item);
        }

        public bool Contains(BaseItem item)
        {
            return _items.Contains(item);
        }
    }
    
    /// <summary>
    /// BaseInventoryView가 사용하는 Container
    /// ContainerType == Item.GetDetailType (string)
    /// </summary>
    public abstract class BaseItemContainer : GridSelectableContainer
    {
        // TODO: line 관련 코드 및 UI
        
        // 마우스 휠 기능
        
        // 근접 무기, 원거리 무기, 강화소재, 도구, 정보 등 다양하게 나뉨
        // Weapon, Accessory, Tool, Helmet, Bolt 등으로 나뉨
        // 나누는 기준 또한 제각각 마음대로 할 수 있다. (Equipment, Tool -> 던지기, 소모품, 재료, 버프 등)
        
        [FormerlySerializedAs("gridSize")] [SerializeField] private Vector2Int defaultGridSize;
        [SerializeField] private RectOffset padding;
        [SerializeField] private Vector2 space;
        
        [SerializeField] private GridLayoutGroup slotLayoutGroup;
        [SerializeField] private VerticalLayoutGroup lineLayoutGroup;
        [SerializeField] private GameObject slotPrefab;
        [SerializeField] private GameObject linePrefab;
        
        private List<GameObject> _lines = new ();
        private readonly SortedDictionary<string, ContainerRegion> _containers = new ();
        
        // 이거 세부 개수까지 들어가면 너무 많아진다.
        /// <summary>
        /// ContainerType으로 Owned Item을 구해온다. -> 이 과정에서 의존성이 발생
        /// ContainerType (Flag) Enum String과 OwnedItem (데이터 검색 가능하도록 의존성)
        /// </summary>
        /// <returns> Flag 일 수도, Flag가 아닐 수도 있다. 반드시 체크해야한다. </returns>
        public abstract Enum GetContainerEnumValue();
        
        public abstract string GetContainerName();
        
        // Equip -> 장착 중 + Equip 슬롯에 따라 체크
        // Inventory, Npc 판매 -> 장착 중인 경우 무조건 체크
        public abstract void UpdateCheck(EquipData equipData, bool isEquipped);

        // Container 내부에서 나누는 용도이다.
        // Container마다 내부를 나누는 기준을 다르게한다.
        // Sort by Name으로 하기 위해 string을 key로 사용. (큰 성능의 문제가 있을 것으로 보이진 않는다.)
        protected virtual string GetItemDivideType(BaseItem item)
        {
            var type = item.GetItemDetailType();
            return type;
        }

        protected void Initialize()
        {
            //Debug.LogWarning("Clear!!!!!!!!!!!!!!!!!!!!!!");
            gameObject.SetActive(false);
            selectableSlotRows.Clear();
            gameObject.name = $"{GetContainerEnumValue().ToString()} Item Container";

            CheckContainerSlotIsFit();
            SetGridLayout();
            SetSlotEnable();
            
            // Container에서 Item을 직접 채우는 것이 
            FillItems();
            CheckAllItems();

            foreach (var (_, container) in _containers)
            {
                UpdateContainerSlot(container);
            }
        }

        private void OnValidate()
        {
            if(slotLayoutGroup == null) return;

            SetGridLayout();
        }

        public void FillItems()
        {
            var items = GetInsertableOwnedItems();
            foreach (var item in items)
            {
                AddItem(item);
            }
        }

        public void CheckAllItems()
        {
            // UpdateCheck
            var equipViewModel = PlaySceneManager.instance.playerDataManager.equipViewModel;
            var equippedItems = equipViewModel.GetAllEquippedItems();

            CheckAllNone();

            foreach (var equippedItem in equippedItems)
            {
                if (!IsItemInsertable(equippedItem.Item)) continue;

                UpdateCheck(equippedItem, true);
            }
        }

        private void SetGridLayout()
        {
            // Update Slot Size
            var rectTransform = slotLayoutGroup.transform as RectTransform;

            var spaceLength = space.x * (defaultGridSize.x - 1);
            var width = rectTransform.rect.width - spaceLength; 
            
            var sideLength = width / defaultGridSize.x;

            slotLayoutGroup.padding = padding;
            slotLayoutGroup.spacing = space;
            slotLayoutGroup.cellSize = new Vector2(sideLength, sideLength);
            
            //float containerHeight = length + space.y;
            // lineLayoutGroup.spacing = containerHeight - linePrefab.height;
        }
        
        public void AddItem(BaseItem item)
        {
            if(item.IsNullOrBare()) return;
            
            var type = GetItemDivideType(item);
            
            if (!_containers.TryGetValue(type, out var container))
            {
                container = CreateContainer(type, item.describeType);
            }
            
            if (container.Contains(item)) return;
            
            if (container.IsContainerFull())
            {
                ExpandContainer(type);
            }

            SelectableItemSlot slot = GetEmptySlot(type);
            container.AddItem(item);
            slot.SetItem(item);
            //Debug.LogWarning($"Add Item {item.GetItemDisplayData()}");

            // Initialize slot (Select, DeSelect) -> 이거 어떻게 됐는지 확인 필요
            
            SortItem(type);
            
            SetSlotEnable();
        }

        public void RemoveItem(BaseItem item)
        {
            if (item.IsNullOrBare())
            {
                Debug.LogWarning("이거 맞나? 작동하면 어디서 작동하지");
            }
            
            var type = GetItemDivideType(item);

            if (!_containers.TryGetValue(type, out var container))
            {
                Debug.LogError($"{item.GetItemDisplayName()}의 Type {type}이 Container에 없습니다.");
                return;
            }

            if (!container.Contains(item))
            {
                Debug.LogWarning("없는 걸 없애려함.");
                return;
            }

            container.RemoveItem(item);
            var slot = FindSlot(item);
            slot.SetItem(null);
            
            Debug.LogWarning("Remove Item");
            
            if (container.IsEmpty())
            {
                DeleteContainer(type);
            }
            else if (container.IsContainerTooMuch())
            {
                ReductContainer(type);
            }
            
            SortItem(type);
            
            SetSlotEnable();
        }

        private ContainerRegion CreateContainer(string key, DescribeViewType itemDescribeType)
        {
            var tContainer = new ContainerRegion(defaultGridSize.x, defaultGridSize.x, itemDescribeType);
            _containers.Add(key, tContainer);
            
            UpdateContainer();

            return tContainer;
        }

        private void DeleteContainer(string key)
        {
            _containers.Remove(key);
            UpdateContainer();
        }

        private void CheckContainerSlotIsFit()
        {
            // selectableSlotRows.Count ~ GetContainerHeight() 생성, IsContainerLack()
            // if (selectableSlotRows.Count < maxHeight)
            if (IsContainerLack())
                InstantiateContainerSlot();
            // GetContainerHeight() ~ selectableSlotRows.Count 삭제, IsContainerTooMuch()
            // if (selectableSlotRows.Count > containersHeight && defaultGridSize.y < containersHeight)
            if (IsContainerTooMuch())
                DeleteContainerSlot();
        }

        private void ExpandContainer(string key)
        {
            _containers[key].Expand();
            UpdateContainer();
        }

        private void ReductContainer(string key)
        {
            if(!_containers[key].IsContainerTooMuch()) return;
            
            _containers[key].Reduct();
            UpdateContainer();
        }

        private void UpdateContainer()
        {
            // StartIndex 재설정
            // Index에 따른 Container Slot 생성 혹은 삭제 ContainerRegion에 관여 X
            // IsDirty인 Container에 대해 아이템 재설정
            // UpdateLine
            
            ShiftContainerIndex();
            CheckContainerSlotIsFit();
            SetContainerItems();
            UpdateLine();
            // foreach (var (key, containerRegion) in _containers)
            // {
            //     for (int y = containerRegion.StartIndex; y < containerRegion.EndIndex; y++)
            //     {
            //         for (int x = 0; x < containerRegion.Width; x++)
            //         {
            //             selectableSlotRows[y][x].Initialize(key);
            //         }
            //     }
            // }
        }
        
        private void InstantiateContainerSlot()
        {
            // 안전 체크
            if (!IsContainerLack()) return;
            
            int containersHeight = GetContainerHeight();
            int maxHeight = Math.Max(defaultGridSize.y, containersHeight);

            int startY = selectableSlotRows.Count;
            
            //Debug.LogWarning($"Create {startY} -> {maxHeight - 1}");
            
            //Debug.LogWarning($"{startY} ~ {maxHeight}  생성!!!!!!!!!!!!!!!!!!!!!!!!!");
            
            for (int y = startY; y < maxHeight; y++)
            {
                var selectableSlotRow = new SelectableSlotRow(defaultGridSize.x);
                for (int x = 0; x < defaultGridSize.x; x++)
                {
                    // selectableSlotRows[y].GetDetailType -> Container.key -> ConvertToDetailType
                    // Create Or ObjectPool Release
                    var slotInstance = Instantiate(slotPrefab, slotLayoutGroup.transform).GetComponent<SelectableInventorySlot>();
                    OnAddSelectableSlot(slotInstance);
                    selectableSlotRow.SetSlot(slotInstance, x);
                }
                
                // 그냥 Slot을 생성할때
                // TODO: line도 생성해야됨.
                // 기본 1개에서 시작, line을 어떻게 해야될지 ㄴㅇ뤔아루마ㅓㅇ루야ㅏ미느이ㅑㅏ믄
                //var line = Instantiate(linePrefab, lineLayoutGroup.transform);
                //_lines.Add(line);
            
                //Debug.LogWarning($"{gameObject.name} - Add SelectableSlot");
                selectableSlotRows.Add(selectableSlotRow);
            }

            var startYy = Math.Max(startY - 1, 0);
            for (int i = startYy; i < selectableSlotRows.Count; i++)
            {
                var row = selectableSlotRows[i]; 
                for (int x = 0; x < row.Length; x++)
                {
                    InitializeNavigation(i, x);
                }
            }
        }

        private void DeleteContainerSlot()
        {
            // 안전 체크
            if(!IsContainerTooMuch()) return;
            
            var containersHeight = GetContainerHeight();
            for (int i = containersHeight; i < selectableSlotRows.Count; i++)
            {
                foreach (var selectableSlot in selectableSlotRows[i].SelectableSlots)
                {
                    // Destroy Or ObjectPool Release
                    Destroy(selectableSlot.gameObject);
                    // TODO: line도 삭제해야됨.
                }
            }
            
            selectableSlotRows.RemoveRange(containersHeight, selectableSlotRows.Count - containersHeight);

            if (selectableSlotRows.Count > 0)
            {
                var y = selectableSlotRows.Count - 1;
                var lastRow = selectableSlotRows[^1];

                for (int x = 0; x < lastRow.Length; x++)
                {
                    InitializeNavigation(y, x);
                }
            }
        }
        
        private void SortItem(string key)
        {
            var container = _containers[key];
            var items = container.GetAllItems();
            items.Sort((item1, item2) => string.CompareOrdinal(item1.GetItemDisplayName(), item2.GetItemDisplayName()));

            
            for (var index = 0; index < items.Count; index++)
            {
                var item = items[index];
                var vector2Int = container.GetIndex(index);
                var selectableSlot = selectableSlotRows[vector2Int.y][vector2Int.x];
                //Debug.LogWarning($"{items.Count} {vector2Int.y}, {vector2Int.x}");
                (selectableSlot as SelectableItemSlot)?.SetItem(item);
            }
        }
        
        // 해당 메소드를 실행하는 위치가 효율적이지 않지만, 임시로 사용.
        private void SetSlotEnable()
        {
            // 모든걸 끄고 container에 대해서만 킨다?
            for (var y = 0; y < selectableSlotRows.Count; y++)
            {
                // 아이템 존재여부에 따라? 맞긴함
                // container의 count에 따라? default 빈칸도 있음.
                
                var row = selectableSlotRows[y];
                var defaultSlot = row.SelectableSlots[0] as SelectableItemSlot;
                var isEnable = defaultSlot.GetItem().IsNullOrEmpty() ? false : true;
                
                //if(isEnable)
                    //Debug.LogWarning($"{y}  {defaultSlot.GetItem().GetItemDisplayData()}");
                
                if(defaultSlot.GetItem().IsBare())
                    Debug.LogError("맨손, 맨몸이 있으면 안됨.");
                
                for (int x = 0; x < row.Length; x++)
                {
                    //Debug.LogWarning($"{x}, {y} Set Enable {isEnable}");
                    var slot = row[x];
                    slot.SetEnable(isEnable);
                    // 이때는 Slot이 Awake를 하지 않은 상태임.
                }
            }
        }
        
        private bool IsContainerLack()
        {
            int containersHeight = GetContainerHeight();
            int maxHeight = Math.Max(defaultGridSize.y, containersHeight);
            
            // Debug.Log($"{selectableSlotRows.Count}, {maxHeight}");
            if (selectableSlotRows.Count < maxHeight)
            {
                return true;
            }

            return false;
        }

        private bool IsContainerTooMuch()
        {
            int containersHeight = GetContainerHeight();

            if (selectableSlotRows.Count > containersHeight && defaultGridSize.y < containersHeight)
            {
                return true;
            }

            return false;
        }

        private void UpdateLine()
        {
            // TODO:
            // foreach (var line in lines)
            // {
            //     line.SetActive(false);
            // }
            //
            // foreach (var (_, container) in _containers)
            // {
            //     lines[container.StartIndex].SetActive(true);
            //     lines[container.EndIndex + 1].SetActive(true);
            // }
        }
        
        /// <summary>
        /// container.height (capacity, Width)를 기반으로 하여 container의 Index를 새롭게 변경한다.
        /// 즉, container가 생성 혹은 삭제, capacity가 변화하는 경우 실행해야된다.
        /// </summary>
        private void ShiftContainerIndex()
        {
            int nextStartIndex = 0;
            foreach (var (_, container) in _containers)
            {
                if (container.StartIndex != nextStartIndex)
                    container.IsDirty = true;

                container.StartIndex = nextStartIndex;

                if (container.IsDirty)
                    UpdateContainerSlot(container);

                //Debug.LogWarning($"{container.StartIndex} ~ {container.EndIndex}");

                nextStartIndex = container.EndIndex + 1;
            }
        }

        private void SetContainerItems()
        {
            foreach (var (_, container) in _containers)
            {
                if(!container.IsDirty) continue;
                container.IsDirty = false;
                
                var items = container.GetAllItems();

                //Debug.LogWarning(string.Join(", ", items.Select(item => item.GetItemName())));
                //Debug.LogWarning($"{container.count},   {container.Capacity}");
                
                //Debug.LogWarning($"{container.StartIndex} ~ {container.EndIndex}");

                for (int y = container.StartIndex; y <= container.EndIndex; y++)
                {
                    for (int i = 0; i < container.count; i++)
                    {
                        var x = i % container.Width;
                        //Debug.LogWarning($"{y}, {x}");
                        (selectableSlotRows[y][x] as SelectableItemSlot)?.SetItem(items[i]);
                    }

                    for (int i = container.count; i < container.Capacity; i++)
                    {
                        var x = i % container.Width;
                        //Debug.LogWarning($"{y}, {x}");
                        (selectableSlotRows[y][x] as SelectableItemSlot)?.SetItem(null);
                    }
                }
            }
        }

        private void UpdateContainerSlot(ContainerRegion container)
        {
            for (int y = container.StartIndex; y <= container.EndIndex; y++)
            {
                for (int i = 0; i < container.Capacity; i++)
                {
                    var x = i % container.Width;
                    var slot = selectableSlotRows[y][x] as SelectableItemSlot;
                    if (slot)
                        slot.DescribeType = container.ItemDescribeType;
                    else
                        Debug.LogWarning($"Container에 DescribeType을 넣을 수 없음.");
                }
            }
        }

        /// <summary>
        /// Must be GetEmpty Before Add Container
        /// </summary>
        private SelectableItemSlot GetEmptySlot(string key)
        {
            var container = _containers[key];
            //Debug.LogWarning($"{container.nextIndex.y}, {container.nextIndex.x}");
            return selectableSlotRows[container.nextIndex.y][container.nextIndex.x] as SelectableItemSlot;
        }

        protected SelectableInventorySlot FindSlot(BaseItem item)
        {
            var type = GetItemDivideType(item);
            var container = _containers[type];
            for (int i = container.StartIndex; i <= container.EndIndex; i++)
            {
                var row = selectableSlotRows[i];

                var selectableSlot = row.SelectableSlots
                    .Cast<SelectableInventorySlot>()
                    .First(slot => slot.GetItem() == item);

                if (selectableSlot != null)
                {
                    return selectableSlot;
                }
            }

            return null;
        }
        
        private int GetContainerHeight()
        {
            if (_containers.Count == 0) return 0;
            
            var prevContainer = _containers.Last().Value;
            var containersHeight = prevContainer.EndIndex + 1;
            return containersHeight;
        }

        public void CheckAllNone()
        {
            var selectableInventorySlots = GetSelectableSlots().Cast<SelectableInventorySlot>();   
            foreach (var selectableSlot in selectableInventorySlots)
            {
                selectableSlot.Check(CheckType.None);
            }
        }
        
        /// <summary>
        /// Item을 해당 Container에 넣을 수 있는가
        /// </summary>
        public bool IsItemInsertable(BaseItem item)
        {
            if (item.IsNullOrBare()) return false;
            
            var containerType = GetContainerEnumValue();

            if (IsFlagsEnum(containerType))
            {
                return Enum.GetValues(containerType.GetType())
                    .Cast<Enum>()
                    .Where(enumValue => containerType.HasFlag(enumValue))
                    .Any(enumValue => item.Equals(enumValue.ToString()));
            }
            else
            {
                return item.Equals(containerType.ToString());
            }
        }

        private List<BaseItem> GetInsertableOwnedItems()
        {
            var ownedItemViewModel = PlaySceneManager.instance.playerDataManager.ownedItemViewModel;
            var containerEnumValue = GetContainerEnumValue();
            
            if (IsFlagsEnum(containerEnumValue))
            {
                List<BaseItem> targetItems = new List<BaseItem>();
                var containerType = containerEnumValue.GetType();
                foreach (Enum enumValue in Enum.GetValues(containerType))
                {
                    if (!containerEnumValue.HasFlag(enumValue)) continue;

                    var comparision = enumValue.ToString();

                    // ItemContainerType, ItemType등 다양한 Type을 하나로 수렴시켜야됨.
                    //Debug.LogWarning($"toString(): {comparision} is working right?");

                    var items = ownedItemViewModel.GetItems(comparision);
                    targetItems.AddRange(items);

                }
                return targetItems;
            }
            else
            {
                var comparision = containerEnumValue.ToString();
                //Debug.LogWarning($"toString(): {comparision} is working right?");

                var items = ownedItemViewModel.GetItems(comparision);

                return items;
            }
        }

        public static bool IsFlagsEnum(Enum enumValue)
        {
            var enumType = enumValue.GetType();
            return enumType.IsEnum && Attribute.IsDefined(enumType, typeof(FlagsAttribute));
        }
    }
}