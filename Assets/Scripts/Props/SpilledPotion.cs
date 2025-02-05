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
                ApplyEffect();
                Destroy(this);
            }
        }

        protected abstract void ApplyEffect();
    }
}