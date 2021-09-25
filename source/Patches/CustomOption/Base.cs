using System;
using System.Collections.Generic;
using System.Linq;

namespace TownOfUs.CustomOption
{
    public class CustomOption
    {
        public static List<CustomOption> AllOptions = new List<CustomOption>();
        public static int ShownOptions()
        {
            return AllOptions.Count(option => option.ShouldShow());
        }

        public readonly int ID;

        public Func<object, string> Format;
        public string Name;


        protected internal CustomOption(int id, string name, CustomOptionType type, object defaultValue,
            Func<object, string> format = null)
        {
            ID = id;
            Name = name;
            Type = type;
            DefaultValue = Value = defaultValue;
            Format = format ?? (obj => $"{obj}");

            if (Type == CustomOptionType.Button) return;
            AllOptions.Add(this);
            Set(Value);
        }

        protected internal object Value { get; set; }
        protected internal OptionBehaviour Setting { get; set; }
        protected internal CustomOptionType Type { get; set; }
        public object DefaultValue { get; set; }

        public static bool LobbyTextScroller { get; set; } = true;
        public Func<bool> ShouldShow { get; set; } = () => true;
        public CustomNumberOption Parent { get; set; } = null;
        protected internal bool Indent { get; set; }

        public override string ToString()
        {
            return Format(Value);
        }

        public virtual void OptionCreated()
        {
            Setting.name = Setting.gameObject.name = Name;
        }


        protected internal void Set(object value, bool SendRpc = true)
        {
            Value = value;

            if (Setting != null && AmongUsClient.Instance.AmHost && SendRpc) Rpc.SendRpc(this);


            try
            {
                if (Setting is ToggleOption toggle)
                {
                    var newValue = (bool) Value;
                    toggle.oldValue = newValue;
                    if (toggle.CheckMark != null) toggle.CheckMark.enabled = newValue;
                }
                else if (Setting is NumberOption number)
                {
                    var newValue = (float) Value;

                    number.Value = number.oldValue = newValue;
                    number.ValueText.text = ToString();
                }
                else if (Setting is StringOption str)
                {
                    var newValue = (int) Value;

                    str.Value = str.oldValue = newValue;
                    str.ValueText.text = ToString();
                }
            }
            catch
            {
            }
        }
    }
}
