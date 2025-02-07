using System;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace FX
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
        public SoundFxType Type;
        public float Volume;

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
            [SerializeField] private AudioClip[] Clips;

            public AudioClip GetClip()
            {
                return Clips[Random.Range(0, Clips.Length)];
            }
        }
    }
}