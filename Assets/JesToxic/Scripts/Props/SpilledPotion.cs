using JesToxic.FX;
using JesToxic.Tools;
using UnityEngine;

namespace JesToxic.Props
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
                this.CreateSoundFx(SoundFxType.HitSoft);
                
                ApplyEffect();
                Destroy(this);
            }
        }

        protected abstract void ApplyEffect();
    }
}