namespace UI.Base
{
    // Input을 변경할때마다 Input Action 및
    public class UIInput
    {
        public delegate void InputAction();
        public delegate bool InputActionEnable();
        
        public bool Enable = true;
        
        public InputAction Decision;    // e
        public InputAction RightArrow;  // right
        public InputAction LeftArrow;   // left
        public InputAction DownArrow;   // down
        public InputAction UpArrow;     // up
        public InputAction SlotLeft;    // z
        public InputAction SlotRight;   // x
        
        public InputActionEnable IsDecisionActive;
        public InputActionEnable IsRightArrowActive;
        public InputActionEnable IsLeftArrowActive;
        public InputActionEnable IsDownArrowActive;
        public InputActionEnable IsUpArrowActive;
        public InputActionEnable IsSlotLeftActive;
        public InputActionEnable IsSlotRightActive;

        public bool IsDecisionInterrupt = true;
        
        public bool IsRightArrowInterrupt = true;
        public bool IsLeftArrowInterrupt = true;
        public bool IsDownArrowInterrupt = true;
        public bool IsUpArrowInterrupt = true;
        
        public bool IsSlotLeftInterrupt = true;
        public bool IsSlotRightInterrupt = true;
    }
}