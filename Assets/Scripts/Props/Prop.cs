using System;
using UnityEngine;

namespace Props
{
    public class Prop : MonoBehaviour
    {
        protected Rigidbody2D _rigidbody;
        private FixedJoint2D _fixedJoint;
        private TagHandle _groundTag;
        private bool _inFreeFall;
        
        [field:SerializeField]
        public bool IsGrounded { get; protected set; }
        public event Action<Prop> PropDeactivated;

        protected virtual void OnEnable()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _rigidbody.simulated = true;
            _fixedJoint = GetComponent<FixedJoint2D>();
            _groundTag = TagHandle.GetExistingTag("Ground");
        }

        protected virtual void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag(_groundTag) || 
                collision.gameObject.GetComponent<Prop>()?.IsGrounded == true)
            {
                if (!IsGrounded)
                {
                    OnDeactivated();
                }
            }
        }
        
        public virtual void Fix(Rigidbody2D parent)
        {
            _fixedJoint.connectedBody = parent;
            _fixedJoint.enabled = true;
            _inFreeFall = false;
        }

        public virtual void Release(Vector2 velocityModifiers, float throwTorqueMoment = 1)
        {
            _fixedJoint.connectedBody = null;
            _fixedJoint.enabled = false;
            
            // multiply velocity
            _rigidbody.linearVelocity *= velocityModifiers;
            _rigidbody.AddTorque(_rigidbody.linearVelocity.magnitude * throwTorqueMoment *
                                 Mathf.Sign(transform.position.x - _rigidbody.transform.position.x));
            
            _inFreeFall = true;
        }

        protected virtual void OnDeactivated()
        {
            IsGrounded = true;
            _inFreeFall = false;
            PropDeactivated?.Invoke(this);
        }
    }
}
