using System.Collections.Generic;
using UnityEngine;

public class DontDestroyOnLoad : MonoBehaviour
{
    private static List<string> _savedObjects = new();
    private bool _saved;
    void Start()
    {
        if (_savedObjects.Contains(name))
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(this);
        _saved = true;
        _savedObjects.Add(name);
    }

    private void OnDestroy()
    {
        if (_saved)
        {
            _savedObjects.Remove(name);
        }
    }
}
