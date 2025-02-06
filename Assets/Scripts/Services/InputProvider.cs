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
            if (PlayerData.IsGamePaused) 
                return;
            OnMoveLeft?.Invoke(ctx.ReadValue<Vector2>());
        }
        
        public void CatchLeftHandler(InputAction.CallbackContext ctx)
        {
            if (PlayerData.IsGamePaused) 
                return;
            OnCatchLeft?.Invoke(ctx.phase == InputActionPhase.Performed);
        }
        
        public void MoveRightHandler(InputAction.CallbackContext ctx)
        {
            if (PlayerData.IsGamePaused) 
                return;
            OnMoveRight?.Invoke(ctx.ReadValue<Vector2>());
        }
        
        public void CatchRightHandler(InputAction.CallbackContext ctx)
        {
            if (PlayerData.IsGamePaused) 
                return;
            OnCatchRight?.Invoke(ctx.phase == InputActionPhase.Performed);
        }
    }
}