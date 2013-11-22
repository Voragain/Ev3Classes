using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ev3Libs
{
    public class Ev3BTMessaging
    {
        public enum MessageType
        {
            MessageNone = 0,
            MessageBool = 1,
            MessageNumber = 2,
            MessageText = 3
        }

        private Ev3BlueToothManager BlueToothManager;

        public void SendText(string msgTitle, string msg)
        {
        }

        public void SendNumber(string msgTitle, float msg)
        {
        }

        public void SendBool(string msgTitle, bool msg)
        {
        }

        
    }
}
