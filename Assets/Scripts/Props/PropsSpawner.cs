using System.Collections;
using Services;
using UnityEngine;

namespace Props
{
    public class PropsSpawner : Singleton<PropsSpawner>
    {
        [SerializeField] private int _propsToSpawn = 1;
        [SerializeField] private Transform[] _spawnPoints;
        [SerializeField] private Prop[] _propPrefabs;
        [SerializeField] private float _delayBeforeSpawn = 1f;
        [SerializeField] private float _fallingDelayOnSpawn = 1f;
        private int _activeProps = 0;
        private WaitForSeconds _spawnDelay;
        private WaitForSeconds _delayAfterSpawn;

        private void Start()
        {
            _spawnDelay = new WaitForSeconds(_delayBeforeSpawn);
            _delayAfterSpawn = new WaitForSeconds(_fallingDelayOnSpawn);
            StartCoroutine(SpawnProps());
        }
        
        private IEnumerator SpawnProps()
        {
            while (true)
            {
                if (_activeProps < _propsToSpawn)
                {
                    yield return _spawnDelay;
                    Prop prop = Instantiate(_propPrefabs[Random.Range(0, _propPrefabs.Length)],
                        _spawnPoints[Random.Range(0, _spawnPoints.Length)].position,
                        Quaternion.Euler(0, 0, Random.Range(0f, 360f)));
                    prop.transform.SetParent(transform);
                    prop.PropGrounded += PropGroundedHandler;
                    RubberCamera.Instance.AddTrackedObject(prop.transform);
                    _activeProps++;
                    yield return _delayAfterSpawn;
                    prop.enabled = true;
                }
                yield return null;
            }
        }

        private void PropGroundedHandler(Prop prop)
        {
            _activeProps--;
            RubberCamera.Instance.RemoveTrackedObject(prop.transform);
            prop.PropGrounded -= PropGroundedHandler;
        }
    }
}
