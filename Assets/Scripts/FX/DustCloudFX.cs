using System.Collections;
using UnityEngine;

namespace FX
{
    public class DustCloudFX : Fx
    {
        [SerializeField] private Transform[] _subClouds;
        [SerializeField] private float _driftSpeed = 1f;
        [SerializeField] private float _duration = 2f;
        private SpriteRenderer[] _sprites;
        private Vector2[] _subCloudsStartPositions;

        private void Awake()
        {
            _sprites = GetComponentsInChildren<SpriteRenderer>(true);
            _subCloudsStartPositions = new Vector2[_subClouds.Length];
            for (int i = 0; i < _sprites.Length; i++)
            {
                _subCloudsStartPositions[i] = _subClouds[i].transform.localPosition;
            }
        }

        private void OnDisable()
        {
            StopAllCoroutines();
            for (int i = 0; i < _sprites.Length; i++)
            {
                _subClouds[i].transform.localPosition = _subCloudsStartPositions[i];
            }
        }

        public override void Execute()
        {
            StartCoroutine(DustCoroutine());
        }

        private IEnumerator DustCoroutine()
        {
            float timeElapsed = 0f;
            while (timeElapsed < _duration)
            {
                timeElapsed += Time.deltaTime;
                float t = 1 - timeElapsed / _duration;
                foreach (var sprite in _sprites)
                {
                    var color = sprite.color;
                    color.a = t;
                    sprite.color = color;
                }

                foreach (var cloud in _subClouds)
                {
                    cloud.Translate((cloud.position - transform.position).normalized * (_driftSpeed * Time.deltaTime),
                        Space.World);
                }

                yield return null;
            }
            Release();
        }
    }
}
