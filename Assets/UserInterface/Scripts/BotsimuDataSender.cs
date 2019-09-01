using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

using static GlobalConstants;

public class BotsimuDataSender
{
    private static IPEndPoint EndPoint = new IPEndPoint(IPAddress.Parse(HOST), PORT);

    public static void SendUDP(byte[] aData)
    {
        using (Socket Socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp))
        {
            try
            {
                Socket.SendTo(aData, EndPoint);
            }
            catch (SocketException _Exception)
            {
                if (_Exception.SocketErrorCode == SocketError.WouldBlock ||
                    _Exception.SocketErrorCode == SocketError.IOPending ||
                    _Exception.SocketErrorCode == SocketError.NoBufferSpaceAvailable)
                {
                    Thread.Sleep(30);
                }
                else
                {
                    throw _Exception;
                }
            }
        }
    }
}

