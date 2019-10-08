using System.IO.Pipes;

namespace StateApples
{
    class PipeDataExchange
    {
        public static void ServerForwardValue(string name, byte value)
        {
            using (var s = new NamedPipeServerStream(name))
            {
                s.WaitForConnection();
                s.WriteByte(value);
            }
        }
        public static void ClientForwardValue(string name, byte value)
        {
            using (var s = new NamedPipeClientStream(name))
            {
                s.Connect();
                s.WriteByte(value);
            }
        }
        public static byte ServerGetValue(string name)
        {
            using (var s = new NamedPipeServerStream(name))
            {
                s.WaitForConnection();
                return (byte)s.ReadByte();
            }
        }
        public static byte ClientGetValue(string name)
        {
            using (var s = new NamedPipeClientStream(name))
            {
                s.Connect();
                return (byte)s.ReadByte();
            }
        }
    }
}
