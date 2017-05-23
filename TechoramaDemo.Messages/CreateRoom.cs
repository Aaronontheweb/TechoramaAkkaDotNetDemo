using System;
using System.Collections.Generic;
using System.Text;

namespace TechoramaDemo.Messages
{
    public class CreateRoom
    {
        public CreateRoom(string userName, string roomName)
        {
            RoomName = roomName;
            UserName = userName;
        }

        public string UserName { get; }

        public string RoomName { get; }
    }
}
