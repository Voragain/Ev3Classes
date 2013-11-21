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
            BlueToothConnection = new SerialPort();
            BlueToothConnection.ReadTimeout = 500;
            BlueToothConnection.WriteTimeout = 500;
        }

        public void SetConnectionPort(string name)
        {
            if (BlueToothConnection.IsOpen)
                throw new Ev3Exceptions.ConnectionError("Connection already open - close first");

            if (name == "")
            {
                bInit = false;
                return;
            }

            BlueToothConnection.PortName = name;
            bInit = true;
        }

        public void OpenConnection()
        {
            if (!bInit)
                throw new Ev3Exceptions.ConnectionError("No port defined");
            if (BlueToothConnection.IsOpen)
                throw new Ev3Exceptions.ConnectionError("Connection already open - close first");
            try
            {
                BlueToothConnection.Open();
            }
            catch (Exception e)
            {
                throw new Ev3Exceptions.ConnectionError("Connection error : " + e.Message);
            }
 

            if (!BlueToothConnection.IsOpen)
            {
                throw new Ev3Exceptions.ConnectionError("Connection failed, still closed");
            }
        }

        public void CloseConnection()
        {
            if (!BlueToothConnection.IsOpen)
                throw new Ev3Exceptions.ConnectionError("Connection is already closed");

            BlueToothConnection.Close();
        }

        public void SendString(string data)
        {
            if (!bInit)
            {
                throw new Ev3Exceptions.ConnectionError("Connection is closed, cannot proceed");
            }

            byte[] msgsize = new byte[2];
            msgsize[0] = 0;
            msgsize[1] = (byte)data.Length;
            try
            {
                BlueToothConnection.Write(msgsize, 0, 2);

                BlueToothConnection.Write(data.ToCharArray(), 0, data.Length);
            }
            catch (Exception e)
            {
                throw new Ev3Exceptions.TransmissionError("Transmission of \"" + data + "\" failed");
            }
        }

        public void SendInteger(int data)
        {
            if (!bInit)
            {
                throw new Ev3Exceptions.ConnectionPortMissing();
            }
        }

        public void SendBoolean(bool data)
        {
            if (!bInit)
            {
                throw new Ev3Exceptions.ConnectionPortMissing();
            }
        }

        public bool DataAvailable()
        {
            if (!BlueToothConnection.IsOpen)
                throw new Ev3Exceptions.ConnectionError("Connection closed, cannot proceed");

            return (BlueToothConnection.BytesToRead > 0);
        }

        public string ReceiveString()
        {
            if (!BlueToothConnection.IsOpen)
            {
                throw new Ev3Exceptions.ConnectionError("Connection is closed, cannot proceed");
            }

            return BlueToothConnection.ReadExisting();
        }

        public int ReceiveInt()
        {
            if (!BlueToothConnection.IsOpen)
            {
                throw new Ev3Exceptions.ConnectionClosed();
            }

            return 0;
        }

        public bool ReceiveBool()
        {
            if (!BlueToothConnection.IsOpen)
            {
                throw new Ev3Exceptions.ConnectionClosed();
            }

            return false;
        }
    }
}
