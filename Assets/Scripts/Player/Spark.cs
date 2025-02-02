using Services;
using UnityEngine;

namespace Player
{
    public class Spark : MonoBehaviour
    {
        [Header("Spark")]
        [SerializeField] private Side _side;
        [SerializeField] private float _sparkSpeed = 5f;
        [SerializeField, Range(1f, 100f)] private float _sparkInertia = 20f;
        [SerializeField] private float _maxDistance = 5f;

        private Vector2 _targetDirection;
        private Vector2 _currentDirection;
        private LineRenderer _handSparkLine;
        private Transform _handTransform;

        private void Awake()
        {
            _handSparkLine = GetComponent<LineRenderer>();
            _handTransform = transform.parent.GetComponentInChildren<Hand>().transform;
        }

        private void OnEnable()
        {
            if (_side == Side.Left)
            {
                InputProvider.OnMoveLeft += Move;
            }
            else
            {
                InputProvider.OnMoveRight += Move;
            }
        }

        private void OnDisable()
        {
            if (_side == Side.Left)
            {
                InputProvider.OnMoveLeft -= Move;
            }
            else
            {
                InputProvider.OnMoveRight -= Move;
            }
        }

        private void Update()
        {
            CalculateDirection();
            MoveSpark();
            DrawLine();
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = _side == Side.Left ? Color.cyan : Color.magenta;
            Gizmos.DrawWireSphere(transform.parent.position, _maxDistance);
        }

        private void Move(Vector2 input)
        {
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
            Vector2 offsetFromStart = transform.position - transform.parent.position;
            if (offsetFromStart.magnitude > _maxDistance)
            {
                transform.position = (Vector2)transform.parent.position + offsetFromStart.normalized * _maxDistance;
            }
        }

        private void DrawLine()
        {
            _handSparkLine.SetPosition(0, _handTransform.position);
            _handSparkLine.SetPosition(1, transform.position);
        }
    }
}
