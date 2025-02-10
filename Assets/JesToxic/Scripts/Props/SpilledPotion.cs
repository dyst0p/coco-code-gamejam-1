using FX;
using Services;
using UnityEngine;

namespace Props
{
    public abstract class SpilledPotion : MonoBehaviour
    {
        private TagHandle _groundTag;
        private void Awake()
        {
            _groundTag = TagHandle.GetExistingTag("Ground");
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag(_groundTag))
            {
                var soundFx = FxService.Instance.GetFx(typeof(SoundFx));
                soundFx.transform.position = transform.position;
                soundFx.Execute(new SoundFxRequest(SoundFxType.HitSoft));
                
                ApplyEffect();
                Destroy(this);
            }
        }

        protected abstract void ApplyEffect();
    }
}