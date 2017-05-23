using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka.Actor;
using TechoramaDemo.Messages;

namespace TechoramaDemo.Client
{
    public class ConsoleReader : UntypedActor
    {
        public class Go { }

        private readonly string _userName;
        private readonly IActorRef _clientActorRef;
        private readonly IActorRef _remoteChatActor;

        protected override void PreStart()
        {
            Self.Tell(new Go());
        }

        public ConsoleReader(string userName, IActorRef clientActorRef, IActorRef remoteChatActor)
        {
            _userName = userName;
            _clientActorRef = clientActorRef;
            _remoteChatActor = remoteChatActor;
        }

        protected override void OnReceive(object message)
        {
            switch (message)
            {
                case Go g:
                    var s = Console.ReadLine();
                    Self.Tell(s);
                    break;
                case string str when str.StartsWith("/leave"):
                    _remoteChatActor.Tell(LeaveRoom.Instance, _clientActorRef);
                    Context.Stop(Self);
                    break;
                case string str when str.StartsWith("/exit"):
                    Context.System.Terminate();
                    break;
                case string str:
                    _remoteChatActor.Tell(new Say(_userName, str), _clientActorRef);
                    Self.Tell(new Go());
                    break;
            }
        }
    }
}
