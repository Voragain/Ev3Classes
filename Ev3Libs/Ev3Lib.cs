using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;

namespace Ev3Libs
{
    public class Ev3BTConnection
    {
        SerialPort BlueToothConnection;

        public Ev3BTConnection(string portName)
        {
            BlueToothConnection = new SerialPort();
            BlueToothConnection.PortName = portName;
            BlueToothConnection.Open();
            BlueToothConnection.ReadTimeout = 1500;
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

    public class Brick
    {
    }

    public class Device
    {
    }

    public class Sensor : Device
    {
    }

    public class Motor : Device
    {
    }

    public class Ev3Lib
    {
    }
}
