using System;
using System.Threading;

namespace FragmentServerWV.Exceptions
{
    [Serializable()]
    public class LobbyEmuCrashException : System.Exception
    {
        public LobbyEmuCrashException() : base() { }
        public LobbyEmuCrashException(string message) : base(message) { }
        public LobbyEmuCrashException(string message, System.Exception inner) : base(message, inner) { }

        public LobbyEmuCrashException(string message, Thread t, System.Exception inner) : base(message, inner)
        {
            Console.WriteLine("Lobby Emulator Crashed");

            if (t.IsAlive)
            {
                t.Abort();
            }
            
            foreach (GameClient client in Server.clients)
                if (!client._exited)
                    client.Exit();
            foreach (ProxyClient client in Server.proxies)
                if (!client._exited)
                    client.Exit();
            
            Log.Writeline("Server exited");

            Console.WriteLine("Restarting the Lobby Emulator ");
            
            Server.Start();
        }

        // A constructor is needed for serialization when an
        // exception propagates from a remoting server to the client.
        protected LobbyEmuCrashException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}