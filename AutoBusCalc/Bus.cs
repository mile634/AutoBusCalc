using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoBusCalc
{
    class Bus
    {
        public string Name { get; set; }
        public int iName { get; set; }
        public DateTime start { get; set; }
        public int cost { get; set; }
        public List<Connection> Connections;
        public List<int> stations;
        public List<Bus> connectedbusses;
        public Bus(int name)
        {
            Name = "Buss№" + name;
            iName = name;
            Connections = new List<Connection>();
            connectedbusses = new List<Bus>();
            stations = new List<int>();
        }
        public void print()
        {
            Console.WriteLine($"\nName: {Name} start: {start} cost: {cost}");
            Console.WriteLine(string.Join("-", stations.ToArray()));
            printConnections();
        }
        public void printConnections()
        {
            Console.WriteLine($"Connection of {this.Name}:");
            foreach (Connection a in Connections)
            {
                a.print();
            }
            foreach(Bus a in connectedbusses)
            {
                Console.WriteLine($"{this.Name} connected with {a.Name}");
            }
        }
        public void addBus(Bus a)
        {
            if (!connectedbusses.Contains(a)) connectedbusses.Add(a);
        }
        public Station getFirstStation()
        {
            return Program.getStationByNum(Connections.First().StationA);
        }
    }
}
