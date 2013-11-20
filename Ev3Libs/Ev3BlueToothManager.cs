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
        bool bInit;

        public Ev3BlueToothManager()
        {
            bInit = false;
        }

        public Ev3BlueToothManager(string portName)
        {
            BlueToothConnection = new SerialPort();
            BlueToothConnection.PortName = portName;
            BlueToothConnection.Open();
            BlueToothConnection.ReadTimeout = 500;
            BlueToothConnection.WriteTimeout = 500;
            bInit = true;
        }

        public void SendString(string data)
        {
            if (!bInit)
            {
                throw new Ev3Exceptions.ConnectionMissing();
            }
        }

        public void SendInteger(int data)
        {
            if (!bInit)
            {
                throw new Ev3Exceptions.ConnectionMissing();
            }
        }

        public void SendBoolean(bool data)
        {
            if (!bInit)
            {
                throw new Ev3Exceptions.ConnectionMissing();
            }
        }

        public string ReceiveString()
        {
            if (!bInit)
            {
                throw new Ev3Exceptions.ConnectionMissing();
            }

            return "";
        }

        public int ReceiveInt()
        {
            if (!bInit)
            {
                throw new Ev3Exceptions.ConnectionMissing();
            }

            return 0;
        }

        public bool ReceiveBool()
        {
            if (!bInit)
            {
                throw new Ev3Exceptions.ConnectionMissing();
            }

            return false;
        }
    }
}
