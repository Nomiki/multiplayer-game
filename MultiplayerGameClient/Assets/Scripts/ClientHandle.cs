﻿using Assets.Scripts;
using Assets.Scripts.Networking;
using Assets.Scripts.Packets;
using GameNetworkingShared.Logging;
using GameNetworkingShared.Objects;
using GameNetworkingShared.Packets;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class ClientHandle : MonoBehaviour
{
    public static void Welcome(Packet packet, int fromClient = -1)
    {
        WelcomeMessage msg = packet.ReadObj<WelcomeMessage>();

        LogFactory.Instance.Debug($"Got WelcomeMessage: {msg}");

        Client client = ClientManager.Instance.Client;
        client.Id = msg.ClientId;

        ClientSend.SendWelcomeReceived();

        client.Udp.Connect(((IPEndPoint)client.Tcp.Socket.Client.LocalEndPoint).Port);
    }

    public static void HandleUdpTest(Packet packet, int fromClient = -1)
    {
        UdpTest test = packet.ReadObj<UdpTest>();
        LogFactory.Instance.Debug($"Got UdpTest message: '{test.Message}'");

        ClientSend.SendUdpTestReceived();
    }
}
