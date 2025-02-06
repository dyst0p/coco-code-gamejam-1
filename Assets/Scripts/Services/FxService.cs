using System;
using System.Collections.Generic;
using System.Linq;
using FX;
using UnityEngine;
using UnityEngine.Pool;

namespace Services
{
    public class FxService : Singleton<FxService>
    {
        [SerializeField] private Fx[] _prefabs;
        private readonly Dictionary<Type,LinkedPool<Fx>> _fxPools = new();

        public Fx GetFx(Type fxType)
        {
            if (_fxPools.TryGetValue(fxType, out LinkedPool<Fx> fxPool))
            {
                return fxPool.Get();
            }
            
            var newPool = CreatePool(fxType);
            _fxPools[fxType] = newPool;
            return newPool.Get();
        }

        public LinkedPool<Fx> GetPool(Type fxType)
        {
            return _fxPools[fxType];
        }

        private LinkedPool<Fx> CreatePool(Type fxType)
        {
            return new LinkedPool<Fx>(CreatePooledItem, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject);
            
            Fx CreatePooledItem()
            {
                var prefab = _prefabs.First(p => p.GetType() == fxType);
                return Instantiate(prefab, transform, true);
            }

            // Called when an item is returned to the pool using Release
            void OnReturnedToPool(Fx fx)
            {
                fx.gameObject.SetActive(false);
            }

            // Called when an item is taken from the pool using Get
            void OnTakeFromPool(Fx fx)
            {
                fx.gameObject.SetActive(true);
            }

            // If the pool capacity is reached then any items returned will be destroyed.
            // We can control what the destroy behavior does, here we destroy the GameObject.
            void OnDestroyPoolObject(Fx fx)
            {
                Destroy(fx.gameObject);
            }
        }
    }
}