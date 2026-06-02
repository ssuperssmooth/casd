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
            public long Weight;

            public Edge(int from, int to, long weight)
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
            int s = int.Parse(first[2]);

            List<Edge> edges = new List<Edge>();
            List<int>[] graph = new List<int>[n + 1];

            for (int i = 1; i <= n; i++)
            {
                graph[i] = new List<int>();
            }

            for (int i = 0; i < m; i++)
            {
                string[] parts = Console.ReadLine().Split();
                int from = int.Parse(parts[0]);
                int to = int.Parse(parts[1]);
                long weight = long.Parse(parts[2]);

                edges.Add(new Edge(from, to, weight));
                graph[from].Add(to);
            }

            long inf = long.MaxValue / 4;
            long[] dist = new long[n + 1];

            for (int i = 1; i <= n; i++)
            {
                dist[i] = inf;
            }

            dist[s] = 0;

            for (int i = 1; i <= n - 1; i++)
            {
                bool changed = false;

                for (int j = 0; j < edges.Count; j++)
                {
                    int from = edges[j].From;
                    int to = edges[j].To;
                    long weight = edges[j].Weight;

                    if (dist[from] != inf && dist[to] > dist[from] + weight)
                    {
                        dist[to] = dist[from] + weight;
                        changed = true;
                    }
                }

                if (!changed)
                {
                    break;
                }
            }

            bool[] bad = new bool[n + 1];
            Queue<int> q = new Queue<int>();

            for (int j = 0; j < edges.Count; j++)
            {
                int from = edges[j].From;
                int to = edges[j].To;
                long weight = edges[j].Weight;

                if (dist[from] != inf && dist[to] > dist[from] + weight)
                {
                    if (!bad[to])
                    {
                        bad[to] = true;
                        q.Enqueue(to);
                    }
                }
            }

            while (q.Count > 0)
            {
                int v = q.Dequeue();

                for (int i = 0; i < graph[v].Count; i++)
                {
                    int to = graph[v][i];

                    if (!bad[to])
                    {
                        bad[to] = true;
                        q.Enqueue(to);
                    }
                }
            }

            for (int i = 1; i <= n; i++)
            {
                if (dist[i] == inf)
                {
                    Console.WriteLine("*");
                }
                else if (bad[i])
                {
                    Console.WriteLine("-");
                }
                else
                {
                    Console.WriteLine(dist[i]);
                }
            }
        }
    }
}