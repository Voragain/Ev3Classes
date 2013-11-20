using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ev3Libs
{
    class Ev3Brick
    {
        Ev3BlueToothManager BlueToothManager;
        List<Ev3Peripheral> Peripherals;

        public Ev3Brick()
        {
            BlueToothManager = new Ev3BlueToothManager();
            Peripherals = new List<Ev3Peripheral>();
        }
    }
}
