using System;
using System.Collections.Generic;
using System.Text;

namespace TechoramaDemo.Messages
{
    /// <summary>
    /// Sent back to the end-user when we disallow their connect request
    /// </summary>
    public sealed class Err
    {
        public Err(string reason)
        {
            Reason = reason;
        }

        public string Reason { get; private set; }
    }
}
