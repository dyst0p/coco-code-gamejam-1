using JesToxic.Services;
using UnityEngine;
using UnityEngine.Serialization;

namespace JesToxic.Player
{
    public class HandTarget : MonoBehaviour
    {
        [SerializeField] private Side _side;
        [FormerlySerializedAs("_sparkSpeed")] [SerializeField] private float _speed = 5f;
        [FormerlySerializedAs("_sparkInertia")] [SerializeField, Range(1f, 100f)] private float _inertia = 20f;
        [SerializeField] private float _maxDistance = 5f;

        private Vector2 _targetDirection;
        private Vector2 _currentDirection;

        private void OnEnable()
        {
            if (_side == Side.Left)
            {
                InputProvider.OnMoveLeft += HandleInput;
            }
            else
            {
                InputProvider.OnMoveRight += HandleInput;
            }
        }

        private void OnDisable()
        {
            if (_side == Side.Left)
            {
                InputProvider.OnMoveLeft -= HandleInput;
            }
            else
            {
                InputProvider.OnMoveRight -= HandleInput;
            }
        }

        private void FixedUpdate()
        {
            CalculateDirection();
            Move();
        }

        private void HandleInput(Vector2 input)
        {
            _targetDirection = input;
        }
        
        private void CalculateDirection()
        {
            if (_currentDirection != _targetDirection)
            {
                _currentDirection = Vector2.MoveTowards(_currentDirection, _targetDirection, 1f / _inertia);
            }
        }

        private void Move()
        {
            transform.Translate(_currentDirection * (_speed * Time.fixedDeltaTime));
            Vector2 offsetFromStart = transform.position - transform.parent.position;
            if (offsetFromStart.magnitude > _maxDistance)
            {
                transform.position = (Vector2)transform.parent.position + offsetFromStart.normalized * _maxDistance;
            }
        }
    }
}
