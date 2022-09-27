using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoBusCalc
{
    public class Station : ICloneable
    {
        static public int stationcount;
        public int num;
        public List<int> marshrutes;
        internal List<Bus> Busses;
        internal Bus currentBus;
        internal List<Station> nextStations;
        public Station(int n)
        {
            marshrutes = new List<int>();
            Busses = new List<Bus>();
            nextStations = new List<Station>();
            num = n;
        }
        public void print()
        {
            Console.WriteLine($"Station: {num} contains:");
            foreach (int a in marshrutes)
            {
                Console.Write($"{a}, ");
            }
            Console.WriteLine();
        }
        public object Clone()
        {
            return this.MemberwiseClone();
        }
        /*public void fillStationBusses()
        {
            foreach (int i in marshrutes)
            {
                Busses.Add(Program.getBusByName(i));
            }
        }*/
        public void printBusses()
        {
            foreach ( Bus a in Busses ) {
                Console.Write(a.Name + ",");    //qwe
            }
        }
    }
}
