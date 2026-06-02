using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace ConsoleApp40
{
    internal class Program
    {
        class Edge
        {
            public int to;
            public int rev;
            public int cap;
            public long cost;
            public int id;

            public Edge(int to, int rev, int cap, long cost, int id)
            {
                this.to = to;
                this.rev = rev;
                this.cap = cap;
                this.cost = cost;
                this.id = id;
            }
        }

        class PathEdge
        {
            public int to;
            public int id;
            public bool used;

            public PathEdge(int to, int id)
            {
                this.to = to;
                this.id = id;
                used = false;
            }
        }

        static List<Edge>[] g;
        static List<PathEdge>[] usedGraph;
        static int n;
        static int finish;

        static void AddEdge(int from, int to, int cap, long cost, int id)
        {
            Edge a = new Edge(to, g[to].Count, cap, cost, id);
            Edge b = new Edge(from, g[from].Count, 0, -cost, 0);

            g[from].Add(a);
            g[to].Add(b);
        }

        static bool FindPath(int v, List<int> path)
        {
            if (v == finish)
            {
                return true;
            }

            for (int i = 0; i < usedGraph[v].Count; i++)
            {
                if (!usedGraph[v][i].used)
                {
                    usedGraph[v][i].used = true;
                    path.Add(usedGraph[v][i].id);

                    if (FindPath(usedGraph[v][i].to, path))
                    {
                        return true;
                    }

                    path.RemoveAt(path.Count - 1);
                    usedGraph[v][i].used = false;
                }
            }

            return false;
        }

        static void Main(string[] args)
        {
            string[] first = Console.ReadLine().Split();

            n = int.Parse(first[0]);
            int m = int.Parse(first[1]);
            int k = int.Parse(first[2]);

            g = new List<Edge>[n];

            for (int i = 0; i < n; i++)
            {
                g[i] = new List<Edge>();
            }

            for (int i = 1; i <= m; i++)
            {
                string[] s = Console.ReadLine().Split();

                int a = int.Parse(s[0]) - 1;
                int b = int.Parse(s[1]) - 1;
                long c = long.Parse(s[2]);

                AddEdge(a, b, 1, c, i);
                AddEdge(b, a, 1, c, i);
            }

            int start = 0;
            finish = n - 1;

            long totalCost = 0;
            int flow = 0;
            long INF = 1000000000000000000L;

            while (flow < k)
            {
                long[] dist = new long[n];
                bool[] inQueue = new bool[n];
                int[] prevV = new int[n];
                int[] prevE = new int[n];

                for (int i = 0; i < n; i++)
                {
                    dist[i] = INF;
                    prevV[i] = -1;
                    prevE[i] = -1;
                }

                Queue<int> q = new Queue<int>();

                dist[start] = 0;
                q.Enqueue(start);
                inQueue[start] = true;

                while (q.Count > 0)
                {
                    int v = q.Dequeue();
                    inQueue[v] = false;

                    for (int i = 0; i < g[v].Count; i++)
                    {
                        Edge e = g[v][i];

                        if (e.cap > 0 && dist[v] + e.cost < dist[e.to])
                        {
                            dist[e.to] = dist[v] + e.cost;
                            prevV[e.to] = v;
                            prevE[e.to] = i;

                            if (!inQueue[e.to])
                            {
                                q.Enqueue(e.to);
                                inQueue[e.to] = true;
                            }
                        }
                    }
                }

                if (dist[finish] == INF)
                {
                    break;
                }

                int cur = finish;

                while (cur != start)
                {
                    int v = prevV[cur];
                    int id = prevE[cur];

                    Edge e = g[v][id];

                    e.cap--;
                    g[cur][e.rev].cap++;

                    totalCost += e.cost;

                    cur = v;
                }

                flow++;
            }

            if (flow < k)
            {
                Console.WriteLine("-1");
                return;
            }

            double answer = (double)totalCost / k;
            Console.WriteLine(answer.ToString("F5", CultureInfo.InvariantCulture));

            usedGraph = new List<PathEdge>[n];

            for (int i = 0; i < n; i++)
            {
                usedGraph[i] = new List<PathEdge>();
            }

            for (int v = 0; v < n; v++)
            {
                for (int i = 0; i < g[v].Count; i++)
                {
                    Edge e = g[v][i];

                    if (e.id > 0)
                    {
                        Edge back = g[e.to][e.rev];

                        if (back.cap > 0)
                        {
                            usedGraph[v].Add(new PathEdge(e.to, e.id));
                        }
                    }
                }
            }

            for (int i = 0; i < k; i++)
            {
                List<int> path = new List<int>();

                FindPath(start, path);

                Console.Write(path.Count);

                for (int j = 0; j < path.Count; j++)
                {
                    Console.Write(" " + path[j]);
                }

                Console.WriteLine();
            }
        }
    }
}