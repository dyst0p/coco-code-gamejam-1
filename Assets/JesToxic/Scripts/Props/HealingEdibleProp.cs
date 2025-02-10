using Services;
using UnityEngine;

namespace Props
{
    public class HealingEdibleProp : EdibleProp
    {
        [SerializeField] private float _healAmount = 10f;
        public override void Eat()
        {
            PlayerData.Instance.ChangeHealth(_healAmount);
            this.CreateTextFx($"<color=red>+{_healAmount}");
            base.Eat();
        }
    }
}