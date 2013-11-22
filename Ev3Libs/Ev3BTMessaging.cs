using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ev3Libs
{
    public class Ev3BTMessaging
    {
        public class Message
        {
            public MessageType Type;
            public string Title;
            public object Value;
        }

        public enum MessageType
        {
            MessageNone = 0,
            MessageBool = 1,
            MessageNumber = 2,
            MessageText = 3
        }

        private Ev3BlueToothManager BTManager;
        private Dictionary<string, List<Message>> MessageBoxes;

        internal Ev3BTMessaging(Ev3BlueToothManager Manager)
        {
            BTManager = Manager;
            MessageBoxes = new Dictionary<string, List<Message>>();
        }

        internal bool ReceiveMessage(Ev3BlueToothManager.RawMessage raw)
        {
            Message parsed = new Message();
            int titleLen = raw.Body[0];
            parsed.Title = raw.Body.GetRange(2, titleLen).ToString();

            if (MessageBoxes.ContainsKey(parsed.Title))
            {
                MessageBoxes[parsed.Title].Add(parsed);
            }
            else
            {
                MessageBoxes.Add(parsed.Title, new List<Message>());
                MessageBoxes[parsed.Title].Add(parsed);
            }

            raw.Body.RemoveRange(0, titleLen + 1);
            int msgLen = raw.Body[0] + (raw.Body[1] << 8);
            if (msgLen == 1)
            {
                parsed.Type = MessageType.MessageBool;
                parsed.Value = (raw.Body[2] == 1);
                return true;
            }
            if (msgLen == 4)
                if ((raw.Body[5] != 0) || (raw.Body[2] == 0))
                {
                    parsed.Type = MessageType.MessageNumber;
                    parsed.Value = Convert.ToSingle(raw.Body.GetRange(2, 4).ToArray());
                    return true;
                }

            parsed.Type = MessageType.MessageText;
            parsed.Value = raw.Body.GetRange(2, msgLen).ToString();
            return true;
        }

        public void SendText(string msgTitle, string msg)
        {
            Ev3BlueToothManager.RawMessage raw = new Ev3BlueToothManager.RawMessage();
            raw.MsgClass = 1;
            raw.byte1 = 0x81;
            raw.byte2 = 0x9e;
            raw.Body = new List<byte>();

            int titleLen = msgTitle.Length;
            raw.Body.Add((byte)(titleLen));
            foreach (char c in msgTitle.ToCharArray())
            {
                raw.Body.Add((byte)c);
            }
            raw.Body.Add(0);

            int msgLen = msg.Length;
            raw.Body.Add((byte)(msgLen & 255));
            raw.Body.Add((byte)(msgLen >> 8));
            foreach (char c in msg.ToCharArray())
            {
                raw.Body.Add((byte)c);
            }
            raw.Body.Add(0);

            BTManager.SendMessage(raw);
        }

        public void SendNumber(string msgTitle, float msg)
        {
            Ev3BlueToothManager.RawMessage raw = new Ev3BlueToothManager.RawMessage();
            raw.MsgClass = 1;
            raw.byte1 = 0x81;
            raw.byte2 = 0x9e;
            raw.Body = new List<byte>();

            int titleLen = msgTitle.Length;
            raw.Body.Add((byte)(titleLen));
            foreach (char c in msgTitle.ToCharArray())
            {
                raw.Body.Add((byte)c);
            }
            raw.Body.Add(0);

            int msgLen = 4;
            raw.Body.Add((byte)(msgLen & 255));
            raw.Body.Add((byte)(msgLen >> 8));
            foreach (char c in BitConverter.GetBytes(msg))
            {
                raw.Body.Add((byte)c);
            }

            BTManager.SendMessage(raw);
        }

        public void SendBool(string msgTitle, bool msg)
        {
            Ev3BlueToothManager.RawMessage raw = new Ev3BlueToothManager.RawMessage();
            raw.MsgClass = 1;
            raw.byte1 = 0x81;
            raw.byte2 = 0x9e;
            raw.Body = new List<byte>();

            int titleLen = msgTitle.Length;
            raw.Body.Add((byte)(titleLen));
            foreach (char c in msgTitle.ToCharArray())
            {
                raw.Body.Add((byte)c);
            }
            raw.Body.Add(0);

            int msgLen = 1;
            raw.Body.Add((byte)(msgLen & 255));
            raw.Body.Add((byte)(msgLen >> 8));
            raw.Body.Add((byte)(msg?1:0));

            BTManager.SendMessage(raw);
        }

        public Message ReadFirstMsgFromBox(string title)
        {
            if (!MessageBoxes.ContainsKey(title))
                return null;

            if (MessageBoxes[title].Count == 0)
                return null;

            Message msg = MessageBoxes[title][0];
            MessageBoxes[title].RemoveAt(0);

            return msg;
        }
    }
}
