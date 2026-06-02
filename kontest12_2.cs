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
            int n = int.Parse(Console.ReadLine());

            long[,] c = new long[n + 1, n + 1];

            for (int i = 1; i <= n; i++)
            {
                string[] s = Console.ReadLine().Split();

                for (int j = 1; j <= n; j++)
                {
                    c[i, j] = long.Parse(s[j - 1]);
                }
            }

            long[] u = new long[n + 1];
            long[] v = new long[n + 1];
            int[] p = new int[n + 1];
            int[] way = new int[n + 1];

            long INF = long.MaxValue / 4;

            for (int i = 1; i <= n; i++)
            {
                p[0] = i;

                int j0 = 0;

                long[] minv = new long[n + 1];
                bool[] used = new bool[n + 1];

                for (int j = 0; j <= n; j++)
                {
                    minv[j] = INF;
                }

                while (true)
                {
                    used[j0] = true;

                    int i0 = p[j0];
                    long delta = INF;
                    int j1 = 0;

                    for (int j = 1; j <= n; j++)
                    {
                        if (!used[j])
                        {
                            long cur = c[i0, j] - u[i0] - v[j];

                            if (cur < minv[j])
                            {
                                minv[j] = cur;
                                way[j] = j0;
                            }

                            if (minv[j] < delta)
                            {
                                delta = minv[j];
                                j1 = j;
                            }
                        }
                    }

                    for (int j = 0; j <= n; j++)
                    {
                        if (used[j])
                        {
                            u[p[j]] += delta;
                            v[j] -= delta;
                        }
                        else
                        {
                            minv[j] -= delta;
                        }
                    }

                    j0 = j1;

                    if (p[j0] == 0)
                    {
                        break;
                    }
                }

                while (true)
                {
                    int j1 = way[j0];
                    p[j0] = p[j1];
                    j0 = j1;

                    if (j0 == 0)
                    {
                        break;
                    }
                }
            }

            int[] answer = new int[n + 1];

            for (int j = 1; j <= n; j++)
            {
                answer[p[j]] = j;
            }

            long sum = 0;

            for (int i = 1; i <= n; i++)
            {
                sum += c[i, answer[i]];
            }

            Console.WriteLine(sum);

            for (int i = 1; i <= n; i++)
            {
                Console.WriteLine(i + " " + answer[i]);
            }
        }
    }
}