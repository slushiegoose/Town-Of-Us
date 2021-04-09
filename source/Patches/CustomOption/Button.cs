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
            Setting.Cast<ToggleOption>().TitleText.Text = Name;
        }
    }
}