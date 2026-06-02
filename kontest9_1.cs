using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp40
{
    internal class Program
    {
        static ulong GetHash(ulong[] h, ulong[] p, int l, int r)
        {
            return h[r] - h[l - 1] * p[r - l + 1];
        }

        static void Main(string[] args)
        {
            string s = Console.ReadLine();
            int n = s.Length;
            int m = int.Parse(Console.ReadLine());

            ulong[] h = new ulong[n + 1];
            ulong[] p = new ulong[n + 1];
            ulong x = 911382323;

            p[0] = 1;

            for (int i = 1; i <= n; i++)
            {
                p[i] = p[i - 1] * x;
                h[i] = h[i - 1] * x + (ulong)s[i - 1];
            }

            StringBuilder ans = new StringBuilder();

            for (int i = 0; i < m; i++)
            {
                string[] parts = Console.ReadLine().Split();
                int a = int.Parse(parts[0]);
                int b = int.Parse(parts[1]);
                int c = int.Parse(parts[2]);
                int d = int.Parse(parts[3]);

                if (b - a != d - c)
                {
                    ans.AppendLine("No");
                }
                else
                {
                    ulong hash1 = GetHash(h, p, a, b);
                    ulong hash2 = GetHash(h, p, c, d);

                    if (hash1 == hash2)
                    {
                        ans.AppendLine("Yes");
                    }
                    else
                    {
                        ans.AppendLine("No");
                    }
                }
            }

            Console.Write(ans.ToString());
        }
    }
}