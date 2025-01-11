
namespace UI.Title
{
    public class TitleUIManager : UIManager
    {
        // Input을 어떻게 막아야 될까
        
        protected override void Awake()
        {
            base.Awake();
            Push(rootBaseUIEntity);
        }

        protected override void UpdateState()
        {
        }
    }
}