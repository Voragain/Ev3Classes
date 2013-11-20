using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ev3Libs
{
    class Ev3Exceptions
    {
        public class ConnectionTimeout : System.Exception
        {
        }

        public class ConnectionMissing : System.Exception
        {
        }
    }
}
