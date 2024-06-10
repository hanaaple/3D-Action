using System.ComponentModel;
using System.Runtime.CompilerServices;
using UI.Entity.Selectable.Slot;

namespace ViewModel
{
    // 선택된 Selectable ViewModel
    public sealed class SelectedUIViewModel : INotifyPropertyChanged
    {
        private SelectableItemSlot _selectedItemSlotDataData;

        public SelectableItemSlot selectedItemSlotData
        {
            get => _selectedItemSlotDataData;
            set
            {
                if (_selectedItemSlotDataData == value)
                {
                    return;
                }
                
                _selectedItemSlotDataData = value;
                OnPropertyChanged();
            }
        }
        
        // 아이템 이름
        // 아이템 효과 (방어구, 도구 등) 종류에 따라 다름
        //      소비 HP, 소비 FP
        //      입수 정보
        //      필요 능력치
        //      공격력, 방어 시 방어력
        //      부가 효과
        //      중량
        //      자세히 보기
        //      
        
        // 소모품, 재사옹 가능
        // 소지 개수 / 최대 개수
        // 능력 보정
        
        public event PropertyChangedEventHandler PropertyChanged;

        public void Initialize()
        {
            
            // selectedItemSlotData.PropertyChanged += (send, e) => OnPropertyChanged(e.PropertyName);
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}