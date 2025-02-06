using Services;
using UnityEngine;

namespace FX
{
    public abstract class Fx : MonoBehaviour
    {
        public abstract void Execute();
        protected abstract void CleanUp();

        protected void Release()
        {
            CleanUp();
            FxService.Instance.GetPool(GetType()).Release(this);
        }
    }
}