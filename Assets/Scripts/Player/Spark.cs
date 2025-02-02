using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class Spark : MonoBehaviour
    {
        [Header("Spark")]
        [SerializeField] private float _sparkSpeed = 5f;
        [SerializeField, Range(1f, 100f)] private float _sparkInertia = 20f;

        private Vector2 _targetDirection;
        private Vector2 _currentDirection;
        private LineRenderer _handSparkLine;
        private Transform _handTransform;

        private void Awake()
        {
            _handSparkLine = GetComponent<LineRenderer>();
            _handTransform = transform.parent.GetComponentInChildren<Hand>().transform;
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
            _targetDirection = input;
        }
        
        private void CalculateDirection()
        {
            if (_currentDirection != _targetDirection)
            {
                _currentDirection = Vector2.MoveTowards(_currentDirection, _targetDirection, 1f / _sparkInertia);
            }
        }

        private void MoveSpark()
        {
            transform.Translate(_currentDirection * (_sparkSpeed * Time.deltaTime));
        }

        private void DrawLine()
        {
            _handSparkLine.SetPosition(0, _handTransform.position);
            _handSparkLine.SetPosition(1, transform.position);
        }
    }
}
