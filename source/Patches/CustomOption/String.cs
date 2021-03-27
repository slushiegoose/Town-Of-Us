using UnityEngine;

namespace TownOfUs.CustomOption
{
    public class CustomStringOption : CustomOption
    {
        
        protected string[] Values { get; set; }

        protected internal CustomStringOption(int id, string name, string[] values) : base(id, name, CustomOptionType.String,
            0)
        {
            Values = values;
            Format = value => Values[(int)value];
        }

        protected internal int Get()
        {
            return (int) Value;
        }

        protected internal void Increase()
        {
            Set(Mathf.Clamp(Get() + 1, 0, Values.Length-1));
        }

        protected internal void Decrease()
        {
            Set(Mathf.Clamp(Get() - 1, 0, Values.Length-1));
        }
        
        public override void OptionCreated()
        {
            var str = Setting.Cast<StringOption>();

            str.TitleText.Text = Name;
            str.Value = str.oldValue = Get();
            str.ValueText.Text = ToString();
        }
        
        
        
    }
}