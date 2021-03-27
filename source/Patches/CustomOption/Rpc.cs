using System.Collections.Generic;
using System.Linq;
using Hazel;
using Reactor;

namespace TownOfUs.CustomOption
{
    public static class Rpc
    {
        public static void SendRpc()
        {
            var writer = AmongUsClient.Instance.StartRpc(PlayerControl.LocalPlayer.NetId,
                (byte) CustomRPC.SyncCustomSettings, SendOption.Reliable);
            foreach (var option in CustomOption.AllOptions)
            {
                writer.Write(option.ID);
                if (option.Type == CustomOptionType.Toggle) writer.Write((bool)option.Value);
                else if (option.Type == CustomOptionType.Number) writer.Write((float)option.Value);
                else if (option.Type == CustomOptionType.String) writer.Write((int)option.Value);
                
            }
            writer.EndMessage();
            
        }

        public static void ReceiveRpc(MessageReader reader)
        {
            PluginSingleton<TownOfUs>.Instance.Log.LogInfo($"Options received:");
            while (reader.BytesRemaining > 0)
            {
                
                int id = reader.ReadInt32();
                CustomOption customOption = CustomOption.AllOptions.FirstOrDefault(option => option.ID == id); // Works but may need to change to gameObject.name check
                var type = customOption?.Type;
                object value = null;
                if (type == CustomOptionType.Toggle) value = reader.ReadBoolean();
                else if (type == CustomOptionType.Number) value = reader.ReadSingle();
                else if (type == CustomOptionType.String) value = reader.ReadInt32();

                customOption?.Set(value);
                
                PluginSingleton<TownOfUs>.Instance.Log.LogInfo($"{customOption?.Name} : {customOption}:");
                
            }
        }
    }
}