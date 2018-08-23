using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Version2
{
    public static class FileReader
    {
        public static void readMarshrutes(string path)
        {
            using (StreamReader fs = new StreamReader(path))
            {
                int BussCount, StationCount;
                int BussCounter = 1;
                BussCount = Convert.ToInt32(fs.ReadLine());
                StationCount = Convert.ToInt32(fs.ReadLine());
                for (int k = 1; k < StationCount; k++) Station.allStations.Add(new Station(k));
                string temp = fs.ReadLine();
                string[] arr = temp.Split(' ');
                foreach (string b in arr)
                {
                    Bus tempBus = new Bus(BussCounter++);
                    string[] hourminute = b.Split(':');
                    double hours = Convert.ToDouble(hourminute[0]);
                    double minutes = Convert.ToDouble(hourminute[1]);
                    DateTime tempDateTime = DateTime.Today;
                    tempDateTime = tempDateTime.AddHours(hours);
                    tempDateTime = tempDateTime.AddMinutes(minutes);
                    tempBus.start = tempDateTime;
                    Bus.allBusses.Add(tempBus);
                }
                temp = fs.ReadLine();
                string[] costs = temp.Split(' ');
                int i = 1;
                foreach (string a in costs)
                {
                    Bus.getBusByName(i++).cost = Convert.ToInt32(a);
                }
                i = 1;
                for (i = 1; i <= BussCount; i++)
                {
                    string ways = fs.ReadLine();
                    string[] values = ways.Split(' ');
                    int k = Convert.ToInt32(values[0]);
                    int n = 1;
                    int first = Convert.ToInt32(values[1]);
                    int last = Convert.ToInt32(values[values.Length / 2]);
                    int max = values.Length;
                    for (int j = 0; j < k - 1; j++)
                    {
                        int stA = Convert.ToInt32(values[1 + j]);
                        if (!Bus.getBusByName(i).connectedstations.Contains(Station.getStationByNum(Convert.ToInt32(values[1 + j])))) 
                            Bus.getBusByName(i).connectedstations.Add(Station.getStationByNum(Convert.ToInt32(values[1 + j])));
                        int stB = Convert.ToInt32(values[2 + j]);
                        int time = Convert.ToInt32(values[k + 1 + j]);
                        Connection a = new Connection(Station.getStationByNum(stA), Station.getStationByNum(stB), time);
                        //Console.WriteLine($"added connect near {stA} and {stB}");
                        Bus.allConnections.Add(a);
                        Bus.getBusByName(i).connections.Add(a);
                    }
                    Connection clast = new Connection(Station.getStationByNum(last), Station.getStationByNum(first),
                            Convert.ToInt32(values[values.Length - 1]));
                    if (!Bus.getBusByName(i).connectedstations.Contains(Station.getStationByNum(last))) Bus.getBusByName(i).connectedstations.Add(Station.getStationByNum(last));
                    Bus.getBusByName(i).connections.Add(clast);
                }
            }
        }
    }
}
