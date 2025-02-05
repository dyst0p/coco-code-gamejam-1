using System;
using System.Collections;
using UnityEngine;

public class DustCloud : MonoBehaviour
{
    [SerializeField] private Transform[] _subClouds;
    [SerializeField] private float _driftSpeed = 1f;
    [SerializeField] private float _duration = 2f;
    private SpriteRenderer[] _sprites;

    private IEnumerator Start()
    {
        _sprites = GetComponentsInChildren<SpriteRenderer>();
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
                cloud.Translate((cloud.position - transform.position).normalized * _driftSpeed * Time.deltaTime,
                    Space.World);
            }

            yield return null;
        }
        Destroy(gameObject);
    }
}
