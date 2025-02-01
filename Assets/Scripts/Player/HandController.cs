using UnityEngine;
using UnityEngine.InputSystem;

public class HandController : MonoBehaviour
{
    [SerializeField] private float _sparkSpeed = 5f;
    [SerializeField, Range(0.001f, 1f)] private float _sparkInertia = 1f;
    [SerializeField] private GameObject _spark;

    private Vector2 _targetDirection;
    private Vector2 _currentDirection;

    private void Update()
    {
        CalculateDirection();
        MoveSpark();
    }

    public void Move(InputAction.CallbackContext ctx)
    {
        Vector2 input = ctx.ReadValue<Vector2>();
        print(input.ToString("0.000"));
        _targetDirection = input;
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
}
