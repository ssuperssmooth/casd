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
            int n = int.Parse(Console.ReadLine());
            List<Edge> edges = new List<Edge>();

            for (int i = 1; i <= n; i++)
            {
                string[] parts = Console.ReadLine().Split();

                for (int j = 1; j <= n; j++)
                {
                    int w = int.Parse(parts[j - 1]);

                    if (w != 100000)
                    {
                        edges.Add(new Edge(i, j, w));
                    }
                }
            }

            long[] dist = new long[n + 1];
            int[] parent = new int[n + 1];

            for (int i = 1; i <= n; i++)
            {
                dist[i] = 0;
                parent[i] = -1;
            }

            int x = -1;

            for (int i = 1; i <= n; i++)
            {
                x = -1;

                for (int j = 0; j < edges.Count; j++)
                {
                    int from = edges[j].From;
                    int to = edges[j].To;
                    int weight = edges[j].Weight;

                    if (dist[to] > dist[from] + weight)
                    {
                        dist[to] = dist[from] + weight;
                        parent[to] = from;
                        x = to;
                    }
                }
            }

            if (x == -1)
            {
                Console.WriteLine("NO");
            }
            else
            {
                for (int i = 1; i <= n; i++)
                {
                    x = parent[x];
                }

                List<int> cycle = new List<int>();
                int cur = x;

                while (true)
                {
                    cycle.Add(cur);
                    cur = parent[cur];

                    if (cur == x && cycle.Count > 1)
                    {
                        break;
                    }
                }

                cycle.Reverse();

                Console.WriteLine("YES");
                Console.WriteLine(cycle.Count);

                for (int i = 0; i < cycle.Count; i++)
                {
                    Console.Write(cycle[i]);

                    if (i < cycle.Count - 1)
                    {
                        Console.Write(" ");
                    }
                }
            }
        }
    }
}