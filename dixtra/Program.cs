using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace dixtra
{
    public class Program
    {
        static int N;
        static public double[,] w;
        static double[] D;
        static bool[] visited;
        static double infinite = Double.PositiveInfinity;
        static double[] ways;
        static Path[] Paths = new Path[20];
        static int counter = 0;
        static void Main(string[] args)
        {
            //N = Convert.ToInt32(Console.ReadLine());
            Initialize();
            //Console.Write(Paths[3].In());
            Console.ReadKey();
        }

        static void Initialize()
        {
            string path = @"E:/matrica2.txt";
            readFile(path);
            ways = new double[N];
            int start = 7;
            int finish = 15;
            Paths = new Path[N];
            for (int i = 0; i < N; i++) Paths[i] = new Path();
            dixtra(start);
            CalcPath(start, finish);
            //Console.WriteLine(Paths.Length);
            for (int i = 0;i<=Paths[0].Length-2;i++)
            {
                Paths[0].DeleteConnection(i);
                dixtra(start);
                CalcPath(start, finish);
                Paths[0].RestoreConnection();
            }
            Console.WriteLine("Possible paths:");
            for (int i = 0;i<Paths.Length;i++)
            {
                Paths[i].printRev();
            }            
            printGraph();
        }
        
        static void CalcPath(int start, int finish)
        {
            Path tmp = new Path();
            List<Point> tmpp = new List<Point>();
            Point previous = new Point(finish, null);
            fixways(start);
            tmpp.Add(previous);
            while(finish!=start)
            {
                finish = (int)ways[finish];
                previous = new Point(finish, previous);
                tmpp.Add(previous);
                
            }
            tmp.setArr(tmpp.ToArray());
            tmp.Length = tmpp.Count;
            if (!tmp.In()) 
            Paths[counter++] = tmp;
        }               
        
        static void fixways(int start)
        {
            for (int i = 0; i < N; i++)
                    if (ways[i] == 0 && w[start,i] != Double.PositiveInfinity) ways[i] = start;
        }
        

        static void dixtra(int st)
        {
            D = new double[N];
            visited = new bool[N];
            int tmp = 0;
            for (int i = 0; i < N; i++)
            {
                D[i] = w[st,i];
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
                    if (!visited[j] && w[u,j] != infinite && D[u] != infinite && (D[u] + w[u,j] < D[j]))
                    {
                        D[j] = D[u] + w[u,j];
                        ways[j] = u;
                        //Console.WriteLine($"2nd cycle: u={u}, j={j}");
                    }
                }                
            }
        }
        static void printGraph()
        {
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    if (w[i,j] == infinite) Console.Write("{0,2}", '-');
                    else Console.Write("{0,2}", w[i, j]);
                }
                Console.WriteLine();
            }
        }
        static void readFile(string path)
        {
            N = 22;
            w = new double[N, N];
            using (StreamReader fs = new StreamReader(path))
            {
                for (int i = 0; i < N; i++)
                {

                    int[] temparr = new int[N];
                    string tempstr = fs.ReadLine();
                    string[] values = tempstr.Split(' ');
                    
                    for (int j = 0; j < N; j++)
                    {
                        if (Convert.ToDouble(values[j]) == 0) w[i, j] = infinite;
                        else w[i, j] = Convert.ToInt32(values[j]);
                    }
                }
            }
            printGraph();
        }
        class Path
        {
            public static int PathCount = -1;
            public int Length;
            int buf1, buf2, buf3;
            public Point[] data;
            public Path()
            {
                Length = 0;
                PathCount++;
                data = new Point[50];
            }
            public void print()
            {
                for(int i = 0;i<Length;i++)
                {
                    Console.Write($"{data[i].Number} ");
                }
            }
            public void printRev()
            {
                for(int i = Length-1; i>=0; i--) Console.Write($"{data[i].Number+1} ");
                Console.WriteLine();
            }
            public Point this[int index]
            {
                get
                {
                    return data[index];
                }
                set
                {
                    data[index] = value;
                }
            }
            public void setArr(Point[] data)
            {
                this.data = data;
            }
            public Path getPathWithNoLastTwo()
            {
                Path rezult = new Path();
                return rezult;
            }
            public void DeleteConnection(int i)
            {
                
                buf1 = data[1+i].Number; buf2 = data[0+i].Number;
                //Console.WriteLine($"Deleting w[{buf1+1},{buf2+1}]");
                buf3 = (int)w[buf1, buf2];
                w[buf1, buf2] = Double.PositiveInfinity;
            }
            public void RestoreConnection()
            {
                w[buf1, buf2] = buf3;
                //Console.WriteLine($"Restoring w[{buf1 + 1},{buf2 + 1}] = {buf3}");
            }
            public bool equal(Path b)
            {
                int i = 0;
                if (this.Length == b.Length)
                {
                    for (int j = 0; j < this.Length; j++) if (this[j].Number == b[j].Number) i++;
                    if (i == this.Length) return true;
                }
                return false;
            }
            public bool In()
            {
                for (int i = 0; i < Paths.Length; i++) if (Paths[i].equal(this)) return true;
                return false;
            }
        }
        class Point
        {
            public int Number;
            public Point Previous;
            public Point(int num, Point prev)
            {
                Number = num; Previous = prev;
            }
        }
    }

}
