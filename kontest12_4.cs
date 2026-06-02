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
            string[] first = Console.ReadLine().Split();

            int n = int.Parse(first[0]);
            int m = int.Parse(first[1]);

            long INF = 1000000000000000000L;

            string[] sub = Console.ReadLine().Split();

            long[] a = new long[n + 1];

            for (int i = 1; i <= n; i++)
            {
                a[i] = long.Parse(sub[i - 1]);
            }

            long[,] d = new long[n + 1, n + 1];

            for (int i = 1; i <= n; i++)
            {
                for (int j = 1; j <= n; j++)
                {
                    if (i == j)
                    {
                        d[i, j] = 0;
                    }
                    else
                    {
                        d[i, j] = INF;
                    }
                }
            }

            int[] from = new int[m];
            int[] to = new int[m];
            long[] price = new long[m];

            for (int i = 0; i < m; i++)
            {
                string[] s = Console.ReadLine().Split();

                int u = int.Parse(s[0]);
                int v = int.Parse(s[1]);
                long c = long.Parse(s[2]);

                from[i] = u;
                to[i] = v;
                price[i] = c;

                if (c < d[u, v])
                {
                    d[u, v] = c;
                }
            }

            for (int k = 1; k <= n; k++)
            {
                for (int i = 1; i <= n; i++)
                {
                    for (int j = 1; j <= n; j++)
                    {
                        if (d[i, k] < INF && d[k, j] < INF)
                        {
                            long x = d[i, k] + d[k, j];

                            if (x < d[i, j])
                            {
                                d[i, j] = x;
                            }
                        }
                    }
                }
            }

            long[] oneCity = new long[n + 1];

            for (int i = 1; i <= n; i++)
            {
                oneCity[i] = a[i];
            }

            for (int i = 0; i < m; i++)
            {
                int u = from[i];
                int v = to[i];

                if (d[v, u] < INF)
                {
                    long x = price[i] + d[v, u];

                    if (x < oneCity[u])
                    {
                        oneCity[u] = x;
                    }
                }
            }

            long[,] cst = new long[n + 1, n + 1];

            for (int i = 1; i <= n; i++)
            {
                for (int j = 1; j <= n; j++)
                {
                    if (i == j)
                    {
                        cst[i, j] = oneCity[i];
                    }
                    else
                    {
                        cst[i, j] = d[i, j];
                    }
                }
            }

            long[] uPot = new long[n + 1];
            long[] vPot = new long[n + 1];

            int[] p = new int[n + 1];
            int[] way = new int[n + 1];

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
                            long cur = cst[i0, j] - uPot[i0] - vPot[j];

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
                            uPot[p[j]] += delta;
                            vPot[j] -= delta;
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

            int[] ans = new int[n + 1];

            for (int j = 1; j <= n; j++)
            {
                ans[p[j]] = j;
            }

            long answer = 0;

            for (int i = 1; i <= n; i++)
            {
                answer += cst[i, ans[i]];
            }

            Console.WriteLine(answer);
        }
    }
}