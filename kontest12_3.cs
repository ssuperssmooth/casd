using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp40
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string[] a = Console.ReadLine().Split();
            string[] b = Console.ReadLine().Split();

            long r1 = long.Parse(a[0]);
            long s1 = long.Parse(a[1]);
            long p1 = long.Parse(a[2]);

            long r2 = long.Parse(b[0]);
            long s2 = long.Parse(b[1]);
            long p2 = long.Parse(b[2]);

            long answer = 0;

            long x1 = r1 - r2 - p2;
            if (x1 > 0)
            {
                answer += x1;
            }

            long x2 = s1 - s2 - r2;
            if (x2 > 0)
            {
                answer += x2;
            }

            long x3 = p1 - p2 - s2;
            if (x3 > 0)
            {
                answer += x3;
            }

            Console.WriteLine(answer);
        }
    }
}