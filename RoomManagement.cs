using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



class RoomManagement
{
    public static void CreateRoom(string name)
    {
        TCPServerSample.rooms.Add(new Room(name));
        Console.WriteLine("Creating room " + name);
    }

    public static bool DoesRoomExist(string name)
    {
        foreach (Room room in TCPServerSample.rooms)
        {
            if (room.name == name)
                return true;
        }
        Console.WriteLine("room don't exist");
        return false;
    }

    public static Room FindRoom(string searchedRoom)
    {
        foreach (Room room in TCPServerSample.rooms)
        {
            if (room.name == searchedRoom)
                return room;
        }
        Console.WriteLine("Room " + searchedRoom + " not found");
        return null;
    }

    public static void MoveClient(Client client, string newRoomName)
    {
        Room newRoom = FindRoom(newRoomName);
        
        for (int i = 0; i < TCPServerSample.rooms.Count; i++)
        {
            for (int j = 0; j < TCPServerSample.rooms[i].clients.Count;
                j++)
            {
                if (TCPServerSample.rooms[i].clients[j] == client)
                    TCPServerSample.rooms[i].clients.RemoveAt(j);
            }
        }
        newRoom.clients.Add(client);
    }
}
