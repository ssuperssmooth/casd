using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp40
{
    internal class Program
    {
        class Edge
        {
            public int From;
            public int To;
            public int Weight;

            public Edge(int from, int to, int weight)
            {
                From = from;
                To = to;
                Weight = weight;
            }
        }

        static void Main(string[] args)
        {
            string[] first = Console.ReadLine().Split();
            int n = int.Parse(first[0]);
            int m = int.Parse(first[1]);
            int k = int.Parse(first[2]);
            int s = int.Parse(first[3]);

            List<Edge> edges = new List<Edge>();

            for (int i = 0; i < m; i++)
            {
                string[] parts = Console.ReadLine().Split();
                int a = int.Parse(parts[0]);
                int b = int.Parse(parts[1]);
                int w = int.Parse(parts[2]);

                edges.Add(new Edge(a, b, w));
            }

            long inf = long.MaxValue / 4;

            long[] dp = new long[n + 1];
            for (int i = 1; i <= n; i++)
            {
                dp[i] = inf;
            }

            dp[s] = 0;

            for (int step = 1; step <= k; step++)
            {
                long[] next = new long[n + 1];
                for (int i = 1; i <= n; i++)
                {
                    next[i] = inf;
                }

                for (int i = 0; i < edges.Count; i++)
                {
                    int from = edges[i].From;
                    int to = edges[i].To;
                    int weight = edges[i].Weight;

                    if (dp[from] != inf)
                    {
                        long newValue = dp[from] + weight;

                        if (newValue < next[to])
                        {
                            next[to] = newValue;
                        }
                    }
                }

                dp = next;
            }

            for (int i = 1; i <= n; i++)
            {
                if (dp[i] == inf)
                {
                    Console.WriteLine(-1);
                }
                else
                {
                    Console.WriteLine(dp[i]);
                }
            }
        }
    }
}