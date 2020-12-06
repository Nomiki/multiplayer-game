using Assets.Scripts;
using GameNetworkingShared.Logging;
using GameNetworkingShared.Objects;
using GameNetworkingShared.Packets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientHandle : MonoBehaviour
{
    public static void Welcome(Packet packet)
    {
        WelcomeMessage msg = packet.ReadObj<WelcomeMessage>();

        LogFactory.Instance.Debug($"Got WelcomeMessage: {msg}");

        ClientManager.Instance.Client.MyId = msg.ClientId;

        //TODO: Add welcome received to server
    }
}
