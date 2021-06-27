using System;
using System.Collections;
using System.Collections.Generic;
using HarmonyLib;
using Hazel;
using InnerNet;
using UnityEngine;

namespace TownOfUs.Handshake
{
    public static class ClientHandshake
    {
        private const byte TOU_ROOT_HANDSHAKE_TAG = 69;
        
        // TODO: super sus but whatever - "2.1.4"
        private const int TOU_VERSION = 214;

        [HarmonyPatch(typeof(AmongUsClient), nameof(AmongUsClient.OnGameJoined))]
        public static class AmongUsClient_OnGameJoined
        {
            public static void Postfix(AmongUsClient __instance)
            {
                if (AmongUsClient.Instance.AmHost)
                    return;

                // If I am client, send handshake
                PluginSingleton<TownOfUs>.Instance.Log.LogMessage($"AmongUsClient.OnGameJoined.Postfix - Am client, sending handshake");
                var messageWriter = MessageWriter.Get(SendOption.Reliable);
                messageWriter.StartMessage(6);
                messageWriter.Write(__instance.GameId);
                messageWriter.WritePacked(__instance.HostId);
                messageWriter.StartMessage(TOU_ROOT_HANDSHAKE_TAG);
                messageWriter.Write(AmongUsClient.Instance.ClientId);
                messageWriter.Write(TOU_VERSION);
                messageWriter.EndMessage();
                messageWriter.EndMessage();
                __instance.SendOrDisconnect(messageWriter);
                messageWriter.Recycle();
            }
        }

        [HarmonyPatch(typeof(InnerNetClient), nameof(InnerNetClient.HandleGameData))]
        public static class InnerNetClient_HandleGameData
        {
            public static bool Prefix(InnerNetClient __instance,
                [HarmonyArgument(0)] MessageReader reader)
            {
                // If i am host, respond to handshake
                if (__instance.AmHost && reader.BytesRemaining > 3)
                {
                    var handshakeReader = MessageReader.Get(reader).ReadMessageAsNewBuffer();
                    if (handshakeReader.Tag == TOU_ROOT_HANDSHAKE_TAG)
                    {
                        PluginSingleton<TownOfUs>.Instance.Log.LogMessage($"InnerNetClient.HandleMessage.Prefix - Host recieved TOU handshake");
                        
                        var clientId = handshakeReader.ReadInt32();
                        var touVersion = handshakeReader.ReadInt32();
                        
                        // List<int> HandshakedClients - exists to disconnect legacy clients that don't send handshake
                        PluginSingleton<TownOfUs>.Instance.Log.LogMessage($"InnerNetClient.HandleMessage.Prefix - Adding {clientId} with TOU version {touVersion} to List<int>HandshakedClients");
                        if (!HandshakedClients.Contains(clientId));
                            HandshakedClients.Add(clientId);

                        if (touVersion != TOU_VERSION)
                        {
                            PluginSingleton<TownOfUs>.Instance.Log.LogMessage($"InnerNetClient.HandleMessage.Prefix - ClientId {clientId} has mismatched TOU version {touVersion}. (Ours is {TOU_VERSION})");
                            __instance.SendCustomDisconnect(clientId);
                        }
                        
                        return false;
                    }
                }

                return true;
            }
        }
        
        // Handle legacy clients that don't send handshakes
        private static List<int> HandshakedClients = new List<int>();
        private static IEnumerator WaitForHandshake(InnerNetClient innerNetClient, int clientId)
        {
            PluginSingleton<TownOfUs>.Instance.Log.LogMessage($"WaitForHandshake(innerNetClient, clientId = {clientId})");
            while (innerNetClient.GameState == InnerNetClient.GameStates.Ended)
                yield return null;

            yield return new WaitForSeconds(5f);
            PluginSingleton<TownOfUs>.Instance.Log.LogMessage($"WaitForHandshake() - Waited 5 seconds");
            if (!HandshakedClients.Contains(clientId))
            {
                PluginSingleton<TownOfUs>.Instance.Log.LogMessage($"WaitForHandshake() - HandshakedClients did not contain clientId {clientId}");
                innerNetClient.SendCustomDisconnect(clientId);
            }
            else
            {
                PluginSingleton<TownOfUs>.Instance.Log.LogMessage($"WaitForHandshake() - HandshakedClients contained clientId {clientId}");
            }
        }
        
        [HarmonyPatch(typeof(AmongUsClient), nameof(AmongUsClient.OnPlayerJoined))]
        public static class AmongUsClient_OnPlayerJoined
        {
            public static void Postfix(AmongUsClient __instance, [HarmonyArgument(0)] ClientData data)
            {
                PluginSingleton<TownOfUs>.Instance.Log.LogMessage($"AmongUsClient.OnPlayerJoined.Postfix");
                if (AmongUsClient.Instance.AmHost)
                {
                    PluginSingleton<TownOfUs>.Instance.Log.LogMessage($"Am host and clientId {data.Id} sent JoinGameResponse");
                    Coroutines.Start(WaitForHandshake(__instance, data.Id));
                }
            }
        }
        
        private static void SendCustomDisconnect(this InnerNetClient innerNetClient, int clientId)
        {
            var messageWriter = MessageWriter.Get(SendOption.Reliable);
            messageWriter.StartMessage(11);
            messageWriter.Write(innerNetClient.GameId);
            messageWriter.WritePacked(clientId);
            messageWriter.Write(false);
            messageWriter.Write(8);
            messageWriter.Write($"The host has a different version of Town Of Us ({TOU_VERSION})");
            messageWriter.EndMessage();
            innerNetClient.SendOrDisconnect(messageWriter);
            messageWriter.Recycle();
        }
    }
}