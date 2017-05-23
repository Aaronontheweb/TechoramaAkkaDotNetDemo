namespace TechoramaDemo.Messages
{
    public class LeftRoom
    {
        public LeftRoom(string roomName)
        {
            RoomName = roomName;
        }

        public string RoomName { get; private set; }
    }
}