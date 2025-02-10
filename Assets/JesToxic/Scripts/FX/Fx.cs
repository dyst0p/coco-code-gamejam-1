using Services;
using UnityEngine;

namespace FX
{
    public abstract class Fx : MonoBehaviour
    {
        public abstract void Execute(object arg = null);
        protected abstract void CleanUp();

        protected void Release()
        {
            CleanUp();
            FxService.Instance.GetPool(GetType()).Release(this);
        }
    }
}