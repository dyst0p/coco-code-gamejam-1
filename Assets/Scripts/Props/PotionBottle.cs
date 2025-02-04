using System;
using UnityEngine;

namespace Props
{
    public class PotionBottle : Prop
    {
        [SerializeField] private Transform _potion;
        [SerializeField, Range(1f, 100f)] private float _potionLiquidity = 10f;
        [SerializeField, Range(0f, 1f)] private float _potionInertia = 0.4f;
        private Vector2 _prevVelocity = Vector2.zero;

        protected void Awake()
        {
            _potion.transform.up = Vector3.up;
        }

        private void FixedUpdate()
        {
            Vector2 deltaVelocity = _rigidbody.linearVelocity - _prevVelocity;
            _prevVelocity = _rigidbody.linearVelocity;
            Vector2 newUp = (Vector2.down * Physics2D.gravity.y + deltaVelocity * _potionLiquidity).normalized;
            _potion.transform.up = Vector3.Slerp(_potion.transform.up, newUp, _potionInertia);
        }
    }
}