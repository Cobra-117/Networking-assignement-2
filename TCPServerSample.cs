using System;
using System.Net.Sockets;
using System.Net;
using System.Collections.Generic;
using shared;
using System.Threading;

class TCPServerSample
{
	public static TcpListener listener;
	//public static List<Client> clients;
	public static List<Room> rooms;

	public static void Main(string[] args)
	{
		Console.WriteLine("Server started on port 55555");

		listener = new TcpListener(IPAddress.Any, 55555);
		listener.Start();

		//List<TcpClient> clients = new List<TcpClient>();
		//clients = new List<Client>();
		rooms = new List<Room>();
		rooms.Add(new Room("general"));

		while (true)
		{
			//First big change with respect to example 001
			//We no longer block waiting for a client to connect, but we only block if we know
			//a client is actually waiting (in other words, we will not block)
			//In order to serve multiple clients, we add that client to a list
			ManageNewClients();
			ManageCurrentClients();
			//Although technically not required, now that we are no longer blocking, 
			//it is good to cut your CPU some slack
			Thread.Sleep(100);
		}
	}

	public static void ManageNewClients()
	{
		string output;
		while (listener.Pending())
		{
			// newclientTcp = listener.AcceptTcpClient();
			Client newclient = new Client(listener.AcceptTcpClient(), rooms[0].clients.Count);
			rooms[0].clients.Add(newclient);
			Console.WriteLine("Accepted new client.");
			ServerUtilities.NotifyClient(newclient, "You joined the chat as " + newclient.pseudo);
			ServerUtilities.NotifyOtherClients(rooms[0].clients, newclient, newclient.pseudo + " joined the chat");
		}
	}

	public static void ManageCurrentClients()
	{
		//Second big change, instead of blocking on one client, 
		//we now process all clients IF they have data available
		//Foreach room
		//foreach (Room room in rooms)
		for (int i = 0; i < rooms.Count; i++)
		{
			Room room = rooms[i];
			//foreach (Client client in room.clients)
			for (int j = 0; j < room.clients.Count; j++)
			{
				if (room.clients[j].tcpClient.Available == 0) continue;
				NetworkStream stream = room.clients[j].tcpClient.GetStream();
				byte[] message = StreamUtil.Read(stream);
				string asciiMessage = System.Text.Encoding.ASCII.GetString(message);
				string output;
				//StreamUtil.Write(stream, StreamUtil.Read(stream));
				Console.WriteLine("got data");
				if (asciiMessage != null && asciiMessage[0] == '/')
					Commands.ManageCommands(room.clients[j], asciiMessage, room);
				else
				{
					ServerUtilities.NotifyClient(room.clients[j], "You: " + asciiMessage);
					room.clients = ServerUtilities.NotifyOtherClients(room.clients,
						room.clients[j], room.clients[j].pseudo + ": " + asciiMessage);
				}
			}
		}
	}
}


