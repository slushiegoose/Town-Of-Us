// This folder is a Stripped down version of Reactor-Essentials
// Please use https://github.com/DorCoMaNdO/Reactor-Essentials because it is more updated and less buggy

using System;

namespace TownOfUs.CustomOption
{
    public class CustomButtonOption : CustomOption
    {

        protected internal System.Action Do;
        protected internal CustomButtonOption(int id, string name, System.Action toDo = null) : base(id, name, CustomOptionType.Button, 0)
        {
            Do = toDo ?? BaseToDo;
        }

        public static void BaseToDo()
        {
        }
        
        

        public override void OptionCreated()
        {
            base.OptionCreated();
            Setting.Cast<ToggleOption>().TitleText.text = Name;
        }
    }
}