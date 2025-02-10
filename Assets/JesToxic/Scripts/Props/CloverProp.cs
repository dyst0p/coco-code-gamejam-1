using JesToxic.Services;
using JesToxic.Tools;

namespace JesToxic.Props
{
    public class CloverProp : EdibleProp
    {
        public override void Eat()
        {
            PlayerData.Instance.GetLucky();
            this.CreateTextFx($"<color=purple>+luck");
            base.Eat();
        }
    }
}