using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka.Actor;
using TechoramaDemo.Messages;

namespace TechoramaDemo.Host.Actors
{
    public class RoomManager : UntypedActor
    {
        private readonly Dictionary<IActorRef, string> _users = new Dictionary<IActorRef, string>();
        private readonly string _roomName;
        private readonly IActorRef _receptionist;

        public RoomManager(string roomName, IActorRef receptionist)
        {
            _roomName = roomName;
            _receptionist = receptionist;
        }

        protected override void OnReceive(object message)
        {
            switch (message)
            {
                case Say s:
                    Publish(s);
                    break;
                case JoinRoom j:
                    _users[Sender] = j.UserName;
                    Context.Watch(Sender);
                    Publish(new SystemMsg($"{j.UserName} has joined {_roomName}"));
                    break;
                case LeaveRoom l:
                    Context.Unwatch(Sender);
                    var user = _users[Sender];
                    Publish(new SystemMsg($"{user} has left {_roomName}"));
                    _users.Remove(Sender);
                    var left = new LeftRoom(_roomName);
                    _receptionist.Tell(left);
                    Sender.Tell(left);
                    break;
                case Terminated t: // client abruptly terminated
                    _users.Remove(t.ActorRef);
                    _receptionist.Tell(new LeftRoom(_roomName));
                    break;
                default:
                    Unhandled(message);
                    break;
            }
        }

        public void Publish(object msg)
        {
            foreach(var u in _users)
                u.Key.Tell(msg);
        }
    }
}
