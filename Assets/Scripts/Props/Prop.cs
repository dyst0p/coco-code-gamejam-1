using UnityEngine;

namespace Props
{
    public class Prop : MonoBehaviour
    {
        private Rigidbody2D _rigidbody;
        private FixedJoint2D _fixedJoint;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _fixedJoint = GetComponent<FixedJoint2D>();
            _rigidbody.simulated = true;
        }
        
        public void Fix(Rigidbody2D parent)
        {
            _fixedJoint.connectedBody = parent;
            _fixedJoint.enabled = true;
        }

        public void Release(Vector2 velocityModifiers, float throwTorqueMoment = 1)
        {
            _fixedJoint.connectedBody = null;
            _fixedJoint.enabled = false;
            
            // multiply velocity
            _rigidbody.linearVelocity *= velocityModifiers;
            _rigidbody.AddTorque(_rigidbody.linearVelocity.magnitude * throwTorqueMoment *
                                 Mathf.Sign(transform.position.x - _rigidbody.transform.position.x));
        }
    }
}
