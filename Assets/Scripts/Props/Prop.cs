using System;
using System.Collections.Generic;
using Player;
using Services;
using UnityEngine;

namespace Props
{
    public class Prop : MonoBehaviour
    {
        public static readonly List<Transform> AllProps = new();
        protected Rigidbody2D _rigidbody;
        private FixedJoint2D _fixedJoint;
        private TagHandle _groundTag;
        private Side _lastHandSide = Side.None;
        private float _throwTime;
        private readonly Color _deactivateColor = new(0.7f, 0.7f, 0.7f);
        [SerializeField] private PhysicsMaterial2D _deactivateMaterial;
        [field:SerializeField]
        public bool IsDeactivated { get; protected set; }
        public event Action<Prop> PropDeactivated;
        
        protected virtual void OnEnable()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _rigidbody.simulated = true;
            _fixedJoint = GetComponent<FixedJoint2D>();
            _groundTag = TagHandle.GetExistingTag("Ground");
            AllProps.Add(transform);
        }

        protected virtual void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag(_groundTag) || 
                collision.gameObject.GetComponent<Prop>()?.IsDeactivated == true)
            {
                if (!IsDeactivated)
                {
                    OnDeactivated();
                }
            }
        }
        
        public virtual void Fix(Rigidbody2D parent)
        {
            _fixedJoint.connectedBody = parent;
            _fixedJoint.enabled = true;
            var newHand = parent.GetComponent<Hand>().HandSide;
            // not a first catch
            if (_lastHandSide != Side.None && !IsDeactivated)
            {
                int modificator = _lastHandSide != newHand ? 2 : 1;
                float score = Time.fixedTime - _throwTime;
                PlayerData.Instance.AddScore(score * modificator);
                var textFx = FxService.Instance.GetFx(typeof(TextFx));
                textFx.transform.position = parent.transform.position;
                string text = modificator == 1
                    ? $"<color=yellow>+{(int) (score * 10)}"
                    : $"<color=yellow>+{(int) (score * 10)}\nx2";
                textFx.Execute(text);
            }
            _lastHandSide = newHand;
        }

        public virtual void Release(Vector2 velocityModifiers, float throwTorqueMoment = 1)
        {
            _fixedJoint.connectedBody = null;
            _fixedJoint.enabled = false;
            
            // multiply velocity
            _rigidbody.linearVelocity *= velocityModifiers;
            _rigidbody.AddTorque(_rigidbody.linearVelocity.magnitude * throwTorqueMoment *
                                 Mathf.Sign(transform.position.x - _rigidbody.transform.position.x));
            
            _throwTime = Time.fixedTime;
        }

        protected virtual void OnDeactivated()
        {
            IsDeactivated = true;
            AllProps.Remove(transform);
            var renderers = GetComponentsInChildren<SpriteRenderer>();
            foreach (var renderer in renderers)
            {
                var oldColor = renderer.color;
                renderer.color = oldColor * _deactivateColor;
            }
            
            _rigidbody.sharedMaterial = _deactivateMaterial;
            PropDeactivated?.Invoke(this);
        }
    }

    public abstract class EdibleProp : Prop
    {
        public static readonly List<Transform> AllEdibleProps = new();

        protected override void OnEnable()
        {
            base.OnEnable();
            AllEdibleProps.Add(transform);
        }

        protected override void OnDeactivated()
        {
            base.OnDeactivated();
            AllEdibleProps.Remove(transform);
        }

        public virtual void Eat()
        {
            OnDeactivated();
            Destroy(gameObject);
        }
    }
}
