using System.Collections.Generic;
using System.Linq;
using Services;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class RubberCamera : Singleton<RubberCamera>
{
    [SerializeField] private List<Transform> _trackedObjects = new();
    [SerializeField] private float _minSize = 7.5f;
    [SerializeField] private float _frameIndent = 1f;
    [SerializeField] private float _speedOfChange = 2f;
    private Camera _camera;
    private float _oldSize;

    protected override void Awake()
    {
        base.Awake();
        _camera = GetComponent<Camera>();
        _oldSize = _minSize;
    }

    private void Update()
    {
        float size = _minSize;
        if (_trackedObjects.Count != 0)
        {
            float maxY = _trackedObjects.Max(obj => obj.position.y) + _frameIndent;
            size = maxY > _minSize * 2 ? maxY / 2f : _minSize; 
        }
        
        float delta = size - _oldSize;
        size = Mathf.Abs(delta) < _speedOfChange * Time.deltaTime
            ? size
            : _oldSize + Mathf.Sign(delta) * _speedOfChange * Time.deltaTime;
        _oldSize = size;
        
        transform.position = new Vector3(0, size, -10);
        _camera.orthographicSize = size;
    }

    public void AddTrackedObject(Transform trackedObject)
    {
        _trackedObjects.Add(trackedObject);
    }

    public void RemoveTrackedObject(Transform trackedObject)
    {
        _trackedObjects.Remove(trackedObject);
    }
}
