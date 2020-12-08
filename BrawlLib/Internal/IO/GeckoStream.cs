using System.IO;
using System.IO.Ports;

namespace BrawlLib.Internal.IO
{
    public class GeckoStream
    {
        public SerialPort _port;
        public Stream _stream => _port.BaseStream;

        public GeckoStream(SerialPort port)
        {
            (_port = port).Open();
        }
    }
}