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
            public int to;
            public int rev;
            public long cap;
            public long cost;

            public Edge(int to, int rev, long cap, long cost)
            {
                this.to = to;
                this.rev = rev;
                this.cap = cap;
                this.cost = cost;
            }
        }

        static List<Edge>[] g;

        static void AddEdge(int v, int to, long cap, long cost)
        {
            Edge a = new Edge(to, g[to].Count, cap, cost);
            Edge b = new Edge(v, g[v].Count, 0, -cost);

            g[v].Add(a);
            g[to].Add(b);
        }

        static void Main(string[] args)
        {
            string[] first = Console.ReadLine().Split();
            int n = int.Parse(first[0]);
            int m = int.Parse(first[1]);

            g = new List<Edge>[n];

            for (int i = 0; i < n; i++)
            {
                g[i] = new List<Edge>();
            }

            for (int i = 0; i < m; i++)
            {
                string[] s = Console.ReadLine().Split();

                int a = int.Parse(s[0]) - 1;
                int b = int.Parse(s[1]) - 1;
                long cap = long.Parse(s[2]);
                long cost = long.Parse(s[3]);

                AddEdge(a, b, cap, cost);
            }

            int start = 0;
            int finish = n - 1;

            long answer = 0;
            long INF = long.MaxValue / 4;

            long[] p = new long[n];

            while (true)
            {
                long[] dist = new long[n];
                bool[] used = new bool[n];
                int[] prevV = new int[n];
                int[] prevE = new int[n];

                for (int i = 0; i < n; i++)
                {
                    dist[i] = INF;
                    prevV[i] = -1;
                    prevE[i] = -1;
                }

                dist[start] = 0;

                for (int i = 0; i < n; i++)
                {
                    int v = -1;

                    for (int j = 0; j < n; j++)
                    {
                        if (!used[j] && dist[j] < INF)
                        {
                            if (v == -1 || dist[j] < dist[v])
                            {
                                v = j;
                            }
                        }
                    }

                    if (v == -1)
                    {
                        break;
                    }

                    used[v] = true;

                    for (int j = 0; j < g[v].Count; j++)
                    {
                        Edge e = g[v][j];

                        if (e.cap > 0)
                        {
                            long newDist = dist[v] + e.cost + p[v] - p[e.to];

                            if (newDist < dist[e.to])
                            {
                                dist[e.to] = newDist;
                                prevV[e.to] = v;
                                prevE[e.to] = j;
                            }
                        }
                    }
                }

                if (dist[finish] == INF)
                {
                    break;
                }

                for (int i = 0; i < n; i++)
                {
                    if (dist[i] < INF)
                    {
                        p[i] += dist[i];
                    }
                }

                long addFlow = INF;
                int cur = finish;

                while (cur != start)
                {
                    int v = prevV[cur];
                    int id = prevE[cur];

                    if (g[v][id].cap < addFlow)
                    {
                        addFlow = g[v][id].cap;
                    }

                    cur = v;
                }

                cur = finish;

                while (cur != start)
                {
                    int v = prevV[cur];
                    int id = prevE[cur];

                    Edge e = g[v][id];

                    answer += addFlow * e.cost;

                    e.cap -= addFlow;
                    g[cur][e.rev].cap += addFlow;

                    cur = v;
                }
            }

            Console.WriteLine(answer);
        }
    }
}