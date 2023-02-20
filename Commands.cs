using System;
using System.Net.Sockets;
using System.Net;
using System.Collections.Generic;
using shared;
using System.Threading;



class Commands
{
	public static Client ManageCommands(Client client, string message, Room room)
	{
		string[] command = message.Split(' ');
		if (command.GetLongLength(0) == 0)
			return client;
		if (command[0] == "/setname" || command[0] == "/sn")
		{
			client = SetNameCommand(client, command, room);
		}
		else if (command[0] == "/help")
		{
			ServerUtilities.NotifyClient(client, "Command list:\n" +
			"/setname, /sn <nickname>: change nickname\n" +
			"/list: display the list of all connected clients\n" +
			"/listroom: display the list of clients in the room\n" +
			"/listrooms: display the list of existing rooms\n" +
			"/joinroom <room name>: join a room\n" +
			"/help: display help\n" +
			"/whisper, /w <nickname> <message> : whisper privately");
		}
		else if (command[0] == "/list")
			ListCommand(client);
		else if (command[0] == "/listroom")
			ListRoomCommand(client, room);
		else if (command[0] == "/listrooms")
			ListRoomsCommand(client);
		else if (command[0] == "/joinroom")
			JoinRoomCommand(client, command);
		else if (command[0] == "/whisper" || command[0] == "/w")
			WhisperCommand(client, command);
		else
			ServerUtilities.NotifyClient(client, "Unknown command");
		return client;
	}

	public static Client SetNameCommand(Client client, string[] command, Room room)
    {
		if (command.GetLongLength(0) < 2)
			ServerUtilities.NotifyClient(client, "You can't set an empty name");

		else if (ServerUtilities.IsNameTaken(room.clients, command[1].ToLower()))
			ServerUtilities.NotifyClient(client, "This nickname is already taken");
		else
		{
			ServerUtilities.NotifyClient(client, "Named changed to " + command[1].ToLower());
			ServerUtilities.NotifyOtherClients(room.clients,
				client, "Client " + client.pseudo +
				" changed the nickname to " + command[1].ToLower());
			client.pseudo = command[1].ToLower();
		}
		return client;
	}

	public static void ListCommand(Client client)
    {
		ServerUtilities.NotifyClient(client, "Connected clients:");
		foreach (Room room in TCPServerSample.rooms)
		{
			foreach (Client otherClient in room.clients)
			{
				ServerUtilities.NotifyClient(client, otherClient.pseudo);
			}
		}
    }

	public static void ListRoomCommand(Client client, Room room)
	{
		ServerUtilities.NotifyClient(client, "Connected clients:");
		foreach (Client otherClient in room.clients)
		{
			ServerUtilities.NotifyClient(client, otherClient.pseudo);
		}
	}

	public static void ListRoomsCommand(Client client)
	{
		ServerUtilities.NotifyClient(client, "Available rooms:");
		foreach (Room room in TCPServerSample.rooms)
		{
			ServerUtilities.NotifyClient(client, room.name);
		}
	}

	public static void WhisperCommand(Client client, string[] command)
	{
		Client target;
		string message = "";
		if (command.GetLongLength(0) < 2)
		{
			ServerUtilities.NotifyClient(client, "Usage: /whisper <nickname> <message>");
			return;
		}
		for (int i = 2; i < command.Length; i++)
        {
			message += command[i];
			message += " ";
        }
		foreach (Room room in TCPServerSample.rooms)
			foreach (Client otherClient in room.clients) {
				{
					if (otherClient.pseudo == command[1])
					{
						ServerUtilities.NotifyClient(otherClient,
							client.pseudo + " whispers " + message);
						ServerUtilities.NotifyClient(client,
							"You whisper to " + command[1] + " " + message);
						return;
					}
				}
			}
		ServerUtilities.NotifyClient(client, "Target " + command[1] + " does not exist");
	}

	public static void JoinRoomCommand(Client client, string[] command)
    {
		//Room Newroom;
		if (command.GetLongLength(0) < 2)
		{
			ServerUtilities.NotifyClient(client, "Usage: /joinroom <roomname>");
			return;
		}
		
		
		if (!RoomManagement.DoesRoomExist(command[1]))
			RoomManagement.CreateRoom(command[1]);
		foreach (Room room in TCPServerSample.rooms)
		{
			foreach (Client otherClient in room.clients)
			{
				if (otherClient == client)
                {
					ServerUtilities.NotifyClient(client, "You joined room " + command[1]);
					ServerUtilities.NotifyOtherClients(room.clients , client, client.pseudo + " left the room");
					RoomManagement.MoveClient(client, command[1]);
					Console.WriteLine("Moved client " + client.pseudo +
						"to " + command[1]);
					break;
                }
			}
		}

	}

}

