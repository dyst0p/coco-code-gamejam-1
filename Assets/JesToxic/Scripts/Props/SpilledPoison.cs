using JesToxic.Services;
using JesToxic.Tools;
using UnityEngine;

namespace JesToxic.Props
{
    public class SpilledPoison : SpilledPotion
    {
        [SerializeField] private float _poisonPower = 1f;
        protected override void ApplyEffect()
        {
            PlayerData.Instance.ChangePoisoning(_poisonPower);
            this.CreateTextFx($"<color=green>+{_poisonPower}");
        }
    }
}