namespace TechoramaDemo.Messages
{
    public class Connect
    {
        public Connect(string userName)
        {
            UserName = userName;
        }

        public string UserName { get; private set; }
    }
}
