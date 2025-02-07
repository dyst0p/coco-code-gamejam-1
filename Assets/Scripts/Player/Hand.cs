using System.Collections;
using System.Collections.Generic;
using FX;
using Props;
using Services;
using UnityEngine;

namespace Player
{
    public class Hand : MonoBehaviour
    {
        [field:SerializeField] public Side HandSide { get; private set; }
        [SerializeField] private Sprite _handOpenSprite;
        [SerializeField] private Sprite _handClosedSprite;

        [SerializeField] private Vector2 _throwAcceleration = Vector2.one;
        [SerializeField] private float _throwTorqueMoment = 1f;
        [SerializeField] private float _hurtDuration = 0.3f;

        private readonly List<Transform> _propsInContact = new();
        private SpriteRenderer _handRenderer;
        private Color _baseColor;
        private Rigidbody2D _handRigidbody;
        private Prop _caughtProp;

        private void Awake()
        {
            _handRenderer = GetComponent<SpriteRenderer>();
            _baseColor = _handRenderer.color;
            _handRigidbody = GetComponent<Rigidbody2D>();
        }
        
        private void OnEnable()
        {
            if (HandSide == Side.Left)
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
            if (HandSide == Side.Left)
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

        public void HurtAnimate()
        {
            var soundFx = FxService.Instance.GetFx(typeof(SoundFx));
            soundFx.transform.position = transform.position;
            soundFx.Execute(new SoundFxRequest(SoundFxType.Hurt));
            
            IEnumerator ColorAnimate(float duration)
            {
                float timeElapsed = 0;
                while (timeElapsed < duration)
                {
                    timeElapsed += Time.deltaTime;
                    _handRenderer.color = Color.Lerp(Color.red, _baseColor, timeElapsed / duration);
                    yield return null;
                }
            }

            StartCoroutine(ColorAnimate(_hurtDuration));
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
            
            var soundFx = FxService.Instance.GetFx(typeof(SoundFx));
            soundFx.transform.position = transform.position;
            soundFx.Execute(new SoundFxRequest(SoundFxType.Catch));
            
            _caughtProp = closestProp.GetComponent<Prop>();
            _caughtProp.Fix(_handRigidbody);

            _handRenderer.sortingOrder = 10;
        }

        private void ReleaseCaughtProp()
        {
            if (_caughtProp == null)
            {
                return;
            }
            _caughtProp.Release(_throwAcceleration, _throwTorqueMoment);
            _caughtProp = null;
            
            _handRenderer.sortingOrder = -10;
        }
    }
}