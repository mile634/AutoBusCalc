using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Version2
{
    public class Station
    {
        
        public static List<Station> allStations = new List<Station>();
        public int num;
        public List<Station> connectedStations;
        internal List<Bus> connectedBusses;
        internal Bus currentBus;
        public Station(int n)
        {
            connectedStations = new List<Station>();
            connectedBusses = new List<Bus>();
            num = n;
        }
        public void print()
        {
            Console.WriteLine($"Station: {num} contains:");
            foreach (Station sta in connectedStations)
            {
                Console.Write($"{sta.num}, ");
            }
            Console.WriteLine();
        }
        public object Clone()
        {
            return this.MemberwiseClone();
        }
        internal void AddBusToList(Bus input)
        {
            if (!connectedBusses.Contains(input)) connectedBusses.Add(input);
        }
        public void printBusses()
        {
            foreach (Bus a in connectedBusses) Console.Write(a.Name + ",");
        }
        public bool checktwost(Station b, out int input)
        {
            input = 0;
            foreach (Station sta in this.connectedStations)
                foreach (Station stb in b.connectedStations)
                {
                    if (sta == stb)
                    {
                        input = sta.num;
                        return true;
                    }
                }
            return false;
        }
        Station getCrossStation(Bus a, Bus b)
        {
            foreach (Station sta in this.connectedStations)
                foreach (Station stb in b.connectedstations)
                    if (sta == stb) return sta;
            return null;
        }
        public static List<Station> GetStationsByBus(Bus a)
        {
            List<Station> rezult = new List<Station>();
            foreach (Station sta in allStations)
            {
                if (a.connectedstations.Contains(sta)) rezult.Add(sta);
            }
            return rezult;
        }
        public static Station getStationByNum(int n)
        {
            foreach (Station a in allStations)
            {
                if (a.num == n) return a;
            }
            return null;
        }
        public void fillStationBusses()
        {
            foreach (Station i in connectedStations)
            {
                connectedBusses.Add(Bus.getBusByName(i.num));
            }
        }

    }
}
