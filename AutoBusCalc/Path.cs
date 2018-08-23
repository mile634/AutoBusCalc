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
        public Path()
        {
            Length = 0;
            PathCount++;
            data = new Point[50];
            buses = new List<Bus>();
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
