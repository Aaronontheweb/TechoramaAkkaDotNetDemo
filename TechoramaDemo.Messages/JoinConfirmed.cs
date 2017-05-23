using System;
using System.Collections.Generic;
using System.Text;

namespace TechoramaDemo.Messages
{
    public class JoinConfirmed
    {
        public JoinConfirmed(string roomName)
        {
            RoomName = roomName;
        }

        public string RoomName { get; }
    }
}
