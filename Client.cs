using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;



class Client
{
    public TcpClient tcpClient;
    public string pseudo;

    public Client(TcpClient _tcpClient, int guestNumber)
    {
        tcpClient = new TcpClient();
        tcpClient = _tcpClient;
        pseudo = "guest" + guestNumber.ToString();
    }

}

