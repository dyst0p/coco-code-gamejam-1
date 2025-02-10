using JesToxic.Services;
using JesToxic.Tools;
using UnityEngine;

namespace JesToxic.Props
{
    public class AntidoteEdibleProp : EdibleProp
    {
        [SerializeField] private float _antidoteAmount = 3f;
        public override void Eat()
        {
            PlayerData.Instance.ChangePoisoning(-_antidoteAmount);
            this.CreateTextFx($"<color=green>-{_antidoteAmount}");
            base.Eat();
        }
    }
}