using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka.Actor;
using TechoramaDemo.Messages;

namespace TechoramaDemo.Client
{
    public class ClientActor : UntypedActor, IWithUnboundedStash
    {
        private readonly Address _addr;
        private string _name;

        private IActorRef receiptionist;
        private IActorRef chatActor;

        private class Start { }

        private class ConnectFailed { }

        protected override void PreStart()
        {
            Self.Tell(new Start());
        }

        public ClientActor(Address addr)
        {
            _addr = addr;
        }

        protected override void OnReceive(object message)
        {
            switch (message)
            {
                case Start s:
                    Console.WriteLine("What's your name?");
                    while (string.IsNullOrEmpty(_name = Console.ReadLine())) { }
                    Context.ActorSelection(new RootActorPath(_addr) / "user" / "Reception")
                        .Tell(new Connect(_name));
                    Context.SetReceiveTimeout(TimeSpan.FromSeconds(3));
                    Context.Become(Connecting);
                    Stash.UnstashAll();
                    break;
                default: 
                    Stash.Stash();
                    break;
            }
        }

        void Connecting(object message)
        {
            switch (message)
            {
                case ReceiveTimeout f:
                    Console.WriteLine($"Failed to connect to {_addr}. Press any enter to exit.");
                    Console.ReadLine();
                    Context.System.Terminate();
                    break;
                case Err e:
                    Console.WriteLine($"Error connecting to {_addr}. {e.Reason}. Press any enter to exit.");
                    Console.ReadLine();
                    Context.System.Terminate();
                    break;
                case Welcome w:
                    receiptionist = Sender;
                    Console.WriteLine("Connected! Current rooms.");
                    foreach (var r in w.RoomsCounts)
                    {
                        Console.WriteLine($"{r.Key}: {r.Value} members");
                    }
                    Console.WriteLine("Type /join [roomname] to join a room, or /create [roomname] to create a new room.");
                    Context.Become(Connected);
                    Stash.UnstashAll();
                    Self.Tell(new Start());
                    Context.SetReceiveTimeout(null);
                    break;
                default:
                    Stash.Stash();
                    break;
            }
        }

        void Connected(object message)
        {
            switch (message)
            {
                case Start s:
                    var joinCmd = Console.ReadLine().Split(' ');
                    if (joinCmd.Length != 2 
                        || (!joinCmd[0].ToLowerInvariant().Equals("/join") 
                        && !joinCmd[0].ToLowerInvariant().Equals("/create")))
                    {
                        Console.WriteLine("Invalid input.");
                        Console.WriteLine("Type /join [roomname] to join a room, or /create [roomname] to create a new room.");
                        Self.Tell(s);
                        return;
                    }

                    switch (joinCmd[0].ToLowerInvariant())
                    {
                        case "/join":
                            receiptionist.Tell(new JoinRoom(joinCmd[1], _name));
                            break;
                        case "/create":
                            receiptionist.Tell(new CreateRoom(joinCmd[1], _name));
                            break;
                    }
                    break;
                case JoinConfirmed confirmed:
                    Console.WriteLine($"Joined {confirmed.RoomName}");
                    chatActor = Sender;
                    Context.Become(InChat);
                    Context.ActorOf(Props.Create(() => new ConsoleReader(_name, Self, Sender)));
                    Stash.UnstashAll();
                    break;
                case Err e:
                    Console.WriteLine($"Error trying to join room. {e.Reason}");
                    Console.WriteLine("Type /join [roomname] to join a room, or /create [roomname] to create a new room.");
                    Self.Tell(new Start());
                    break;
                default:
                    Stash.Stash();
                    break;
            }
        }

        void InChat(object message)
        {
            switch (message)
            {
                case Say s:
                    Console.WriteLine(s);
                    break;
                case LeftRoom l:
                    Console.WriteLine($"Left {l.RoomName}");
                    Context.Become(Connected);
                    Self.Tell(new Start());
                break;
                default:
                    Unhandled(message);
                    break;
            }
        }

        public IStash Stash { get; set; }
    }
}
