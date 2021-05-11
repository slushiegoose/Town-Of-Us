// This folder is a Stripped down version of Reactor-Essentials
// Please use https://github.com/DorCoMaNdO/Reactor-Essentials because it is more updated and less buggy

using System;
using System.Collections.Generic;

namespace TownOfUs.CustomOption
{
    public class CustomOption
    {

        public static List<CustomOption> AllOptions = new List<CustomOption>();
        protected internal object Value { get; set; }
        protected internal OptionBehaviour Setting { get; set; }
        protected internal CustomOptionType Type { get; set; }
        public string Name;
        public readonly int ID;
        public object DefaultValue { get; set; }

        public Func<object, string> Format;

        public static bool LobbyTextScroller { get; set; } = true;
        
        protected internal bool Indent { get; set; }


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
            
            System.Console.WriteLine($"{Name} set to {value}");
            
            Value = value;

            if (Setting != null && AmongUsClient.Instance.AmHost && SendRpc)
            {
                Rpc.SendRpc(this);
            }

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
                    float newValue = (float) Value;

                    number.Value = number.oldValue = newValue;
                    number.ValueText.text = ToString();
                }
                else if (Setting is StringOption str)
                {
                    int newValue = (int) Value;

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