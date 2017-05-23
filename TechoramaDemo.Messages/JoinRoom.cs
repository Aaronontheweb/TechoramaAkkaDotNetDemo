using System;
using System.Collections.Generic;
using System.Text;

namespace TechoramaDemo.Messages
{
    public class JoinRoom
    {
        public JoinRoom(string roomName, string userName)
        {
            RoomName = roomName;
            UserName = userName;
        }

        public string RoomName { get; }

        public string UserName { get; }
    }
}
