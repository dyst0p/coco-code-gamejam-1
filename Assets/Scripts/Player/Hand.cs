using System.Collections.Generic;
using Services;
using UnityEngine;

namespace Player
{
    public class Hand : MonoBehaviour
    {
        [SerializeField] private Side _side;
        [SerializeField] private Sprite _handOpenSprite;
        [SerializeField] private Sprite _handClosedSprite;

        [SerializeField] private float _throwAcceleration = 3f;
        [SerializeField] private float _throwTorqueMoment = 1f;

        private readonly List<Transform> _propsInContact = new();
        private SpriteRenderer _handRenderer;
        private Rigidbody2D _handRigidbody;
        private Rigidbody2D _caughtPropRigidbody;
        private FixedJoint2D _caughtPropFixedJoint;

        private void Awake()
        {
            _handRenderer = GetComponent<SpriteRenderer>();
            _handRigidbody = GetComponent<Rigidbody2D>();
        }
        
        private void OnEnable()
        {
            if (_side == Side.Left)
            {
                InputProvider.OnCatchLeft += Take;
            }
            else
            {
                InputProvider.OnCatchRight += Take;
            }
        }

        private void OnDisable()
        {
            if (_side == Side.Left)
            {
                InputProvider.OnCatchLeft -= Take;
            }
            else
            {
                InputProvider.OnCatchRight -= Take;
            }
        }


        private void OnTriggerEnter2D(Collider2D other)
        {
            _propsInContact.Add(other.transform.parent);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            _propsInContact.Remove(other.transform.parent);
        }

        private void Take(bool mode)
        {
            if (mode)
            {
                CloseHand();
            }
            else
            {
                OpenHand();
            }
        }

        private void CloseHand()
        {
            _handRenderer.sprite = _handClosedSprite;
            CatchClosestProp();
        }

        private void OpenHand()
        {
            _handRenderer.sprite = _handOpenSprite;
            ReleaseCaughtProp();
        }

        private void CatchClosestProp()
        {
            var minDistance = float.PositiveInfinity;
            Transform closestProp = null;
            foreach (var prop in _propsInContact)
            {
                float distance = Vector2.SqrMagnitude(prop.position - transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closestProp = prop;
                }
            }
            
            if (closestProp == null)
            {
                return;
            }
            _caughtPropRigidbody = closestProp.GetComponent<Rigidbody2D>();
            _caughtPropFixedJoint = closestProp.GetComponent<FixedJoint2D>();
            _caughtPropFixedJoint.connectedBody = _handRigidbody;
            _caughtPropFixedJoint.enabled = true;

            _handRenderer.sortingOrder = 1;
        }

        private void ReleaseCaughtProp()
        {
            if (_caughtPropRigidbody == null)
            {
                return;
            }
            _caughtPropFixedJoint.enabled = false;
            // add velocity
            _caughtPropRigidbody.linearVelocity *= _throwAcceleration;
            _caughtPropRigidbody.AddTorque(_caughtPropRigidbody.linearVelocity.magnitude * _throwTorqueMoment *
                                           Mathf.Sign(transform.position.x -
                                                      _caughtPropRigidbody.transform.position.x));
            
            _caughtPropRigidbody = null;
            _caughtPropFixedJoint = null;
            
            _handRenderer.sortingOrder = -1;
        }
    }
}