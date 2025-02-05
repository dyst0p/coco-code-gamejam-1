using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Services
{
    public enum Side
    {
        Left,
        Right,
        None
    }
    public class InputProvider : MonoBehaviour
    {
        public static event Action<Vector2> OnMoveLeft;
        public static event Action<bool> OnCatchLeft;
        public static event Action<Vector2> OnMoveRight;
        public static event Action<bool> OnCatchRight;
        
        public void MoveLeftHandler(InputAction.CallbackContext ctx)
        {
            if (Time.timeScale == 0) 
                return;
            OnMoveLeft?.Invoke(ctx.ReadValue<Vector2>());
        }
        
        public void CatchLeftHandler(InputAction.CallbackContext ctx)
        {
            if (Time.timeScale == 0) 
                return;
            OnCatchLeft?.Invoke(ctx.phase == InputActionPhase.Performed);
        }
        
        public void MoveRightHandler(InputAction.CallbackContext ctx)
        {
            if (Time.timeScale == 0) 
                return;
            OnMoveRight?.Invoke(ctx.ReadValue<Vector2>());
        }
        
        public void CatchRightHandler(InputAction.CallbackContext ctx)
        {
            if (Time.timeScale == 0) 
                return;
            OnCatchRight?.Invoke(ctx.phase == InputActionPhase.Performed);
        }
    }
}