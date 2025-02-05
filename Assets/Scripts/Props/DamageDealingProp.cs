using System.Collections;
using Player;
using UnityEngine;

namespace Props
{
    public class DamageDealingProp : Prop
    {
        [SerializeField] protected float _damage = 1f;
        [SerializeField] protected float _delayInSeconds = 1f;
        private Coroutine _hurtRoutine;
        
        public override void Fix(Rigidbody2D parent)
        {
            base.Fix(parent);
            _hurtRoutine = StartCoroutine(HurtRoutine(parent.GetComponent<Hand>()));
        }

        private IEnumerator HurtRoutine(Hand hand)
        {
            while (true)
            {
                hand.HurtAnimate();
                PlayerData.Instance.ChangeHealth(-_damage);
                yield return new WaitForSeconds(_delayInSeconds);
            }
        }

        public override void Release(Vector2 velocityModifiers, float throwTorqueMoment = 1)
        {
            base.Release(velocityModifiers, throwTorqueMoment);
            if (_hurtRoutine != null)
            {
                StopCoroutine(_hurtRoutine);
                _hurtRoutine = null;
            }
        }
    }
}