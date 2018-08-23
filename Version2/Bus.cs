using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Version2
{
    public class Bus
    {
        internal static List<Connection> allConnections = new List<Connection>();
        public static List<Bus> allBusses = new List<Bus>();
        public string Name { get; set; }
        public int iName { get; set; }
        public DateTime start { get; set; }
        public int cost { get; set; }
        internal List<Connection> connections { get; set; }
        public List<Station> connectedstations { get; set; }
        public List<Bus> connectedbusses { get; set; }
        public Bus(int name)
        {
            Name = "Buss№" + name;
            iName = name;
            connections = new List<Connection>();
            connectedbusses = new List<Bus>();
            connectedstations = new List<Station>();
        }
        public void print()
        {
            Console.WriteLine($"\nName: {Name} start: {start} cost: {cost}");
            //Console.WriteLine(string.Join("-", connectedstations.ToArray()));
        }
        public void printConnections()
        {
            Console.WriteLine($"Connection of {Name}:");
            foreach (Connection a in connections) a.print();
        }
        public void addBus(Bus a)
        {
            if (!connectedbusses.Contains(a)) connectedbusses.Add(a);
        }
        public Station getNextStation(Station from)
        {
            foreach (Connection a in this.connections)
            {
                if (a.StationA == from) return a.StationB;
            }
            return null;
        }
        public string getPathBetweenStOnMarsh(Station a, Station b)
        {
            string rezult = "";
            while (a != b)
            {
                rezult += $"{a.num}-";
                a = getNextStation(a);
            }
            rezult += b.num;
            return rezult;
        }
        static public Bus getBusByName(int bn)
        {
            foreach (Bus a in allBusses)
            {
                if (a.Name.Equals("Buss№" + bn)) return a;
            }
            return null;
        }
        static public List<Bus> getBussesbyBus(Bus a)
        {
            List<Bus> rezult = new List<Bus>();
            foreach (Station tmpa in Station.GetStationsByBus(a))
            {
                foreach (Bus tmpb in getBussesByStation(tmpa))
                {
                    rezult.Add(tmpb);
                }
            }
            List<Bus> uniq = rezult.Distinct().ToList<Bus>();
            return uniq;
        }
        static public List<Bus> getBussesByStation(Station a)
        {
            List<Bus> rezult = new List<Bus>();
            foreach (Bus bus in allBusses)
            {
                if (bus.connectedstations.Contains(a)) rezult.Add(bus);
            }
            return rezult;
        }
    }
    class Connection
    {
        public Station StationA;
        public Station StationB;
        int time;
        public Connection(Station a, Station b, int timespend)
        {
            StationA = a;
            StationB = b;
            time = timespend;
        }
        public void print() =>
            Console.WriteLine($"From '{StationA.num}' to '{StationB.num}' for {time} minutes");

        
    }
}
