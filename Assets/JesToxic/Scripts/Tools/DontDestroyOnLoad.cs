using System.Collections.Generic;
using UnityEngine;

namespace JesToxic.Tools
{
    public class DontDestroyOnLoad : MonoBehaviour
    {
        private static readonly List<string> SavedObjects = new();
        private bool _saved;
        void Start()
        {
            if (SavedObjects.Contains(name))
            {
                Destroy(gameObject);
                return;
            }

            DontDestroyOnLoad(this);
            _saved = true;
            SavedObjects.Add(name);
        }

        private void OnDestroy()
        {
            if (_saved)
            {
                SavedObjects.Remove(name);
            }
        }
    }
}
