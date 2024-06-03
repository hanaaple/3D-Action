using System.ComponentModel;
using System.Runtime.CompilerServices;
using Model;

namespace ViewModel
{
    public class OwnedItemViewModel : INotifyPropertyChanged
    {
        private OwnedItemData _ownedItemData;
        public event PropertyChangedEventHandler PropertyChanged;
        
        public void Initialize(OwnedItemData ownedItemData)
        {
            _ownedItemData = ownedItemData;

            _ownedItemData.PropertyChanged += (send, e) => OnPropertyChanged(e.PropertyName);
        }
        
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}