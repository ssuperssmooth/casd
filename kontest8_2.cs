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
            public int Weight;

            public Edge(int to, int weight)
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
                int w = int.Parse(parts[2]);

                graph[u].Add(new Edge(v, w));
                graph[v].Add(new Edge(u, w));
            }

            long[] dist = new long[n + 1];
            for (int i = 1; i <= n; i++)
            {
                dist[i] = long.MaxValue;
            }

            dist[1] = 0;

            SortedSet<Node> set = new SortedSet<Node>();
            set.Add(new Node(0, 1));

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
                    int weight = graph[v][i].Weight;

                    if (dist[v] + weight < dist[to])
                    {
                        dist[to] = dist[v] + weight;
                        set.Add(new Node(dist[to], to));
                    }
                }
            }

            for (int i = 1; i <= n; i++)
            {
                Console.Write(dist[i]);

                if (i < n)
                {
                    Console.Write(" ");
                }
            }
        }
    }
}