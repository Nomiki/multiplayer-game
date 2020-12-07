using Assets.Scripts;
using Assets.Scripts.Packets;
using GameNetworkingShared.Logging;
using GameNetworkingShared.Objects;
using GameNetworkingShared.Packets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientHandle : MonoBehaviour
{
    public static void Welcome(Packet packet, int fromClient = -1)
    {
        WelcomeMessage msg = packet.ReadObj<WelcomeMessage>();

        LogFactory.Instance.Debug($"Got WelcomeMessage: {msg}");

        ClientManager.Instance.Client.Id = msg.ClientId;

        ClientSend.SendWelcomeReceived();
    }
}
