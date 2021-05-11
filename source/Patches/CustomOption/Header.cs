// This folder is a Stripped down version of Reactor-Essentials
// Please use https://github.com/DorCoMaNdO/Reactor-Essentials because it is more updated and less buggy

using Il2CppSystem.Runtime.Remoting.Messaging;

namespace TownOfUs.CustomOption
{
    public class CustomHeaderOption : CustomOption
    {
        protected internal CustomHeaderOption(int id, string name) : base(id, name, CustomOptionType.Header, 0)
        {
        }

        public override void OptionCreated()
        {
            base.OptionCreated();
            Setting.Cast<ToggleOption>().TitleText.text = Name;
        }
    }
}