using JesToxic.Services;
using JesToxic.Tools;
using UnityEngine;

namespace JesToxic.Props
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