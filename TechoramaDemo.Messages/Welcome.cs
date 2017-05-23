using System;
using System.Collections.Generic;
using System.Text;

namespace TechoramaDemo.Messages
{
    public sealed class Welcome
    {
        public Welcome(string userName, IReadOnlyDictionary<string, int> roomsCounts)
        {
            UserName = userName;
            RoomsCounts = roomsCounts;
        }

        public string UserName { get; }
        public IReadOnlyDictionary<string, int> RoomsCounts { get; }
    }
}
