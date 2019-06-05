using System.IO.Ports;

namespace System.IO
{
    public class GeckoStream
    {
        public SerialPort _port;

        public GeckoStream(SerialPort port)
        {
            (_port = port).Open();
        }

        public Stream _stream => _port.BaseStream;
    }
}