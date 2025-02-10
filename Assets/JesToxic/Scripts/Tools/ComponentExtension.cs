using JesToxic.FX;
using JesToxic.Services;
using UnityEngine;

namespace JesToxic.Tools
{
    public static class ComponentExtension
    {
        public static void CreateTextFx(this Component component, string text)
        {
            var textFx = FxService.Instance.GetFx(typeof(TextFx));
            textFx.transform.position = component.transform.position;
            textFx.Execute(text);
        }
        
        public static void CreateSoundFx(this Component component, SoundFxType sfxType, float volume = 1)
        {
            var soundFx = FxService.Instance.GetFx(typeof(SoundFx));
            soundFx.transform.position = component.transform.position;
            soundFx.Execute(new SoundFxRequest(sfxType, volume));
        }
    }
}