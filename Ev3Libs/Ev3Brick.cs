using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ev3Libs
{
    public class Ev3Brick
    {
        Ev3BlueToothManager BlueToothManager;
        List<Ev3Peripheral> Peripherals;

        public Ev3Brick()
        {
            BlueToothManager = new Ev3BlueToothManager();
            Peripherals = new List<Ev3Peripheral>();
        }

        public bool ConnectFromPort(string port)
        {
            try
            {
                BlueToothManager.SetConnectionPort(port);
                BlueToothManager.OpenConnection();
            }
            catch (Ev3Exceptions.ConnectionAlreadyOpen e)
            {
                Console.Error.WriteLine("ConnectFromPort : ConnectionAlreadyOpen");
                return false;
            }
            catch (Ev3Exceptions.ConnectionOpenFailed e)
            {
                Console.Error.WriteLine("ConnectFromPort : ConnectionOpenFailed");
                return false;
            }
            return true;
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

        public bool CheckHasData()
        {
            return BlueToothManager.DataAvailable();
        }

        public string GetStringData()
        {
            return BlueToothManager.ReceiveString();
        }

        public void SendStringData(string s)
        {
            BlueToothManager.SendString(s);
        }
    }
}
