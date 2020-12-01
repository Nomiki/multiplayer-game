using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace GameNetworkingShared.Protocols
{
    public abstract class TCP
    {
        public TcpClient Socket { get; protected set; }
        protected NetworkStream Stream { get; set; }
        protected byte[] ReceiveBuffer { get; set; }

        protected TCP()
        {
            // Empty ctor
        }

        public abstract void Connect(TcpClient client = null);

        protected abstract void ReceiveCallback(IAsyncResult result);
    }
}
