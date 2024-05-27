using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif
public class InputStateHandler : MonoBehaviour
{
    [Header("Character Input Values")]
    public Vector2 move;
    public Vector2 look;
    public bool roll;
    // public bool jump;
    public bool run;
    public bool crouch;
    public bool lockOnOff;
    public bool attack;
    
    [Header("Movement Settings")]
    public bool analogMovement;
    
    [Header("Mouse Cursor Settings")]
    public bool cursorLocked = true;
    public bool cursorInputForLook = true;
    
#if ENABLE_INPUT_SYSTEM
    public void OnMove(InputValue value)
    {
        MoveInput(value.Get<Vector2>());
    }

    public void OnLook(InputValue value)
    {
        if (cursorInputForLook)
        {
            LookInput(value.Get<Vector2>());
        }
    }

    public void OnRun(InputValue value)
    {
        RunInput(value.isPressed);
    }

    public void OnJump(InputValue value)
    {
        RollInput(value.isPressed);
    }
    
    public void OnCrouch(InputValue value)
    {
        CrouchInput(value.isPressed);
    }

    public void OnLock(InputValue value)
    {
        LockInput(value.isPressed);
    }

    public void OnAttack(InputValue value)
    {
        AttackInput(value.isPressed);
    }
#endif


    private void MoveInput(Vector2 newMoveDirection)
    {
        move = newMoveDirection;
    }
    
    private void LookInput(Vector2 newLookDelta)
    {
        look = newLookDelta;
    }
    
    private void RunInput(bool newRunState)
    {
        run = newRunState;
    }
    
    private void RollInput(bool newRollState)
    {
        roll = newRollState;
    }
    
    private void CrouchInput(bool newCrouchState)
    {
        crouch = newCrouchState;
    }
    
    private void LockInput(bool newLockState)
    {
    }
    
    private void AttackInput(bool newAttackState)
    {
    }
    
    private void OnApplicationFocus(bool hasFocus)
    {
        SetCursorState(cursorLocked);
    }

    private static void SetCursorState(bool newState)
    {
        Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
    }
}