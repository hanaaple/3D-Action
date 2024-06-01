using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ViewModel
{
    // SelectedViewModel
    public class DescribeViewModel : INotifyPropertyChanged
    {
        // Item.Item item;
        
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
            // .PropertyChanged += (send, e) => OnPropertyChanged(e.PropertyName);
        }
        
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}