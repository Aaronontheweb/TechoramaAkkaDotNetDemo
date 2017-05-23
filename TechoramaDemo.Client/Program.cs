using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka.Actor;

namespace TechoramaDemo.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var sys = ActorSystem.Create("chat-client"))
            {
                var serverAddress = Address.Parse(sys.Settings.Config.GetString("server-address"));
                sys.ActorOf(Props.Create(() => new ClientActor(serverAddress)));
                sys.WhenTerminated.Wait();
            }
        }
    }
}
