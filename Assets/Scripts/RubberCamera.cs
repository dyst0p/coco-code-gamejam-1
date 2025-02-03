using System.Collections.Generic;
using System.Linq;
using Services;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class RubberCamera : Singleton<RubberCamera>
{
    [SerializeField] private List<Transform> _trackedObjects = new List<Transform>();
    [SerializeField] private float _minSize = 7.5f;
    [SerializeField] private float _frameIndent = 1f;
    private Camera _camera;

    protected override void Awake()
    {
        base.Awake();
        _camera = GetComponent<Camera>();
    }

    private void Update()
    {
        if (_trackedObjects.Count == 0)
            return;
        
        float maxY = _trackedObjects.Max(obj => obj.position.y) + _frameIndent;
        float size = maxY > _minSize * 2 ? maxY / 2f : _minSize;
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
