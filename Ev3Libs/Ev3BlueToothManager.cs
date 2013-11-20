using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;

namespace Ev3Libs
{
    class Ev3BlueToothManager
    {
        SerialPort BlueToothConnection;

        public Ev3BTConnection(string portName)
        {
            BlueToothConnection = new SerialPort();
            BlueToothConnection.PortName = portName;
            BlueToothConnection.Open();
            BlueToothConnection.ReadTimeout = 500;
            BlueToothConnection.WriteTimeout = 500;
        }

        public void SendString(string data)
        {
        }

        public void SendInteger(int data)
        {
        }

        public void SendBoolean(bool data)
        {
        }

        public string ReceiveString()
        {
            return "";
        }

        public int ReceiveInt()
        {
            return 0;
        }

        public bool ReceiveBool()
        {
            return false;
            
        }
    }
}
