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
        [SerializeField, Range(1f, 100f)] private float _potionLiquidity = 10f;
        [SerializeField, Range(0f, 1f)] private float _potionInertia = 0.4f;
        [SerializeField] private float _ultimateImpact = 1f;
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

        protected override void OnCollisionEnter2D(Collision2D collision)
        {
            // Vector2 impact = _rigidbody.mass * _rigidbody.linearVelocity -
            //                  collision.otherRigidbody.mass * _rigidbody.linearVelocity;
            Vector2 impact = _rigidbody.mass * _rigidbody.linearVelocity;
            print("Impact: " + impact.magnitude);
            if (impact.magnitude > _ultimateImpact)
            {
                OnDeactivated();
                OnCracked();
                
                print("Cracked: " + name);
                return;
            }
            base.OnCollisionEnter2D(collision);
        }

        private void OnCracked()
        {
            Vector2 forceUp = (_rigidbody.linearVelocity.normalized + (Vector2)transform.up) *
                          _rigidbody.linearVelocity.magnitude;
            Vector2 forceDown = (_rigidbody.linearVelocity.normalized - (Vector2)transform.up) *
                              _rigidbody.linearVelocity.magnitude;
            
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
            _spilledPotion.AddForce(Vector2.down * 4, ForceMode2D.Impulse);
        }

        protected override void OnDeactivated()
        {
            base.OnDeactivated();
            OnCracked();
        }
    }
}