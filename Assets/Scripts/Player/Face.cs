using System.Collections;
using System.Collections;
using System.Collections.Generic;
using Props;
using UnityEngine;

public class Face : MonoBehaviour
{
    [SerializeField] private Transform _mouth;
    [SerializeField] private Transform _leftPupil;
    [SerializeField] private Transform _rightPupil;
    [SerializeField] private float _distanceToPupil = 0.2f;
    [SerializeField] private Vector3 _closeScale;
    [SerializeField] private Vector3 _chewScale;
    [SerializeField] private float _chewDuration = 2f;
    [SerializeField] private float _minMouthOpenDistance = 3f;
    [SerializeField] private float _maxMouthOpenDistance = 8f;
    private Coroutine _chewCoroutine;

    private void Update()
    {
        bool FindClosesProp(List<Transform> props, out Transform closest, out float minDistance)
        {
            closest = null;
            minDistance = float.PositiveInfinity;
            foreach (Transform prop in EdibleProp.AllEdibleProps)
            {
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
