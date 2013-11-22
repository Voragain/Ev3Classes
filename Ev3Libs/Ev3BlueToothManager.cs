using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;

namespace Ev3Libs
{
    class Ev3BlueToothManager
    {
        public delegate bool BlueToothMessageHandler(Ev3BlueToothManager.RawMessage raw);

        public class RawMessage
        {
            public int MsgClass;
            public byte byte1;
            public byte byte2;
            public List<byte> Body;
        }

        Dictionary<int, BlueToothMessageHandler> MessageHandlers;

        SerialPort BlueToothConnection;

        List<byte> InBuffer = new List<byte>();

        bool bInit;

        public Ev3BlueToothManager()
        {
            bInit = false;
            BlueToothConnection = new SerialPort();
            BlueToothConnection.ReadTimeout = 500;
            BlueToothConnection.WriteTimeout = 500;
            BlueToothConnection.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);

            MessageHandlers = new Dictionary<int,BlueToothMessageHandler>();
        }

        public void AddHandler(int msgclass, BlueToothMessageHandler handler)
        {
            MessageHandlers.Add(msgclass, handler);
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

        private void ParseMessages()
        {
            do
            {
                int msgSize = (InBuffer[0] + (InBuffer[1] << 8));
                if (InBuffer.Count < (msgSize + 2))
                    return;

                RawMessage raw = new RawMessage();
                raw.MsgClass = InBuffer[2] + (InBuffer[3] << 8);
                raw.byte1 = InBuffer[4];
                raw.byte2 = InBuffer[5];
                raw.Body = InBuffer.GetRange(6, msgSize - 4);
                if (MessageHandlers.ContainsKey(raw.MsgClass))
                    MessageHandlers[raw.MsgClass].Invoke(raw);

                InBuffer.RemoveRange(0, msgSize + 2);
            } while (InBuffer.Count > 0);           
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

        public void SendMessage(RawMessage raw)
        {
            if (!bInit)
            {
                throw new Ev3Exceptions.ConnectionError("Connection is closed, cannot proceed");
            }

            List<byte> message = new List<byte>();

            int totalLen = raw.Body.Count + 6;
            message.Add((byte)(totalLen & 255));
            message.Add((byte)(totalLen >> 8));
            message.Add((byte)(raw.MsgClass & 255));
            message.Add((byte)(raw.MsgClass >> 8));
            message.Add(raw.byte1);
            message.Add(raw.byte2);
            foreach (byte b in raw.Body.ToArray())
            {
                message.Add(b);
            }


            BlueToothConnection.Write(message.ToArray(), 0, message.Count);

        }
    }
}
