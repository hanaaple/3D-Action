using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Data.PlayItem;

namespace Data.Play
{
    // 보유 중인 장비 데이터
    [Serializable]
    public class OwnedItemData : INotifyPropertyChanged
    {
        public List<Weapon> weapons;
        public List<Armor> armors;
        public List<Accessory> accessories;
        public List<Tool> tools;
        public List<Valuable> valuables;

        [field: NonSerialized] public event PropertyChangedEventHandler PropertyChanged;

        public OwnedItemData()
        {
            weapons = new List<Weapon>();
            armors = new List<Armor>();
            accessories = new List<Accessory>();
            tools = new List<Tool>();
            valuables = new List<Valuable>();
        }
        
        /// <summary>
        /// 추가하는 아이템은 복사 후 가져올 것
        /// </summary>
        /// <param name="item"></param>
        public void AddItem(Item item)
        {
            switch (item)
            {
                case Weapon weapon:
                    weapons.Add(weapon);
                    break;
                case Armor armor:
                    armors.Add(armor);
                    break;
                case Accessory accessory:
                    accessories.Add(accessory);
                    break;
                case Tool tool:
                    var existTool = tools.Find(item => item.GetItemData() == tool.GetItemData());
                    if (existTool != null)
                    {
                        if (tool.toolType == ToolType.Expendables)
                        {
                            existTool.possessionCount++;
                        }
                    }
                    else
                    {
                        tools.Add(tool);
                    }

                    break;
                case Valuable valuable:
                    if (!valuables.Contains(valuable))
                    {
                        valuables.Add(valuable);
                    }
                    break;
            }

            OnPropertyChanged();
        }

        public void RemoveItem<T>() where T : Item
        {
            // 모든 아이템은 그대로 존재해
            // 단, 개수가 0인것은 화면에 보이지 않아
            // 나쁘지 않은데
            
            
            // Tool - 소모품 or 재사용 가능 등에 따라 중복 허가
            // Accessory - 중복 불허
            // Valuable - 중복 불허
            // Armor - 중복 불허
            // Weapon - 중복 불허
            
            OnPropertyChanged();
        }

        public virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}