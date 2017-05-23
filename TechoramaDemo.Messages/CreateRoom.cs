using System;
using System.Collections.Generic;
using System.Text;

namespace TechoramaDemo.Messages
{
    public class CreateRoom
    {
        public CreateRoom(string roomName)
        {
            RoomName = roomName;
        }

        public string RoomName { get; }
    }
}
