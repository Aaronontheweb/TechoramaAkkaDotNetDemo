using System;
using Akka.Actor;
using TechoramaDemo.Host.Actors;
using TechoramaDemo.Messages;

namespace TechoramaDemo.Host
{
    class Program
    {
        static void Main(string[] args)
        {
            using(var system = ActorSystem.Create("chat-host"))
            {
                var receptionist = system.ActorOf(Props.Create(() => new Receptionist()), "Reception");
                receptionist.Ask<Err>(new Connect(null), TimeSpan.FromMilliseconds(100))
                    .ContinueWith(tr => Console.WriteLine(tr.Result.Reason));
                system.WhenTerminated.Wait();
            }
        }
    }
}
