using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace JesToxic.FX
{
    public enum SoundFxType
    {
        Click = 0,
        MoveSelection,
        HitStone = 100,
        HitSoft,
        HitGlass,
        HitMetal,
        BreakGlass = 200,
        Hurt = 300,
        Death = 400,
        EatFood = 500,
        Catch = 550,
        Ovation = 600,
        OvationBig = 601,
        PopUp = 700,
    }

    public class SoundFxRequest
    {
        public readonly SoundFxType Type;
        public readonly float Volume;

        public SoundFxRequest(SoundFxType type, float volume = 1.0f)
        {
            Type = type;
            Volume = volume;
        }
    }
    
    public class SoundFx : Fx
    {
        [SerializeField] private SoundFxParameters[] _sounds;
        private AudioSource _source;
        private void Awake()
        {
            _source = GetComponent<AudioSource>();
        }

        private void Update()
        {
            if (!_source.isPlaying)
            {
                Release();
            }
        }

        public override void Execute(object arg = null)
        {
            var settings = arg as SoundFxRequest;
            _source.PlayOneShot(_sounds.First(s => s.Key == settings.Type).GetClip(), settings.Volume);
        }

        protected override void CleanUp()
        {
            _source.Stop();
        }

        [Serializable]
        public class SoundFxParameters
        {
            [field: SerializeField]public SoundFxType Key { get; private set; }
            [FormerlySerializedAs("Clips")] [SerializeField] private AudioClip[] _clips;

            public AudioClip GetClip()
            {
                return _clips[Random.Range(0, _clips.Length)];
            }
        }
    }
}