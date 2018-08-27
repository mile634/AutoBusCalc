using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoBusCalc
{
    public class Path
    {
        public static int PathCount = -1;
        public int Length;
        int buf1, buf2, buf3;
        internal Point[] data;
        internal List<Bus> buses;
        //internal List<Connection> connections;
        public Path()
        {
            Length = 0;
            PathCount++;
            data = new Point[50];
            buses = new List<Bus>();
            //connections = new List<Connection>();
        }
        public void print()
        {
            for (int i = 0; i < Length; i++)
                Console.Write($"{data[i].Number} ");
        }
        public void printRev()
        {
            for (int i = Length - 1; i >= 0; i--) Console.Write($"{data[i].Number + 1} ");
            Console.WriteLine();
        }
        /*public bool MultiPath()
        {
            bool rezult = false;
            for (int i = Length-1; i >=1; i--)
            {
                int count = getConCount(data[i].Number+1,data[i-1].Number+1);

                if (count > 0) { Console.WriteLine($"Multi connection founded {data[i].Number + 1} and {data[i].Number - 1}: {count} connections"); return true; }
            }
            return rezult;
        }
        public void fillConns()
        {
            for (int i = Length-1; i >=1; i--)
            {
                //int count = getConCount(data[i].Number + 1, data[i - 1].Number + 1);

                connections.Add(GetConnection(data[i].Number, data[i - 1].Number,1));
            }
        }*/
        Connection GetConnection(int stA, int stB, int n)
        {
            Connection rezult = null;
            int counter = 0;
            foreach(Connection a in Program.allConnections)
            {
                if (a.StationA == stA && a.StationB == stB) counter++;
                if (counter == n) rezult = a;
            }
            return rezult;
        }
        int getConCount(int stA, int stB)
        {
            int rezult = 0;
            foreach (Connection con in Program.allConnections)
            {
                if (con.StationA == stA && con.StationB == stB) rezult++;
            }
            return rezult;
        }
        internal Point this[int index]
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
        internal void setArr(Point[] data) => this.data = data;

        public void DeleteConnection(int i)
        {
            buf1 = data[1 + i].Number; buf2 = data[0 + i].Number;
            buf3 = (int)Program.w[buf1, buf2];
            Program.w[buf1, buf2] = Double.PositiveInfinity;
        }
        public void RestoreConnection() => Program.w[buf1, buf2] = buf3;
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
