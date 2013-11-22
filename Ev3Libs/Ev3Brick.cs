using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ev3Libs
{
    public class Ev3Brick
    {
        
        public Ev3BTMessaging Messaging;
        Ev3BlueToothManager BlueToothManager;
        List<Ev3Peripheral> Peripherals;
        

        public Ev3Brick()
        {
            BlueToothManager = new Ev3BlueToothManager();
            Messaging = new Ev3BTMessaging(BlueToothManager);
            Peripherals = new List<Ev3Peripheral>();

            BlueToothManager.AddHandler(1, new Ev3BlueToothManager.BlueToothMessageHandler(Messaging.ReceiveMessage));
        }

        public void SetPort(string port)
        {
            BlueToothManager.SetConnectionPort(port);
        }

        public void Connect()
        {
            BlueToothManager.OpenConnection();
        }

        public void Close()
        {
            BlueToothManager.CloseConnection();
        }
   }
}
