using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Version2
{
    class Program
    {
        static string strbuf = "";
        static int ibuf = 0;
        static List<info> ways = new List<info>();
        static void Main(string[] args)
        {
            Initialize();
            foreach(Station sta in Station.allStations)
            {
                sta.print();
                sta.printBusses();
            }
            Console.ReadKey();
        }
        static void Initialize()
        {
            FileReader.readMarshrutes(@"E:\Marshrut2.txt");

            for (int i = 1; i <= Station.allStations.Count; i++)
            {
                Station temp = new Station(i);
                foreach (Bus a in Bus.allBusses)
                {
                    int j = 0;
                    foreach (Station b in a.connectedstations)
                        if (b == temp) { temp.connectedBusses.Add(a); if (j == 0) { temp.currentBus = a; j++; } }

                }
                Station.allStations.Add(temp);
                //Console.WriteLine($"Station: {temp.num} DefaultBus: {temp.currentBus.Name}");
            }
            foreach (Bus a in Bus.allBusses)
            {
                a.print();
                a.printConnections();
            }
            foreach (Station a in Station.allStations)
            {
                a.print();
            }
            /*foreach (Station a in GetStationsByBus(getBusByName(2)))
            {
                Console.WriteLine($"station for {getBusByName(2).Name}: {a.num}");

            }*/
            for (int i = 1; i <= Station.allStations.Count; i++)
            {
                //Console.WriteLine($"Busses on station {Bus.getBusByName(i).Name}:");
                Station.getStationByNum(i).fillStationBusses();
                Station.getStationByNum(i).printBusses();
                //Console.WriteLine();
            }
            //Console.WriteLine("_______________________________");
            //for (int i = 1; i < StationCount; i++) paths.Add(new Path(i));
            foreach (Bus a in Bus.allBusses)
            {
                foreach (Station b in Station.GetStationsByBus(a))
                    foreach (Bus c in Bus.getBussesbyBus(a)) if (!b.connectedBusses.Contains(c)) b.connectedBusses.Add(c);
            }

        }
        string getCheapestPath(List<Bus> visited, Station a, Station b, string rezult)
        {
            if (visited.Count == 0)
            {
                rezult += $"{a.num}-";
            }
            if (a.checktwost(b, out ibuf))
            {
                rezult += $"Connection confirmed: Station №{a.num} and Station №{b.num} are connected by Buss№{ibuf}\n The right way: "
                    + Bus.getBusByName(ibuf).getPathBetweenStOnMarsh(a,b);
                return rezult;
            }
            visited.Add(a.currentBus);
            Bus start = a.currentBus;
            Bus last = a.currentBus;
            Bus finish = b.currentBus;
            foreach(Bus tmpb in start.connectedbusses)
            {
                while(start!=finish)
                {
                    if(!visited.Contains(tmpb))
                    {                        
                        start = tmpb;
                        visited.Add(start);
                        getInfoByTrg(start.iName).target = start.iName;
                        getInfoByTrg(start.iName).previous = last.iName;
                        getCheapestPath(visited, start, finish, rezult);
                    }
                }
            }            
            rezult += $"{b.num}";
            return rezult;
        }
        string getCheapestPath(List<Bus> visited, Bus a, Bus b, string rezult)
        {
            Bus start = a;
            Bus last = a;
            Bus finish = b;
            foreach (Bus tmpb in start.connectedbusses)
            {
                while (start != finish)
                {
                    if (!visited.Contains(tmpb))
                    {
                        start = tmpb;
                        visited.Add(start);
                        getInfoByTrg(start.iName).target = start.iName;
                        getInfoByTrg(start.iName).previous = last.iName;
                        getCheapestPath(visited, start, finish, rezult);
                    }
                }
            }
            rezult += $"{b.iName}";
            return rezult;
        }
        static info getInfoByTrg(int trg)
        {
            foreach(info tmp in ways)
            {
                if (tmp.target == trg) return tmp;
            }
            return null;
        }
    }
    class info
    {
        public int target { get; set; }
        public int previous { get; set; }
    }
}
