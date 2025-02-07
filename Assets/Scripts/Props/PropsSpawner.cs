using System;
using System.Collections;
using FX;
using Services;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Props
{
    public class PropsSpawner : Singleton<PropsSpawner>
    {
        [SerializeField] private int _propsToSpawn = 1;
        [SerializeField] private Transform[] _spawnPoints;
        [SerializeField] private Prop[] _propPrefabs;
        [SerializeField] private float _delayBeforeSpawn = 1f;
        [SerializeField] private float _fallingDelayOnSpawn = 1f;
        private int _activeProps;
        private WaitForSeconds _spawnDelay;
        private WaitForSeconds _delayAfterSpawn;

        private void Start()
        {
            PlayerData.Instance.ScoreChanged += Rebalance;
            PlayerData.Instance.GameOver += StopAllCoroutines;
            _spawnDelay = new WaitForSeconds(_delayBeforeSpawn);
            _delayAfterSpawn = new WaitForSeconds(_fallingDelayOnSpawn);
            StartCoroutine(SpawnProps());
        }

        private void OnDisable()
        {
            PlayerData.Instance.ScoreChanged -= Rebalance;
            PlayerData.Instance.GameOver -= StopAllCoroutines;
        }

        private void Rebalance(float score)
        {
            _propsToSpawn = (int)(score / 30) + 1;
                
        }

        private IEnumerator SpawnProps()
        {
            while (true)
            {
                if (_activeProps < _propsToSpawn)
                {
                    yield return _spawnDelay;
                    int propMaxIndex = Mathf.Min(Mathf.CeilToInt(PlayerData.Instance.Score/3), _propPrefabs.Length);
                    if (propMaxIndex < 3)
                        propMaxIndex = 3;
                    int propIndex = Random.Range(0, propMaxIndex);
                    print($"max index = {propMaxIndex} generated index = {propIndex}");
                    Prop prop = Instantiate(_propPrefabs[propIndex],
                        _spawnPoints[Random.Range(0, _spawnPoints.Length)].position,
                        Quaternion.Euler(0, 0, Random.Range(0f, 360f)));
                    prop.transform.SetParent(transform);
                    prop.PropDeactivated += PropDeactivatedHandler;
                    
                    var dustCloud = FxService.Instance.GetFx(typeof(DustCloudFX));
                    dustCloud.transform.position = prop.transform.position;
                    dustCloud.Execute();
                    
                    RubberCamera.Instance.AddTrackedObject(prop.transform);
                    _activeProps++;
                    yield return _delayAfterSpawn;
                    prop.enabled = true;
                }
                yield return null;
            }
        }

        private void PropDeactivatedHandler(Prop prop)
        {
            _activeProps--;
            RubberCamera.Instance.RemoveTrackedObject(prop.transform);
            prop.PropDeactivated -= PropDeactivatedHandler;
        }
    }
}
