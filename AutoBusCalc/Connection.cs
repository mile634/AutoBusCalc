using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoBusCalc
{
    class Connection
    {
        public int StationA;
        public int StationB;
        public int time;
        int cost;
        public Bus busBetwen;
        public Connection(int a, int b, int timespend, int moneyspend)
        {
            StationA = a;
            StationB = b;
            time = timespend;
            cost = moneyspend;
        }
        public void print()
        {
            Console.WriteLine($"From '{StationA}' to '{StationB}' for {time} minutes for {cost} rub");
        }
    }
}
