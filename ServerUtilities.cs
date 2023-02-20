using System;
using System.Net.Sockets;
using System.Net;
using System.Collections.Generic;
using shared;
using System.Threading;


class ServerUtilities
{
	public static void NotifyClient(Client client, string message)
	{
		NetworkStream stream = client.tcpClient.GetStream();
		StreamUtil.Write(stream, System.Text.Encoding.ASCII.GetBytes(message));
	}

	public static void NotifyOtherClients(List<Client> clientsList, Client CurrentClient, string message)
	{
		int i = 0;
		try
		{
			foreach (Client otherClient in clientsList)
			{
				if (otherClient == CurrentClient)
					continue;
					NetworkStream stream = otherClient.tcpClient.GetStream();
					StreamUtil.Write(stream, System.Text.Encoding.ASCII.GetBytes(message));
				i++;
			}
		}
		catch
        {
			Console.WriteLine("Unexisting client");
			//TCPServerSample.clients.RemoveAt(i);
			//To fix ASAP

		}
	}

	public static bool IsNameTaken(List<Client> clientsList, string TestedName)
	{
		foreach (Client otherClient in clientsList)
		{
			if (otherClient.pseudo == TestedName)
				return true;
		}
		return false;
	}

}

