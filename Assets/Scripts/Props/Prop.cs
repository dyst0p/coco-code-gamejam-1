using System;
using UnityEngine;

namespace Props
{
    public class Prop : MonoBehaviour
    {
        private Rigidbody2D _rigidbody;
        private FixedJoint2D _fixedJoint;
        private TagHandle _groundTag;
        private bool _inFreeFall;
        
        [field:SerializeField]
        public bool IsGrounded { get; private set; }
        public event Action<Prop> PropGrounded;

        private void OnEnable()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _rigidbody.simulated = true;
            _fixedJoint = GetComponent<FixedJoint2D>();
            _groundTag = TagHandle.GetExistingTag("Ground");
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag(_groundTag) || 
                collision.gameObject.GetComponent<Prop>()?.IsGrounded == true)
            {
                OnGrounded();
            }
        }
        
        public void Fix(Rigidbody2D parent)
        {
            _fixedJoint.connectedBody = parent;
            _fixedJoint.enabled = true;
            _inFreeFall = false;
        }

        public void Release(Vector2 velocityModifiers, float throwTorqueMoment = 1)
        {
            _fixedJoint.connectedBody = null;
            _fixedJoint.enabled = false;
            
            // multiply velocity
            _rigidbody.linearVelocity *= velocityModifiers;
            _rigidbody.AddTorque(_rigidbody.linearVelocity.magnitude * throwTorqueMoment *
                                 Mathf.Sign(transform.position.x - _rigidbody.transform.position.x));
            
            _inFreeFall = true;
        }

        private void OnGrounded()
        {
            IsGrounded = true;
            _inFreeFall = false;
            PropGrounded?.Invoke(this);
            print($"{name} is grounded");
        }
    }
}
