using System;
using UnityEngine;

namespace Props
{
    public class PotionBottle : Prop
    {
        [SerializeField] private Transform _potion;
        [SerializeField] private Rigidbody2D _crackedBottleBot;
        [SerializeField] private Rigidbody2D _crackedBottleTop;
        [SerializeField] private Rigidbody2D _spilledPotion;
        [SerializeField, Range(1f, 100f)] private float _potionLiquidity = 25f;
        [SerializeField, Range(0f, 1f)] private float _potionInertia = 0.1f;
        [SerializeField] private float _ultimateImpact = 20f;
        [SerializeField] private float _spilledPotionStartSpeed = 4f;
        private Vector2 _prevVelocity = Vector2.zero;

        private void Awake()
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

        protected override void OnCollisionEnter2D(Collision2D collision)
        {
            Vector2 impact = _rigidbody.mass * _rigidbody.linearVelocity;
            if (impact.magnitude > _ultimateImpact)
            {
                OnDeactivated();
                OnCracked();
                return;
            }
            base.OnCollisionEnter2D(collision);
        }

        private void OnCracked()
        {
            Vector2 forceUp = (_rigidbody.linearVelocity.normalized + (Vector2)transform.up) *
                          _rigidbody.linearVelocity.magnitude / 2;
            Vector2 forceDown = (_rigidbody.linearVelocity.normalized - (Vector2)transform.up) *
                              _rigidbody.linearVelocity.magnitude / 2;
            
            _crackedBottleBot.transform.parent = transform.parent;
            _crackedBottleTop.transform.parent = transform.parent;
            _spilledPotion.transform.parent = transform.parent;
            
            Destroy(gameObject);
            
            _crackedBottleBot.gameObject.SetActive(true);
            _crackedBottleBot.AddForce(forceDown, ForceMode2D.Impulse);
            _crackedBottleTop.gameObject.SetActive(true);
            _crackedBottleTop.AddForce(forceUp, ForceMode2D.Impulse);
            _spilledPotion.gameObject.SetActive(true);
            _spilledPotion.transform.up = Vector2.up;
            _spilledPotion.linearVelocity = -Vector2.up * _spilledPotionStartSpeed;
        }

        protected override void OnDeactivated()
        {
            base.OnDeactivated();
            OnCracked();
        }
    }
}