using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using static AutoBusCalc.Program;

namespace AutoBusCalc
{

    class Program
    {
        static int BussCounter = 1;//readmarshrutes
        static int BussCount = 0;//readmarshrutes
        static int StationCount = 0;
        static List<Bus> Busses = new List<Bus>();
        static List<Station> Stations = new List<Station>();
        static List<Connection> allConnections = new List<Connection>();
        static int N;
        static public double[,] w;
        static double infinite = Double.PositiveInfinity;
        static double[] ways;
        static Path[] Paths = new Path[20];
        static int counter = 0; //счетчик путей
        static DateTime startingtime;
        static public Station getNextStation(Station from, Bus bus, out int minutes)
        {
            minutes = 0;
            foreach (Connection a in bus.Connections)
            {
                if (a.StationA == from.num) { minutes = a.time; return getStationByNum(a.StationB); }
            }
            return null;//*
        }

        static void printInfo()
        {
            foreach (Bus a in Busses)
            {
                a.print();
                a.printConnections();
            }
            foreach (Station a in Stations)
            {
                a.print();
            }
            foreach (Station a in GetStationsByBus(getBusByName(2)))
            {
                Console.WriteLine($"station for {getBusByName(2).Name}: {a.num}");
            }
            for (int i = 1; i <= StationCount; i++)
            {
                Console.WriteLine($"Busses on station {getStationByNum(i).num}:");
                getStationByNum(i).printBusses();
                Console.WriteLine();
            }
            foreach (Station a in Stations)
                foreach (Station b in a.nextStations)
                    Console.WriteLine($"{a.num} next possible station: {b.num}");
            //Console.WriteLine($"Path 6: {paths[6-1].Num} {paths[6-1].way}");
            foreach(Connection a in allConnections)
            {
                a.print();
            }
            PrintMatrica();
        }
        

        static void Initialize()
        {
            readMarshrutes(@"E:\Marshrut2.txt");
            for (int i = 1; i <= Station.stationcount; i++) Stations.Add(new Station(i));
            ways = new double[StationCount];
            Paths = new Path[20];
            for (int i = 0; i < N; i++) Paths[i] = new Path();
            ConvertConnsToArr();
            
        }
        static void ConvertConnsToArr()
        {
            w = new double[StationCount, StationCount];
            for(int i = 0; i<StationCount; i++)
                for (int j = 0; j < StationCount; j++)
                    w[i, j] = infinite;
            foreach (Connection tc in allConnections)
                w[tc.StationA - 1, tc.StationB - 1] = tc.time;
        }
        static void PrintMatrica()
        {
            for (int i = 0; i < StationCount + 1; i++) if (i == 0) Console.Write("---"); else Console.Write("{0,3}", i);
            Console.WriteLine();
            for (int i = 0; i < StationCount; i++)
            {
                Console.Write("{0,3}", i + 1);
                for (int j = 0; j < StationCount; j++)
                    if (w[i, j] == infinite) Console.Write("{0,3}", '-'); else Console.Write("{0,3}", w[i, j]);
                Console.WriteLine();
            }
        }
        static void Main(string[] args)
        {
            Initialize();
            printInfo();
            Console.WriteLine("Enter starting time if format 00:00 ");
            string[] enteredtime = Console.ReadLine().Split(':');
            startingtime = DateTime.Today;
            startingtime = startingtime.AddHours(Convert.ToInt32(enteredtime[0]));
            startingtime = startingtime.AddMinutes(Convert.ToInt32(enteredtime[1]));
            Console.WriteLine("Enter start point, finish point:");
            Station start = getStationByNum(Convert.ToInt32(Console.ReadLine()));
            Station finish = getStationByNum(Convert.ToInt32(Console.ReadLine()));
            dixtra(start.num);
            CalcPath(start.num, finish.num);
            for (int i = 0; i <= Paths[0].Length - 2; i++)
            {
                Paths[0].DeleteConnection(i);
                dixtra(start.num);
                CalcPath(start.num, finish.num);
                Paths[0].RestoreConnection();
            }
            getCheapestPath();
            getFastestPath(start.num,finish.num);
            
            Console.ReadLine();
        }
        
        class MyTime:ICloneable
        {
            public int hour;
            public int minute;
            public int gettotal()
            {
                return this.hour * 60 + this.minute;
            }
            public string getString()
            {                
                return $"{hour}:{minute}";
            }
            public MyTime addMinutes(int mins)
            {
                int totalminutes = hour * 60 + minute+mins;
                return new MyTime() { hour = totalminutes/60, minute = totalminutes%60 };
            }
            public object Clone()
            {
                return new MyTime { hour = this.hour, minute = this.minute };
            }
            public static MyTime operator -(MyTime c1, MyTime c2)
            {
                int a = c1.hour * 60 + c1.minute;
                int b = c2.hour * 60 + c2.minute;                
                return new MyTime { hour = (a-b)/60, minute = (a-b)%60 };
            }
            public static bool operator >=(MyTime c1, MyTime c2)
            {
                int t1 = c1.hour * 60 + c1.minute;
                int t2 = c2.hour * 60 + c2.minute;
                if (t1 <= t2) return true;
                return false;
            }
            public static bool operator <=(MyTime c1, MyTime c2)
            {
                int t1 = c1.hour * 60 + c1.minute;
                int t2 = c2.hour * 60 + c2.minute;
                if (t1 <= t2) return true;
                return false;
            }
            public static MyTime operator +(MyTime c1, MyTime c2)
            {
                int a = c1.hour * 60 + c1.minute;
                int b = c2.hour * 60 + c2.minute;
                return new MyTime { hour = (a + b) / 60, minute = (a + b) % 60 };
            }
        }

        static MyTime getPathTime(Path input)
        {
            MyTime start = new MyTime() { hour = startingtime.Hour, minute = startingtime.Minute };
            int first = input[input.Length - 1].Number;//получили id первой станции
            int second = input[input.Length - 2].Number;//получили id второй станции
            int minutes;
            Bus between = getBusBetwenTwo(first, second, out minutes);//первый автобус, который нужен            
            MyTime now = new MyTime() { hour = startingtime.Hour, minute = startingtime.Minute };
            waitingforbus(getStationByNum(first + 1), between, start, out now);
            now.addMinutes(minutes);
            //Console.WriteLine($"Transfer from {first+1} to {second+1} is done");
            Bus previous = between;
            for (int i = input.Length-2; i>=1; i--)
            {
                first = input[i].Number;
                second = input[i - 1].Number;
                minutes = 0;
                between = getBusBetwenTwo(first, second, out minutes);
                //Console.WriteLine($"Trying transfer from {first+1} to {second+1} on {between.Name}" +
                //    $" previous = {previous.Name}");
                if(between!=previous)
                {
                //    Console.WriteLine($"New bus!");
                    waitingforbus(getStationByNum(first + 1), between, (MyTime)now.Clone(), out now);
                    now = now.addMinutes(getMinutesBetweenTwo(first+1, second+1));
                }
                else
                {
                    now = now.addMinutes(getMinutesBetweenTwo(first+1,second+1));
                    //Console.WriteLine($"Just going next from {first+1} to {second+1} now: {now.getString()}");
                }
                previous = between;
            }
            return now;
        }
        static void waitingforbus(Station where, Bus targetbus, MyTime now, out MyTime rezult)
        { //возвращает в rezult время прибытия автобуса 
            //Console.WriteLine($"Begin wait on {where.num} for {targetbus.Name}. Now: {now.getString()}:");
            rezult = (MyTime)now.Clone();
            Station temp = targetbus.getFirstStation();            
            MyTime busstart = new MyTime() { hour = targetbus.start.Hour, minute = targetbus.start.Minute };
            rezult = (MyTime)busstart.Clone();
            //Console.WriteLine($"first station: {temp.num} Busstart: {rezult.getString()} WHERE: {where.num}");
            if (where.num == temp.num)
            {
                //rezult = now;
                while (true)
                {
                    int minutes = 0;
                    temp = getNextStation(temp, targetbus, out minutes);
                    rezult = rezult.addMinutes(minutes);
                    //Console.WriteLine($"Transmissed to {temp.num}. rezult: {rezult.getString()}");
                    if (temp.num == where.num && rezult.gettotal() >= now.gettotal()) break;
                }
            }
            else
            {
                if (now.gettotal() <= busstart.gettotal())
                {
                    while (true)
                    {
                        int minutes = 0;
                        temp = getNextStation(temp, targetbus, out minutes);
                        rezult = rezult.addMinutes(minutes);
                        //Console.WriteLine($"Transmissed to {temp.num}. rezult: {rezult.getString()}");
                        if (temp.num == where.num && rezult.gettotal() >= now.gettotal()) break;                        
                    }
                }
                else
                {
                    while (true)
                    {
                        int minutes = 0;
                        temp = getNextStation(temp, targetbus, out minutes);
                        rezult = rezult.addMinutes(minutes);
                        //Console.WriteLine($"Transmissed to {temp.num}. rezult: {rezult.getString()}" +
                        //$"Needed time: {now.getString()}");
                        if (temp.num != where.num && rezult.gettotal() >= now.gettotal()) break;
                    }
                }
            }

            //Console.WriteLine($"Wait finished, now: {rezult.getString()}");
        }

        static Path getFastestPath(int start, int finish)
        {
            Path rez = null;
            int rezid = 0;
            MyTime fromhome = new MyTime() { hour = startingtime.Hour, minute = startingtime.Minute };
            MyTime rezrezult = null;
            for (int j = 0; j < counter; j++)
            {
                if (rez == null) rez = Paths[j];
                Console.WriteLine($"Checking {j+1}");
                MyTime pathrezult = getPathTime(Paths[j]);
                rezrezult = getPathTime(rez);
                Console.WriteLine($"[Path] {j + 1} rezult: {pathrezult.getString()}");
                
                int first = Paths[j][Paths[j].Length - 1].Number;
                int second = Paths[j][Paths[j].Length - 2].Number;
                
                Bus pathstartBus = getBusBetwenTwo(first, second);
                MyTime startbusstart = new MyTime() { hour = pathstartBus.start.Hour, minute = pathstartBus.start.Minute };
                //Console.WriteLine($"Two first statioms on [Path] {j + 1}: {first + 1} and {second + 1}, Bus: {pathstartBus.Name}, Busstart: {pathstartBus.start.ToString()}");
                Console.WriteLine($"Wasted time for [Path] {j + 1} = {(pathrezult - fromhome).getString()}");
                if (pathrezult <= rezrezult) { rez = Paths[j]; rezid = j; }
            }
            
            Console.WriteLine($"The fastest path is {rezid+1}, wasted time: {(rezrezult-fromhome).getString()}");
            Paths[rezid].printRev();
            return rez;
        }

        static int getMinutesBetweenTwo(int a, int b)
        {
            foreach (Connection tc in allConnections)
                if (tc.StationA == a && tc.StationB == b) return tc.time;
            Console.WriteLine($"Connection betwen {a} and {b} not founded!");
            return 1111;
        }

        static MyTime wait(MyTime begin, int minutes)
        {
            MyTime rezult = begin.addMinutes(minutes);
            return rezult;
        }
        static MyTime wait(MyTime from, Bus bustowait, Station stationwhere)
        {
            MyTime now = from; //во сколько мы оказались на остановке
            MyTime rezult;
            MyTime temp = (MyTime)from.Clone();
            MyTime howmuch = new MyTime { hour = 0, minute = 0 };
            MyTime bustime = new MyTime() { hour = bustowait.start.Hour, minute = bustowait.start.Minute }; //начало движения автобуса
            //Console.WriteLine($"bustime start: {bustime.getString()}");
            if (bustime <= now)
            {
                Station tempStation = bustowait.getFirstStation();
                int minutes = 0;
                int minutessum = 0;
                while (tempStation.num != stationwhere.num && bustime <= now)
                {
                    tempStation = getNextStation(tempStation, bustowait, out minutes);
                    //Console.WriteLine($"minutes: {minutes}");
                    bustime = wait(bustime,minutes);
                    //Console.WriteLine($"BusTime: {bustime.getString()}, Now: {now.getString()}");
                    //minutessum += minutes;
                    //Console.WriteLine($"The next station of {bustowait.Name} is {tempStation.num}&&&&");
                }
                //temp = bustime - now;
            }
            else
            {
                now = bustime;
                Station tempStation = bustowait.getFirstStation();
                int minutes = 0;
                int minutessum = 0;
                while (tempStation.num != stationwhere.num)
                {
                    tempStation = getNextStation(tempStation, bustowait, out minutes);
                    //Console.WriteLine($"minutes: {minutes}");
                    bustime = wait(bustime,minutes);
                    //minutessum += minutes;
                    //Console.WriteLine($"ELSE: The next station of {bustowait.Name} is {tempStation.num} we need station: {stationwhere.num} BusTime: {bustime.getString()}");
                }
            }
            //Console.WriteLine($"Return value: {bustime.getString()}");
            now = (MyTime)bustime.Clone();
            return bustime;
        }

        static Path getCheapestPath()
        {
            Path rezult = null;
            int minsum = 1000;
            for (int j = 0; j < counter; j++)
            {
                if (rezult == null) rezult = Paths[j];
                List<Bus> PathBusses = new List<Bus>();
                for (int i = Paths[j].Length - 1; i >= 1; i--)
                {
                    Bus busBetwen = getBusBetwenTwo(Paths[j].data[i].Number, Paths[j].data[i - 1].Number);
                    if (!PathBusses.Contains(busBetwen)&&(busBetwen!=null))
                        PathBusses.Add(busBetwen);
                }
                Console.Write($"**************************\n[Path] {j+1}: ");
                Paths[j].printRev();
                Console.WriteLine($"Contains {PathBusses.Count} busses: ");
                int sum = 0;
                foreach (Bus a in PathBusses)
                {
                    Console.WriteLine($"{a.Name} cost: {a.cost}");
                    sum += a.cost;
                }
                if (sum < minsum) { rezult = Paths[j]; minsum = sum; }
                Console.WriteLine($"Total cost = {sum}");
            }
            for (int i = 0; i < counter; i++)
                if (Paths[i] == rezult)
                    Console.WriteLine($"**********The cheapest path is {i+1}**********");
            return rezult;
        }

        

        static void CalcPath(int start, int finish)
        {
            start -= 1;
            finish -= 1;
            Path tmp = new Path();
            List<Point> tmpp = new List<Point>();
            Point previous = new Point(finish, null);
            for (int i = 0; i < N; i++)
                if (ways[i] == 0 && w[start, i] != infinite) ways[i] = start; //little fix
            tmpp.Add(previous);
            while (finish != start)
            {
                finish = (int)ways[finish];
                previous = new Point(finish, previous);
                tmpp.Add(previous);
            }
            tmp.setArr(tmpp.ToArray());
            tmp.Length = tmpp.Count;
            if (!In(tmp))
                Paths[counter++] = tmp;
        }
        static Bus getBusBetwenTwo(int stA, int stB)  //0-21 нужен
        {
            Bus rezult = null;
            foreach (Connection tc in allConnections)
            {
                if (tc.StationA == stA + 1 && tc.StationB == stB + 1) return tc.busBetwen;
            }
            Console.WriteLine($"Cannot find connect between {stA + 1} and {stB + 1}");
            return rezult;
        }
        static Bus getBusBetwenTwo(int stA, int stB, out int minutes)  //0-21
        {
            Bus rezult = null;
            minutes = 0;
            foreach (Connection tc in allConnections)
            {
                if (tc.StationA == stA + 1 && tc.StationB == stB + 1) { minutes = tc.time; return tc.busBetwen; }
            }
            Console.WriteLine($"Cannot find connect between {stA + 1} and {stB + 1}");
            return rezult;
        }
        
        static List<Station> GetStationsByBus(Bus a)
        {
            List<Station> rezult = new List<Station>();
            foreach(Station sta in Stations)
            {
                if (a.stations.Contains(sta.num)) rezult.Add(sta);
            }
            return rezult;
        }
        static public List<Bus> getBussesByStation(Station a)
        {
            List<Bus> rezult = new List<Bus>();
            foreach(Bus bus in Busses)
            {
                if (bus.stations.Contains(a.num)) rezult.Add(bus);
            }
            return rezult;
        }
        static public List<Bus> getBussesbyBus(Bus a)
        {
            List<Bus> rezult = new List<Bus>();
            foreach(Station tmpa in GetStationsByBus(a))
            {
                foreach(Bus tmpb in getBussesByStation(tmpa))
                {
                    rezult.Add(tmpb);
                }
            }
            List<Bus> uniq = rezult.Distinct().ToList<Bus>();
            return uniq;
        }
        static public List<Bus> getBussesbyBus(int a)
        {
            List<Bus> rezult = new List<Bus>();
            foreach (Station tmpa in GetStationsByBus(getBusByName(a)))
            {
                foreach (Bus tmpb in getBussesByStation(tmpa))
                {
                    rezult.Add(tmpb);
                }
            }
            List<Bus> uniq = rezult.Distinct().ToList<Bus>();
            return uniq;
        }

        

        public static Station getStationByNum(int n)
        {
            foreach(Station a in Stations)
            {
                if (a.num == n) return a;
            }
            return null;
        }

        static public Bus getBusByName(int bn)
        {
            foreach(Bus a in Busses)
            {
                if (a.Name.Equals("Buss№" + bn)) return a;
            }
            return null;
        }

        static void dixtra(int st)
        {
            st -= 1;
            N = StationCount;
            double[] D = new double[N];
            bool[] visited = new bool[N];
            for (int i = 0; i < N; i++)
            {
                D[i] = w[st, i];
                visited[i] = false;
            }
            D[st] = infinite;
            int index = 0, u = 0;
            for (int i = 0; i < N; i++)
            {
                double min = infinite;
                for (int j = 0; j < N; j++)
                {
                    if (!visited[j] && D[j] < min)
                    {
                        min = D[j];
                        index = j;
                        //Console.WriteLine($"1st cycle: min setted {min}, index setted {index} i={i}, j={j}");
                    }
                }
                u = index;
                visited[u] = true;
                //Console.WriteLine($"Between cycles: i={i}, u setted {u}, visited[{u}] setted true");
                for (int j = 0; j < N; j++)
                {
                    if (!visited[j] && w[u, j] != infinite && D[u] != infinite && (D[u] + w[u, j] < D[j]))
                    {
                        D[j] = D[u] + w[u, j];
                        ways[j] = u;
                        //Console.WriteLine($"2nd cycle: u={u}, j={j}");
                    }
                }
            }
        }
        static public bool In(Path a)
        {
            for (int i = 0; i < Paths.Length; i++)
                if(Paths[i]!=null)
                    if (Paths[i].equal(a)) return true;
            return false;
        }

        static void readMarshrutes(string path)
        {            
            using (StreamReader fs = new StreamReader(path))
            {
                
                BussCount = Convert.ToInt32(fs.ReadLine());
                StationCount = Convert.ToInt32(fs.ReadLine());
                Station.stationcount = StationCount;
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
                    Busses.Add(tempBus);
                }
                temp = fs.ReadLine();
                string[] costs = temp.Split(' ');
                int i = 1;
                foreach (string a in costs)
                {
                    getBusByName(i++).cost = Convert.ToInt32(a);
                }
                i = 1;
                for (i = 1; i <= BussCount; i++)
                {
                    string ways = fs.ReadLine();
                    string[] values = ways.Split(' ');
                    int k = Convert.ToInt32(values[0]);
                    int n = 1;
                    int first = Convert.ToInt32(values[1]);
                    int last = Convert.ToInt32(values[values.Length/2]);
                    int max = values.Length;
                    for (int j = 0; j < k - 1; j++)
                    {
                        int stA = Convert.ToInt32(values[1 + j]);
                        if (!getBusByName(i).stations.Contains(Convert.ToInt32(values[1 + j]))) getBusByName(i).stations.Add(Convert.ToInt32(values[1 + j]));                        
                        int stB = Convert.ToInt32(values[2 + j]);
                        int time = Convert.ToInt32(values[k+1+j]);
                        Connection a = new Connection(stA, stB, time, getBusByName(i).cost);
                        Console.WriteLine($"Connect {stA} to {stB}");
                        getBusByName(i).Connections.Add(a);
                        a.busBetwen = getBusByName(i);
                        allConnections.Add(a);
                    }
                    Connection clast = new Connection(last, first,
                            Convert.ToInt32(values[values.Length-1]), getBusByName(i).cost);
                    clast.busBetwen = getBusByName(i);
                    Console.WriteLine($"Connect {last} to {first}");
                    allConnections.Add(clast);

                    if (!getBusByName(i).stations.Contains(last)) getBusByName(i).stations.Add(last);
                    getBusByName(i).Connections.Add(clast);
                }
            }
        }
    }        
}
