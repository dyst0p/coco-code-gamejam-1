using System.Collections;
using System.Collections.Generic;
using FX;
using Props;
using Services;
using UnityEngine;

namespace Player
{
    public class Face : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private SpriteRenderer _spriteRendererMouth;
        [SerializeField] private SpriteRenderer _spriteRendererTonge;
        [SerializeField] private Color _fullPoisonedColor;
        [SerializeField] private float _minAlpha = 0.2f;
        [SerializeField] private float _maxPoisonedFace = 10;
        [SerializeField] private Transform _mouth;
        [SerializeField] private Transform _leftPupil;
        [SerializeField] private Transform _rightPupil;
        [SerializeField] private float _distanceToPupil = 0.2f;
        [SerializeField] private Vector3 _closeScale;
        [SerializeField] private Vector3 _chewScale;
        [SerializeField] private float _chewDuration = 2f;
        [SerializeField] private float _minMouthOpenDistance = 3f;
        [SerializeField] private float _maxMouthOpenDistance = 8f;
        private Color _startFaceColor;
        private Color _targetFaceColor;
        private float _targetAlpha = 1f;
        private Coroutine _chewCoroutine;

        private void Start()
        {
            PlayerData.Instance.PoisoningChanged += ChangeFaceColor;
            PlayerData.Instance.HealthChanged += ChangeFaceColorAlpha;
            PlayerData.Instance.GameOver += CloseEyes;
            _targetFaceColor = _startFaceColor = _spriteRenderer.color;
        }

        private void OnDisable()
        {
            PlayerData.Instance.PoisoningChanged -= ChangeFaceColor;
            PlayerData.Instance.HealthChanged -= ChangeFaceColorAlpha;
            PlayerData.Instance.GameOver -= CloseEyes;
        }

        private void CloseEyes()
        {
            var soundFx = FxService.Instance.GetFx(typeof(SoundFx));
            soundFx.transform.position = transform.position;
            soundFx.Execute(new SoundFxRequest(SoundFxType.Death));
            
            _leftPupil.parent.gameObject.SetActive(false);
            _rightPupil.parent.gameObject.SetActive(false);
        }

        private void ChangeFaceColor(float poison)
        {
            _targetFaceColor =
                Color.Lerp(_startFaceColor, _fullPoisonedColor, Mathf.Clamp01(poison / _maxPoisonedFace));
        }
        
        private void ChangeFaceColorAlpha(float health)
        {
            _targetAlpha = _minAlpha + (health / PlayerData.MaxHealth) * (1 - _minAlpha);
        }

        private void Update()
        {
            bool FindClosesProp(List<Transform> props, out Transform closest, out float minDistance)
            {
                closest = null;
                minDistance = float.PositiveInfinity;
                foreach (Transform prop in props)
                {
                    if (prop == null)
                        continue;
                    float sqrDistance = (prop.position - _mouth.position).sqrMagnitude;
                    if (sqrDistance < minDistance)
                    {
                        closest = prop;
                        minDistance = sqrDistance;
                    }
                }
                minDistance = Mathf.Sqrt(minDistance);
                return closest != null;
            }
        
            void LookAtFood(Transform pupil, Transform closestProp)
            {
                Vector3 dir = (closestProp.position - pupil.parent.position).normalized;
                pupil.localPosition = dir * _distanceToPupil;
            }

            void SetAlpha(SpriteRenderer sprite)
            {
                Color color = sprite.color;
                color.a = _targetAlpha;
                sprite.color = color;
            }

            if (FindClosesProp(Prop.AllProps, out Transform closestProp, out float minDistance))
            {
                LookAtFood(_leftPupil, closestProp);
                LookAtFood(_rightPupil, closestProp);
            }
            else
            {
                _leftPupil.localPosition = Vector3.zero;
                _rightPupil.localPosition = Vector3.zero;
            }

            if (FindClosesProp(EdibleProp.AllEdibleProps, out Transform closestEdible, out float minDistanceToEdible) && _chewCoroutine == null)
            {
                float t = Mathf.Clamp01((minDistanceToEdible - _minMouthOpenDistance) /
                                        (_maxMouthOpenDistance - _minMouthOpenDistance));
                _mouth.localScale = Vector3.Lerp(Vector3.one, _closeScale, t);
            }
            else
            {
                _mouth.localScale = _closeScale;
            }
            
            var newColor = Color.Lerp(_spriteRenderer.color, _targetFaceColor, Time.deltaTime);
            _spriteRenderer.color = newColor;
            SetAlpha(_spriteRenderer);
            SetAlpha(_spriteRendererMouth);
            SetAlpha(_spriteRendererTonge);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            var edible = other.GetComponentInParent<EdibleProp>();
            if (edible != null && !edible.IsDeactivated && _chewCoroutine == null)
            {
                edible.Eat();
                _chewCoroutine = StartCoroutine(Chew());
            }
        }

        private IEnumerator Chew()
        {
            float timeElapsed = 0f;
            while (timeElapsed < _chewDuration)
            {
                timeElapsed += Time.deltaTime;
                float t = Mathf.Abs(Mathf.Sin(2 * Mathf.PI * (timeElapsed / _chewDuration)));
                _mouth.localScale = Vector3.Lerp(_closeScale, _chewScale, t);
                yield return null;
            }
            _chewCoroutine = null;
        }
    }
}
