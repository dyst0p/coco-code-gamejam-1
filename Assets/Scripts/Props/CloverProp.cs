using Services;

namespace Props
{
    public class CloverProp : EdibleProp
    {
        public override void Eat()
        {
            PlayerData.Instance.IsLucky = true;
            this.CreateTextFx($"<color=purple>+luck");
            base.Eat();
        }
    }
}