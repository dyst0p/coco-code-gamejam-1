using UnityEngine;

namespace Props
{
    public class AntidoteEdibleProp : EdibleProp
    {
        [SerializeField] private float _antidoteAmount = 3f;
        public override void Eat()
        {
            PlayerData.Instance.ChangePoisoning(-_antidoteAmount);
            base.Eat();
        }
    }
}