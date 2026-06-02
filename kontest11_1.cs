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
            public long Cap;
            public long Flow;
            public int Rev;

            public Edge(int to, long cap, long flow, int rev)
            {
                To = to;
                Cap = cap;
                Flow = flow;
                Rev = rev;
            }
        }

        static List<Edge>[] graph;
        static int[] level;
        static int[] ptr;
        static int n, m;

        static void AddDirectedEdge(int from, int to, long cap)
        {
            Edge forward = new Edge(to, cap, 0, graph[to].Count);
            Edge backward = new Edge(from, 0, 0, graph[from].Count);

            graph[from].Add(forward);
            graph[to].Add(backward);
        }

        static bool Bfs(int s, int t)
        {
            for (int i = 0; i < level.Length; i++)
            {
                level[i] = -1;
            }

            Queue<int> q = new Queue<int>();
            q.Enqueue(s);
            level[s] = 0;

            while (q.Count > 0)
            {
                int v = q.Dequeue();

                for (int i = 0; i < graph[v].Count; i++)
                {
                    Edge e = graph[v][i];

                    if (level[e.To] == -1 && e.Flow < e.Cap)
                    {
                        level[e.To] = level[v] + 1;
                        q.Enqueue(e.To);
                    }
                }
            }

            return level[t] != -1;
        }

        static long Dfs(int v, int t, long pushed)
        {
            if (pushed == 0)
            {
                return 0;
            }

            if (v == t)
            {
                return pushed;
            }

            while (ptr[v] < graph[v].Count)
            {
                int id = ptr[v];
                Edge e = graph[v][id];

                if (level[e.To] == level[v] + 1 && e.Flow < e.Cap)
                {
                    long canPush = Math.Min(pushed, e.Cap - e.Flow);
                    long tr = Dfs(e.To, t, canPush);

                    if (tr > 0)
                    {
                        e.Flow += tr;
                        graph[e.To][e.Rev].Flow -= tr;
                        return tr;
                    }
                }

                ptr[v]++;
            }

            return 0;
        }

        static long Dinic(int s, int t)
        {
            long flow = 0;

            while (Bfs(s, t))
            {
                for (int i = 0; i < ptr.Length; i++)
                {
                    ptr[i] = 0;
                }

                while (true)
                {
                    long pushed = Dfs(s, t, long.MaxValue);

                    if (pushed == 0)
                    {
                        break;
                    }

                    flow += pushed;
                }
            }

            return flow;
        }

        static void Main(string[] args)
        {
            n = int.Parse(Console.ReadLine());
            m = int.Parse(Console.ReadLine());

            graph = new List<Edge>[n];
            for (int i = 0; i < n; i++)
            {
                graph[i] = new List<Edge>();
            }

            level = new int[n];
            ptr = new int[n];

            int[] a = new int[m];
            int[] b = new int[m];
            long[] c = new long[m];

            int[] forwardIndex1 = new int[m];
            int[] forwardIndex2 = new int[m];

            for (int i = 0; i < m; i++)
            {
                string[] parts = Console.ReadLine().Split();
                a[i] = int.Parse(parts[0]) - 1;
                b[i] = int.Parse(parts[1]) - 1;
                c[i] = long.Parse(parts[2]);

                forwardIndex1[i] = graph[a[i]].Count;
                AddDirectedEdge(a[i], b[i], c[i]);

                forwardIndex2[i] = graph[b[i]].Count; 
                AddDirectedEdge(b[i], a[i], c[i]);
            }

            long maxFlow = Dinic(0, n - 1);

            Console.WriteLine(maxFlow);

            for (int i = 0; i < m; i++)
            {
                long flowAB = graph[a[i]][forwardIndex1[i]].Flow;
                long flowBA = graph[b[i]][forwardIndex2[i]].Flow;
                long answer = flowAB - flowBA;

                Console.WriteLine(answer);
            }
        }
    }
}