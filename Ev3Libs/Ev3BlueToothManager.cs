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

        List<byte> InBuffer;

        bool bInit;

        public Ev3BlueToothManager()
        {
            bInit = false;
            BlueToothConnection = new SerialPort();
            BlueToothConnection.ReadTimeout = 500;
            BlueToothConnection.WriteTimeout = 500;
            BlueToothConnection.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
        }

        private void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;
            string data = sp.ReadExisting();

            foreach (char c in data.ToCharArray())
            {
                InBuffer.Add((byte)c);
            }
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

        public void SendString(string title, string msg)
        {
            if (!bInit)
            {
                throw new Ev3Exceptions.ConnectionError("Connection is closed, cannot proceed");
            }

            //title = title + (char)0;
            //msg = msg + (char)0;

            List<byte> buffer = new List<byte>();
            buffer.Add(0);
            buffer.Add(0);
            buffer.Add(1);
            buffer.Add(0);
            buffer.Add(0x81);
            buffer.Add(0x9e);
            buffer.Add((byte)(title.Length + 1));
            foreach (char c in title.ToCharArray())
            {
                buffer.Add((byte)c);
            }
            buffer.Add(0);

            buffer.Add((byte)(msg.Length + 1));
            buffer.Add(0);

            foreach (char c in msg.ToCharArray())
            {
                buffer.Add((byte)c);
            }
            buffer.Add(0);

            buffer[0] = (byte)(buffer.Count - 2);

            try
            {
                BlueToothConnection.Write(buffer.ToArray(), 0, buffer.Count);
            }
            catch (Exception e)
            {
                throw new Ev3Exceptions.TransmissionError("Transmission of \"" + title + "@" + msg + "\" failed");
            }
        }

        public void SendNumber(string title, float data)
        {
            if (!bInit)
            {
                throw new Ev3Exceptions.ConnectionError("Connection is closed, cannot proceed");
            }

            //title = title + (char)0;
            //msg = msg + (char)0;

            List<byte> buffer = new List<byte>();
            buffer.Add(0);
            buffer.Add(0);
            buffer.Add(1);
            buffer.Add(0);
            buffer.Add(0x81);
            buffer.Add(0x9e);
            buffer.Add((byte)(title.Length + 1));
            foreach (char c in title.ToCharArray())
            {
                buffer.Add((byte)c);
            }
            buffer.Add(0);

            buffer.Add(4);
            buffer.Add(0);


            foreach (byte b in BitConverter.GetBytes(data))
            {
                buffer.Add(b);
            }

            buffer[0] = (byte)(buffer.Count - 2);

            try
            {
                BlueToothConnection.Write(buffer.ToArray(), 0, buffer.Count);
            }
            catch (Exception e)
            {
                throw new Ev3Exceptions.TransmissionError("Transmission of \"" + title + "@" + data.ToString() + "\" failed");
            }
        }

        public void SendBoolean(string title, bool data)
        {
            if (!bInit)
            {
                throw new Ev3Exceptions.ConnectionError("Connection is closed, cannot proceed");
            }

            //title = title + (char)0;
            //msg = msg + (char)0;

            List<byte> buffer = new List<byte>();
            buffer.Add(0);
            buffer.Add(0);
            buffer.Add(1);
            buffer.Add(0);
            buffer.Add(0x81);
            buffer.Add(0x9e);
            buffer.Add((byte)(title.Length + 1));
            foreach (char c in title.ToCharArray())
            {
                buffer.Add((byte)c);
            }
            buffer.Add(0);

            buffer.Add(1);
            buffer.Add(0);
            if (data)
                buffer.Add(1);
            else
                buffer.Add(0);

            buffer[0] = (byte)(buffer.Count - 2);

            try
            {
                BlueToothConnection.Write(buffer.ToArray(), 0, buffer.Count);
            }
            catch (Exception e)
            {
                throw new Ev3Exceptions.TransmissionError("Transmission of \"" + title + "@" + data.ToString() + "\" failed");
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

        public int ReceiveByte()
        {
            if (!BlueToothConnection.IsOpen)
            {
                throw new Ev3Exceptions.ConnectionError("Connection is closed, cannot proceed");
            }

            return BlueToothConnection.ReadByte();
        }

        public Ev3BTMessaging.MessageType PeekMessageType()
        {
            if (InBuffer.Count == 0)
                return Ev3BTMessaging.MessageType.MessageNone;

            int NextMsgSize = (InBuffer[0] + (InBuffer[1] << 8));
            if (InBuffer.Count < (NextMsgSize + 2))
                return Ev3BTMessaging.MessageType.MessageNone;

            int msgSize = (InBuffer[InBuffer[6] + 7] + (InBuffer[InBuffer[6] + 8] >> 8));

            if (msgSize == 1)
                return Ev3BTMessaging.MessageType.MessageBool;

            if (msgSize == 4)
                if ((InBuffer[InBuffer[6] + 12] != 0) || (InBuffer[InBuffer[6] + 9] == 0))
                    return Ev3BTMessaging.MessageType.MessageNumber;

            return Ev3BTMessaging.MessageType.MessageText;

        }
    }
}
