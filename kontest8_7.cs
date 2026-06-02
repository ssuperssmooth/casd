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

        class Heap
        {
            public long[] Dist;
            public int[] Vertex;
            public int[] Mod;
            public int Count;

            public Heap()
            {
                Dist = new long[4];
                Vertex = new int[4];
                Mod = new int[4];
                Count = 0;
            }

            void Swap(int i, int j)
            {
                long tempDist = Dist[i];
                Dist[i] = Dist[j];
                Dist[j] = tempDist;

                int tempVertex = Vertex[i];
                Vertex[i] = Vertex[j];
                Vertex[j] = tempVertex;

                int tempMod = Mod[i];
                Mod[i] = Mod[j];
                Mod[j] = tempMod;
            }

            void Resize()
            {
                long[] newDist = new long[Dist.Length * 2];
                int[] newVertex = new int[Vertex.Length * 2];
                int[] newMod = new int[Mod.Length * 2];

                for (int i = 0; i < Dist.Length; i++)
                {
                    newDist[i] = Dist[i];
                    newVertex[i] = Vertex[i];
                    newMod[i] = Mod[i];
                }

                Dist = newDist;
                Vertex = newVertex;
                Mod = newMod;
            }

            public void Add(long dist, int vertex, int mod)
            {
                Count++;

                if (Count >= Dist.Length)
                {
                    Resize();
                }

                Dist[Count] = dist;
                Vertex[Count] = vertex;
                Mod[Count] = mod;

                int i = Count;

                while (i > 1)
                {
                    int parent = i / 2;

                    if (Dist[parent] <= Dist[i])
                    {
                        break;
                    }

                    Swap(i, parent);
                    i = parent;
                }
            }

            public void Pop(out long dist, out int vertex, out int mod)
            {
                dist = Dist[1];
                vertex = Vertex[1];
                mod = Mod[1];

                Dist[1] = Dist[Count];
                Vertex[1] = Vertex[Count];
                Mod[1] = Mod[Count];
                Count--;

                int i = 1;

                while (true)
                {
                    int left = i * 2;
                    int right = i * 2 + 1;
                    int smallest = i;

                    if (left <= Count && Dist[left] < Dist[smallest])
                    {
                        smallest = left;
                    }

                    if (right <= Count && Dist[right] < Dist[smallest])
                    {
                        smallest = right;
                    }

                    if (smallest == i)
                    {
                        break;
                    }

                    Swap(i, smallest);
                    i = smallest;
                }
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

            int minEdgeFromN = int.MaxValue;

            for (int i = 0; i < m; i++)
            {
                string[] parts = Console.ReadLine().Split();
                int a = int.Parse(parts[0]);
                int b = int.Parse(parts[1]);
                int d = int.Parse(parts[2]);

                graph[a].Add(new Edge(b, d));
                graph[b].Add(new Edge(a, d));

                if (a == n || b == n)
                {
                    if (d < minEdgeFromN)
                    {
                        minEdgeFromN = d;
                    }
                }
            }

            long t = long.Parse(Console.ReadLine());

            if (minEdgeFromN == int.MaxValue)
            {
                Console.WriteLine("Impossible");
                return;
            }

            int modValue = minEdgeFromN * 2;
            int needMod = (int)(t % modValue);

            long inf = long.MaxValue / 4;
            long[][] dist = new long[n + 1][];

            for (int i = 1; i <= n; i++)
            {
                dist[i] = new long[modValue];

                for (int j = 0; j < modValue; j++)
                {
                    dist[i][j] = inf;
                }
            }

            dist[1][0] = 0;

            Heap heap = new Heap();
            heap.Add(0, 1, 0);

            while (heap.Count > 0)
            {
                long currentDist;
                int v;
                int currentMod;

                heap.Pop(out currentDist, out v, out currentMod);

                if (currentDist != dist[v][currentMod])
                {
                    continue;
                }

                if (v == n && currentMod == needMod)
                {
                    break;
                }

                for (int i = 0; i < graph[v].Count; i++)
                {
                    int to = graph[v][i].To;
                    int weight = graph[v][i].Weight;

                    int newMod = (currentMod + weight) % modValue;
                    long newDist = currentDist + weight;

                    if (newDist < dist[to][newMod])
                    {
                        dist[to][newMod] = newDist;
                        heap.Add(newDist, to, newMod);
                    }
                }
            }

            if (dist[n][needMod] <= t)
            {
                Console.WriteLine("Possible");
            }
            else
            {
                Console.WriteLine("Impossible");
            }
        }
    }
}