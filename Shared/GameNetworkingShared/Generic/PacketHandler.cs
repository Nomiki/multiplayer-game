using GameNetworkingShared.Packets;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameNetworkingShared.Generic
{
    public delegate void PacketHandler(Packet packet, int fromClient = -1);
}
