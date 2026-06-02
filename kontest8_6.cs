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
            public int To;
            public long Weight;

            public Edge(int to, long weight)
            {
                To = to;
                Weight = weight;
            }
        }

        class Node : IComparable<Node>
        {
            public long Dist;
            public int Vertex;

            public Node(long dist, int vertex)
            {
                Dist = dist;
                Vertex = vertex;
            }

            public int CompareTo(Node other)
            {
                if (Dist != other.Dist)
                {
                    return Dist.CompareTo(other.Dist);
                }

                return Vertex.CompareTo(other.Vertex);
            }
        }

        static long[] Dijkstra(List<Edge>[] graph, int start, int n)
        {
            long inf = long.MaxValue / 4;
            long[] dist = new long[n + 1];

            for (int i = 1; i <= n; i++)
            {
                dist[i] = inf;
            }

            dist[start] = 0;

            SortedSet<Node> set = new SortedSet<Node>();
            set.Add(new Node(0, start));

            while (set.Count > 0)
            {
                Node current = set.Min;
                set.Remove(current);

                long currentDist = current.Dist;
                int v = current.Vertex;

                if (currentDist != dist[v])
                {
                    continue;
                }

                for (int i = 0; i < graph[v].Count; i++)
                {
                    int to = graph[v][i].To;
                    long weight = graph[v][i].Weight;

                    if (dist[v] + weight < dist[to])
                    {
                        dist[to] = dist[v] + weight;
                        set.Add(new Node(dist[to], to));
                    }
                }
            }

            return dist;
        }

        static void Main(string[] args)
        {
            string[] first = Console.ReadLine().Split();
            int n = int.Parse(first[0]);
            int m = int.Parse(first[1]);

            List<Edge>[] graph = new List<Edge>[n + 1];
            for (int i = 1; i <= n; i++)
            {
                graph[i] = new List<Edge>();
            }

            for (int i = 0; i < m; i++)
            {
                string[] parts = Console.ReadLine().Split();
                int u = int.Parse(parts[0]);
                int v = int.Parse(parts[1]);
                long w = long.Parse(parts[2]);

                graph[u].Add(new Edge(v, w));
                graph[v].Add(new Edge(u, w));
            }

            string[] last = Console.ReadLine().Split();
            int a = int.Parse(last[0]);
            int b = int.Parse(last[1]);
            int c = int.Parse(last[2]);

            long[] distA = Dijkstra(graph, a, n);
            long[] distB = Dijkstra(graph, b, n);
            long[] distC = Dijkstra(graph, c, n);

            long inf = long.MaxValue / 4;

            long ans = inf;

            if (distA[b] != inf && distA[c] != inf)
            {
                ans = Math.Min(ans, distA[b] + distA[c]);
            }

            if (distB[a] != inf && distB[c] != inf)
            {
                ans = Math.Min(ans, distB[a] + distB[c]);
            }

            if (distC[a] != inf && distC[b] != inf)
            {
                ans = Math.Min(ans, distC[a] + distC[b]);
            }

            if (ans == inf)
            {
                Console.WriteLine(-1);
            }
            else
            {
                Console.WriteLine(ans);
            }
        }
    }
}