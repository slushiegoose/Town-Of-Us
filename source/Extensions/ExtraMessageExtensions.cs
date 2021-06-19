using System;
using Hazel;
using Hazel.Udp;
using UnityEngine;

namespace TownOfUs.Extensions
{
    public static class ExtraMessageExtensions
    {
        private const float MIN = -50f;
        private const float MAX = 50f;

        private static float ReverseLerp(float t)
        {
            return Mathf.Clamp((t - MIN) / (MAX - MIN), 0f, 1f);
        }

        public static void Write(this MessageWriter writer, Vector2 value)
        {
            var x = (ushort) (ReverseLerp(value.x) * ushort.MaxValue);
            var y = (ushort) (ReverseLerp(value.y) * ushort.MaxValue);

            writer.Write(x);
            writer.Write(y);
        }

        public static Vector2 ReadVector2(this MessageReader reader)
        {
            var x = reader.ReadUInt16() / (float) ushort.MaxValue;
            var y = reader.ReadUInt16() / (float) ushort.MaxValue;

            return new Vector2(Mathf.Lerp(MIN, MAX, x), Mathf.Lerp(MIN, MAX, y));
        }

        public static void Send(this UdpConnection connection, MessageWriter msg, Action ackCallback)
        {
            if (msg.SendOption != SendOption.Reliable)
                throw new InvalidOperationException("Message SendOption has to be Reliable.");

            var buffer = new byte[msg.Length];
            Buffer.BlockCopy(msg.Buffer, 0, buffer, 0, msg.Length);

            connection.ResetKeepAliveTimer();

            connection.AttachReliableID(buffer, 1, ackCallback);
            connection.WriteBytesToConnection(buffer, buffer.Length);
            connection.Statistics.LogReliableSend(buffer.Length - 3, buffer.Length);
        }
    }
}