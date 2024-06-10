using System.ComponentModel;
using UI.Entity.Base;

namespace UI.View
{
    /// <summary>
    /// System View
    /// </summary>
    public class SystemView : UIContainerEntity
    {
        protected override void UpdateView()
        {
            UpdateUiView(null, null);
        }

        private void UpdateUiView(object sender, PropertyChangedEventArgs e)
        {
            if (!IsUpdateEnable()) return;
        }
    }
}