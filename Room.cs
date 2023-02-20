using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


class Room
{
    public List<Client> clients;

    public string name;

    public Room(string roomName)
    {
        name = roomName;
        clients = new List<Client>();
    }
}

