using Services;
using UnityEngine;

namespace FX
{
    public abstract class Fx : MonoBehaviour
    {
        public abstract void Execute();

        protected void Release()
        {
            FxService.Instance.GetPool(GetType()).Release(this);
        }
    }
}