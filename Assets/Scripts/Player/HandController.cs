using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class HandController : MonoBehaviour
    {
        [Header("Spark")]
        [SerializeField] private GameObject _spark;
        [SerializeField] private float _sparkSpeed = 5f;
        [SerializeField, Range(0.001f, 1f)] private float _sparkInertia = 1f;
        [Header("Hand")]
        [SerializeField] private SpriteRenderer _handSprite;
        [SerializeField] private Sprite _handOpenSprite;
        [SerializeField] private Sprite _handClosedSprite;

        private Vector2 _targetDirection;
        private Vector2 _currentDirection;
        private LineRenderer _handSparkLine;

        private void Awake()
        {
            _handSparkLine = _handSprite.GetComponent<LineRenderer>();
        }

        private void Update()
        {
            CalculateDirection();
            MoveSpark();
            DrawLine();
        }

        public void Move(InputAction.CallbackContext ctx)
        {
            Vector2 input = ctx.ReadValue<Vector2>();
            print(input.ToString("0.000"));
            _targetDirection = input;
        }

        public void Take(InputAction.CallbackContext ctx)
        {
            print(ctx.phase.ToString());
            if (ctx.phase == InputActionPhase.Performed)
            {
                CloseHand();
            }

            if (ctx.phase == InputActionPhase.Canceled)
            {
                OpenHand();
            }
        }

        private void CalculateDirection()
        {
            if (_currentDirection != _targetDirection)
            {
                _currentDirection = Vector2.MoveTowards(_currentDirection, _targetDirection, 1 / _sparkInertia);
            }
        }

        private void MoveSpark()
        {
            _spark.transform.Translate(_currentDirection * (_sparkSpeed * Time.deltaTime));
        }

        private void DrawLine()
        {
            _handSparkLine.SetPosition(0, _handSprite.transform.position);
            _handSparkLine.SetPosition(1, _spark.transform.position);
        }

        private void CloseHand()
        {
            _handSprite.sprite = _handClosedSprite;
        }

        private void OpenHand()
        {
            _handSprite.sprite = _handOpenSprite;
        }
    }
}
