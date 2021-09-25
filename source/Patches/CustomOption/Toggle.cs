namespace TownOfUs.CustomOption
{
    public class CustomToggleOption : CustomOption
    {
        protected internal CustomToggleOption(int id, string name, bool value) : base(id, name,
            CustomOptionType.Toggle,
            value)
        {
            Format = val => (bool) val ? "On" : "Off";
        }

        protected internal bool Get()
        {
            return (bool) Value;
        }

        protected internal void Toggle()
        {
            Set(!Get());
        }

        public override void OptionCreated()
        {
            base.OptionCreated();
            Setting.Cast<ToggleOption>().TitleText.text = Name;
            Setting.Cast<ToggleOption>().CheckMark.enabled = Get();
        }
    }
}
