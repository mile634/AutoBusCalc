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
        static public List<Connection> allConnections = new List<Connection>();
        static int N;
        static public double[,] w;
        static double infinite = Double.PositiveInfinity;
        static double[] ways;
        static Path[] Paths;
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
            foreach (Bus a in Busses) a.print();
            foreach (Station a in Stations) a.print();
            foreach (Connection a in allConnections) a.print();
            PrintMatrica();
        }
        static void test()
        {
            Console.WriteLine("[test] Enter starting time in format 00:00");
            string input = Console.ReadLine();
            if ((input.Length == 5) && (input[2].Equals(':')))
            {
                string[] enteredtime = input.Split(':');
                startingtime = DateTime.Today;
                int rezlt = 0;
                if (Int32.TryParse(enteredtime[0], out rezlt))
                {
                    startingtime = startingtime.AddHours(rezlt);
                    if (Int32.TryParse(enteredtime[1], out rezlt))
                    {
                        startingtime = startingtime.AddMinutes(rezlt);
                        for (int i = 1; i <= StationCount; i++)
                        {
                            Console.WriteLine($"Checking from {i} to others");
                            for (int j = 1; j <= StationCount; j++)
                            {
                                if (j % 10 == 0) Console.ReadKey();
                                if (i != j)
                                {
                                    Console.WriteLine("/͞͞͞͞͞͞͞͞͞͞͞͞͞͞͞͞͞͞͞͞͞͞͞͞͞͞͞͞͞͞͞͞͞͞͞͞͞͞͞͞͞͞͞\\");
                                    Console.WriteLine($"Checking {i} and {j}");
                                    ways = new double[StationCount];
                                    Paths = new Path[50];
                                    for (int k = 0; k < N; k++) Paths[k] = new Path();
                                    counter = 0;
                                    CalcAllPaths(i, j);
                                    Console.WriteLine("\\͟͟͟͟͟͟͟͟͟͟͟͟͟͟͟͟͟͟͟͟͟͟͟͟͟͟͟͟͟͟͟͟͟͟͟͟͟͟͟͟͟͟͟/");
                                }
                            }
                        }
                    }
                    else Console.WriteLine("Wrong minutes entered.");
                }
                else Console.WriteLine("Wrong hours entered.");
            }
            else Console.WriteLine("Wrong starting time entered");
            Console.ReadLine();
        }


        static bool Initialize()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine(@"Enter input file in format C:\folder\file.txt");
            string filepath = Console.ReadLine();
            bool ok = readMarshrutes(@filepath);
            if (ok)
            {
                for (int i = 1; i <= Station.stationcount; i++) Stations.Add(new Station(i));
                ways = new double[StationCount];
                Paths = new Path[50];
                for (int i = 0; i < N; i++) Paths[i] = new Path();
                ConvertConnsToArr();
            }
            else return false;
            return true;
        }
        static void ConvertConnsToArr()
        {
            w = new double[StationCount, StationCount];
            for (int i = 0; i < StationCount; i++)
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
        /*static Path getMultiPath(Path input)
        {
            Path rezult = null;

            return rezult;
        }*/
        static void Main(string[] args)
        {
            if (Initialize())
            {
                //printInfo(); //для вывода входных данных            
                Console.WriteLine("Enter starting time in format 00:00 or 'test'");
                string input = Console.ReadLine();
                if (input == "test") test();
                else
                {
                    if ((input.Length == 5) && (input[2].Equals(':')))
                    {
                        string[] enteredtime = input.Split(':');
                        startingtime = DateTime.Today;
                        bool succes = false;
                        int rezlt = 0;
                        if (Int32.TryParse(enteredtime[0], out rezlt))
                        {
                            startingtime = startingtime.AddHours(rezlt);
                            if (Int32.TryParse(enteredtime[1], out rezlt))
                            {
                                startingtime = startingtime.AddMinutes(rezlt);
                                Console.WriteLine($"Enter start and finish point in format: 1..{StationCount}");
                                int start = 0;
                                int finish = 0;
                                bool first = (Int32.TryParse((Console.ReadLine()), out start) && start <= StationCount) && start > 0;
                                bool second = (Int32.TryParse(Console.ReadLine(), out finish) && finish <= StationCount) && finish > 0;
                                if (first && second)
                                    CalcAllPaths(start, finish);
                                else Console.WriteLine("Wrong start/finish entered");
                            }
                            else Console.WriteLine("Wrong minutes entered.");
                        }
                        else Console.WriteLine("Wrong hours entered.");
                    }
                    else
                    {
                        Console.WriteLine("Wrong starting time entered.");
                    }
                    Console.ReadLine();
                }
            }
            else { Console.WriteLine("Error when openning file"); Console.ReadLine(); }
        }

        static void CalcAllPaths(int start, int finish)
        {
            dixtra(start);
            CalcPath(start, finish);
            //for (int j = 0; j < counter; j++)//с этим уходит в бесконечный цикл иногда
            for (int i = 0; i <= Paths[0].Length - 2; i++)
            {
                Paths[0].DeleteConnection(i);
                dixtra(start);
                CalcPath(start, finish);
                Paths[0].RestoreConnection();
            }
            getCheapestPath();
            getFastestPath();
        }
        /*static Connection getNthConnection(int stA, int stB, int n)
        {
            int counter = 0;
            foreach (Connection con in allConnections)
            {
                if (con.StationA == stA && con.StationB == stB) counter++;
                if (counter == n) return con;
            }
            return null;
        }*/

        static MyTime getPathTime(Path input)
        {
            MyTime start = new MyTime() { hour = startingtime.Hour, minute = startingtime.Minute };
            int first = input[input.Length - 1].Number;//получили id первой станции
            int second = input[input.Length - 2].Number;//получили id второй станции
            int minutes;
            Bus between = getBusBetwenTwo(first, second, out minutes);//первый автобус, который нужен    
            MyTime now = new MyTime() { hour = startingtime.Hour, minute = startingtime.Minute };
            waitingforbus(getStationByNum(first + 1), between, start, out now);
            //Console.WriteLine($" time betwen {first + 1} and {second + 1} = {minutes}");
            //Console.WriteLine($"Transfer from {first + 1} to {second + 1} on {between.Name} for {minutes} minutes");
            now = now.addMinutes(minutes);
            //Console.WriteLine($"Transfer from {first + 1} to {second + 1} is done");
            Bus previous = between;
            for (int i = input.Length - 2; i >= 1; i--)
            {
                first = input[i].Number;
                second = input[i - 1].Number;
                minutes = 0;
                between = getBusBetwenTwo(first, second, out minutes);
                //Console.WriteLine($"Transfer from {first + 1} to {second + 1} on {between.Name} for {minutes} minutes");
                   //$" previous = {previous.Name}");
                if (between != previous)
                {
                    //Console.WriteLine($"New bus!");
                    waitingforbus(getStationByNum(first + 1), between, (MyTime)now.Clone(), out now);
                    now = now.addMinutes(getMinutesBetweenTwo(first + 1, second + 1));
                }
                else
                {
                    now = now.addMinutes(getMinutesBetweenTwo(first + 1, second + 1));
                    //Console.WriteLine($"Just going next from {first + 1} to {second + 1} now: {now.getString()}");
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
            if (where.num == temp.num&&now.gettotal()<=busstart.gettotal())
            {
                //rezult = now;
                
            }
            else if(where.num == temp.num)
            {
                while (true)
                {
                    //Console.WriteLine("waitingfrobus");
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
                        //Console.WriteLine("waitingfrobus else");
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
                        //Console.WriteLine("waitingfrobus else else");
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

        static Path getFastestPath()
        {
            Path rez = null;
            int rezid = 0;
            MyTime fromhome = new MyTime() { hour = startingtime.Hour, minute = startingtime.Minute };
            MyTime rezrezult = getPathTime(Paths[0]);
            Console.WriteLine("[   ]");
            if (counter == 1)
            {
                rez = Paths[0];
                MyTime pathrezult = rezrezult;
                //Console.WriteLine(rezrezult.getString() + " " + fromhome.getString());
                if (pathrezult.gettotal() >= 24 * 60) Console.WriteLine($"[!] There are only one path from {rez[rez.Length - 1].Number + 1} to {rez[0].Number + 1} and it is too late. Wasted time: {(rezrezult - fromhome).getString()}. Way complete at {rezrezult.getString()}");
                else
                    Console.WriteLine($"[!] There are only one path from {rez[rez.Length - 1].Number + 1} to {rez[0].Number + 1}, wasted time: {(rezrezult - fromhome).getString()}. Way complete at {rezrezult.getString()}");
                Paths[rezid].printRev();
            }
            else
            {
                for (int j = 0; j < counter; j++)
                {
                    if (rez == null) rez = Paths[j];
                    //Console.WriteLine($" [ ] Checking {j + 1}");
                    MyTime pathrezult = getPathTime(Paths[j]);
                    if (pathrezult.gettotal() >= 24 * 60) Console.WriteLine($" [ ] Too late for Path {j + 1}, wasted time: {(pathrezult - fromhome).getString()}. Way complete at {pathrezult.getString()}");
                    else if (pathrezult != null)
                    {
                        //rezrezult = getPathTime(rez);

                        Console.WriteLine($" [ ] Path {j + 1} rezult: {pathrezult.getString()}");

                        int first = Paths[j][Paths[j].Length - 1].Number;
                        int second = Paths[j][Paths[j].Length - 2].Number;

                        Bus pathstartBus = getBusBetwenTwo(first, second);
                        MyTime startbusstart = new MyTime() { hour = pathstartBus.start.Hour, minute = pathstartBus.start.Minute };
                        Console.WriteLine($" [ ] Wasted time for Path {j + 1} = {(pathrezult - fromhome).getString()}");
                        if (pathrezult <= rezrezult) { rez = Paths[j]; rezid = j; rezrezult = pathrezult; }
                    }
                }
                Console.WriteLine($"[!] The fastest path is {rezid + 1}, wasted time: {(rezrezult - fromhome).getString()}. Way complete at {rezrezult.getString()}");
                Paths[rezid].printRev();
            }
            return rez;
        }
        static int getMinutesBetweenTwo(int a, int b)
        {
            int rezult = 1000;
            foreach (Connection tc in allConnections)
                if (tc.StationA == a && tc.StationB == b&& tc.time<rezult) rezult = tc.time;
            if(rezult == 1000) Console.WriteLine($"Connection betwen {a} and {b} not founded!");
            return rezult;
        }
        static Path getCheapestPath()
        {
            Path rezult = null;
            int minsum = 1000;
            int rezid = 0;
            for (int j = 0; j < counter; j++)
            {
                Console.WriteLine("[   ]");
                if (rezult == null) rezult = Paths[j];
                List<Bus> PathBusses = new List<Bus>();
                for (int i = Paths[j].Length - 1; i >= 1; i--)
                {
                    Bus busBetwen = getBusBetwenTwo(Paths[j].data[i].Number, Paths[j].data[i - 1].Number);
                    if (!PathBusses.Contains(busBetwen) && (busBetwen != null))
                        PathBusses.Add(busBetwen);
                }
                Console.Write($" [ ] Path {j + 1}: ");
                Paths[j].printRev();
                Console.WriteLine($" [ ] Contains {PathBusses.Count} busses: ");
                int sum = 0;
                foreach (Bus a in PathBusses)
                {
                    Console.WriteLine($" [ ] {a.Name} cost: {a.cost}");
                    sum += a.cost;
                }
                if (sum < minsum) { rezult = Paths[j]; minsum = sum; rezid = j; }
                Console.WriteLine($" [ ] Total cost = {sum}");
            }
            Console.WriteLine($"[!] The cheapest path is {rezid + 1}\n [~] But the most cheapest way is to go for a walk");
            return rezult;
        }
        static void CalcPath(int start, int finish)
        {
            start -= 1;
            finish -= 1;
            Path temppath = new Path();
            int localpathcounter = 0;
            Path[] temppaths = new Path[10];
            List<Point> tempway = new List<Point>();
            Point previous = new Point(finish, null);
            for (int i = 0; i < N; i++)
                if (ways[i] == 0 && w[start, i] != infinite) ways[i] = start; //little fix
            tempway.Add(previous);
            while (finish != start)
            {
                //Console.WriteLine($"{finish+1} {start+1}"); //сдесь уходит в бесконечность, если в test()
                finish = (int)ways[finish];
                previous = new Point(finish, previous);
                tempway.Add(previous);
            }
            temppath.setArr(tempway.ToArray());
            temppath.Length = tempway.Count;
            if (!In(temppath))
                Paths[counter++] = temppath;
        }
        static List<Connection> getListFromPoints(Point[] input)
        {
            List<Connection> rezult = new List<Connection>();
            return rezult;
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
        static Bus getBusBetwenTwo(int stA, int stB, out int minutes)  //0-21 нужен
        {
            Bus rezult = null;
            minutes = 1000;
            foreach (Connection tc in allConnections)
            {
                if (tc.StationA == stA + 1 && tc.StationB == stB + 1&&tc.time<=minutes) { minutes = tc.time; rezult = tc.busBetwen; }
            }
            if(rezult==null) Console.WriteLine($"Cannot find connect between {stA + 1} and {stB + 1}");
            return rezult;
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
        static public bool In(Path a)
        {
            for (int i = 0; i < Paths.Length; i++)
                if(Paths[i]!=null)
                    if (Paths[i].equal(a)) return true;
            return false;
        }
        static bool readMarshrutes(string path)
        {
            bool ok = true;
            try
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
                        int last = Convert.ToInt32(values[values.Length / 2]);
                        int max = values.Length;
                        for (int j = 0; j < k - 1; j++)
                        {
                            int stA = Convert.ToInt32(values[1 + j]);
                            if (!getBusByName(i).stations.Contains(Convert.ToInt32(values[1 + j]))) getBusByName(i).stations.Add(Convert.ToInt32(values[1 + j]));
                            int stB = Convert.ToInt32(values[2 + j]);
                            int time = Convert.ToInt32(values[k + 1 + j]);
                            Connection a = new Connection(stA, stB, time, getBusByName(i).cost);
                            //Console.WriteLine($"Connect {stA} to {stB}");
                            getBusByName(i).Connections.Add(a);
                            a.busBetwen = getBusByName(i);
                            allConnections.Add(a);
                        }
                        Connection clast = new Connection(last, first,
                                Convert.ToInt32(values[values.Length - 1]), getBusByName(i).cost);
                        clast.busBetwen = getBusByName(i);
                        //Console.WriteLine($"Connect {last} to {first}");
                        allConnections.Add(clast);
                        if (!getBusByName(i).stations.Contains(last)) getBusByName(i).stations.Add(last);
                        getBusByName(i).Connections.Add(clast);
                    }
                }
            }
            catch (Exception e) { ok = false; }
            return ok;
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
                    }
                }
                u = index;
                visited[u] = true;
                for (int j = 0; j < N; j++)
                {
                    if (!visited[j] && w[u, j] != infinite && D[u] != infinite && (D[u] + w[u, j] < D[j]))
                    {
                        D[j] = D[u] + w[u, j];
                        ways[j] = u;
                    }
                }
            }
        }
    }        
}
