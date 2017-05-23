namespace TechoramaDemo.Messages
{
    public class SystemMsg : Say
    {
        public SystemMsg(string msg) : base(string.Empty, msg)
        {
        }

        public override string ToString()
        {
            return $"{Msg}";
        }
    }
}