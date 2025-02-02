using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class RubberCamera : MonoBehaviour
{
    [SerializeField] private List<Transform> _trackedObjects = new List<Transform>();
    [SerializeField] private float _minSize = 7.5f;
    [SerializeField] private float _frameIndent = 1f;
    private Camera _camera;

    private void Awake()
    {
        _camera = GetComponent<Camera>();
    }

    private void Update()
    {
        float maxY = _trackedObjects.Max(obj => obj.position.y) + _frameIndent;
        float size = maxY > _minSize * 2 ? maxY / 2f : _minSize;
        transform.position = new Vector3(0, size, -10);
        _camera.orthographicSize = size;
    }
}
