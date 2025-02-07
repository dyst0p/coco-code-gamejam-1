using UnityEngine;

namespace Services
{
    public static class ComponentExtension
    {
        public static void CreateTextFx(this Component component, string text)
        {
            var textFx = FxService.Instance.GetFx(typeof(TextFx));
            textFx.transform.position = component.transform.position;
            textFx.Execute(text);
        }
    }
}