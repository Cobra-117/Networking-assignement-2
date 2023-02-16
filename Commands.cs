using System;
using System.Net.Sockets;
using System.Net;
using System.Collections.Generic;
using shared;
using System.Threading;



class Commands
{
	public static Client ManageCommands(Client client, string message)
	{
		string[] command = message.Split(' ');
		if (command.GetLongLength(0) == 0)
			return client;
		if (command[0] == "/setname" || command[0] == "/sn")
		{
			client = SetNameCommand(client, command);
		}
		else if (command[0] == "/help")
		{
			ServerUtilities.NotifyClient(client, "Command list:\n" +
			"/setname, /sn <nickname>: change nickname\n" +
			"/list: display the list of connected clients\n" +
			"/help: display help\n" +
			"/whisper, /w <nickname> <message> : whisper privately");
		}
		else if (command[0] == "/list")
			ListCommand(client);
		else if (command[0] == "/whisper" || command[0] == "/w")
			WhisperCommand(client, command);
		return client;
	}

	public static Client SetNameCommand(Client client, string[] command)
    {
		if (command.GetLongLength(0) < 2)
			ServerUtilities.NotifyClient(client, "You can't set an empty name");

		else if (ServerUtilities.IsNameTaken(TCPServerSample.clients, command[1].ToLower()))
			ServerUtilities.NotifyClient(client, "This nickname is already taken");
		else
		{
			ServerUtilities.NotifyClient(client, "Named changed to " + command[1].ToLower());
			ServerUtilities.NotifyOtherClients(TCPServerSample.clients,
				client, "Client " + client.pseudo +
				" changed the nickname to " + command[1].ToLower());
			client.pseudo = command[1].ToLower();
		}
		return client;
	}

	public static void ListCommand(Client client)
    {
		ServerUtilities.NotifyClient(client, "Connected clients:");
		foreach (Client otherClient in TCPServerSample.clients)
        {
			ServerUtilities.NotifyClient(client, otherClient.pseudo);
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
		foreach (Client otherClient in TCPServerSample.clients)
		{
			if (otherClient.pseudo == command[1])
            {
				ServerUtilities.NotifyClient(otherClient,
					client.pseudo + " whispers "+ message);
				ServerUtilities.NotifyClient(client,
					"You whisper to " + command[1]+ " " + message);
				return;
            }
		}
		ServerUtilities.NotifyClient(client, "Target " + command[1] + " does not exist");
	}

}

