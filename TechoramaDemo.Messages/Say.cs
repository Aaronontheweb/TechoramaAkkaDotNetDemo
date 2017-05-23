using System;
using System.Collections.Generic;
using System.Text;

namespace TechoramaDemo.Messages
{
    public class Say
    {
        public Say(string userName, string msg)
        {
            UserName = userName;
            Msg = msg;
        }

        public string UserName { get; }
        public string Msg { get; }

        public override string ToString()
        {
            return $"{UserName}: {Msg}"
        }
    }

    public class SystemMsg : Say
    {
        public SystemMsg(string msg) : base(string.Empty, msg)
        {
        }
    }
}
