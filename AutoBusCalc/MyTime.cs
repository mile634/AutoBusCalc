using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoBusCalc
{
    class MyTime : ICloneable
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
            int totalminutes = hour * 60 + minute + mins;
            return new MyTime() { hour = totalminutes / 60, minute = totalminutes % 60 };
        }
        public object Clone()
        {
            return new MyTime { hour = this.hour, minute = this.minute };
        }
        public static MyTime operator -(MyTime c1, MyTime c2)
        {
            int a = c1.hour * 60 + c1.minute;
            int b = c2.hour * 60 + c2.minute;
            return new MyTime { hour = (a - b) / 60, minute = (a - b) % 60 };
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
}
