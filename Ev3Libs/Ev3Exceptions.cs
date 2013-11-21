using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ev3Libs
{
    public class Ev3Exceptions
    {
        public class ConnectionError : System.Exception
        {
            public ConnectionError(string msg) : base(msg) {}
        }

        public class TransmissionError : System.Exception
        {
            public TransmissionError(string msg) : base(msg) {}
        }

        public class ConnectionClosed : System.Exception
        {
        }

        public class ConnectionAlreadyOpen : System.Exception
        {
        }

        public class ConnectionOpenFailed : System.Exception
        {
        }

        public class ConnectionTimeout : System.Exception
        {
        }
        
        public class ConnectionPortMissing : System.Exception
        {
        }
    }
}
