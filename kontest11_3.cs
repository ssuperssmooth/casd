using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp40
{
    internal class Program
    {
        class Edge
        {
            public int To;
            public int Rev;
            public int Cap;
            public int Flow;
            public bool Original;

            public Edge(int to, int rev, int cap, int flow, bool original)
            {
                To = to;
                Rev = rev;
                Cap = cap;
                Flow = flow;
                Original = original;
            }
        }

        static List<Edge>[] graph;
        static int[] level;
        static int[] ptr;

        static void AddEdge(int from, int to)
        {
            Edge forward = new Edge(to, graph[to].Count, 1, 0, true);
            Edge backward = new Edge(from, graph[from].Count, 0, 0, false);

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

        static int Dfs(int v, int t, int pushed)
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
                    int canPush = Math.Min(pushed, e.Cap - e.Flow);
                    int tr = Dfs(e.To, t, canPush);

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

        static int Dinic(int s, int t)
        {
            int flow = 0;

            while (Bfs(s, t))
            {
                for (int i = 0; i < ptr.Length; i++)
                {
                    ptr[i] = 0;
                }

                while (true)
                {
                    int pushed = Dfs(s, t, 1000000000);

                    if (pushed == 0)
                    {
                        break;
                    }

                    flow += pushed;
                }
            }

            return flow;
        }

        static List<int> FindPath(int s, int t, int n)
        {
            int[] parentVertex = new int[n];
            int[] parentEdge = new int[n];

            for (int i = 0; i < n; i++)
            {
                parentVertex[i] = -1;
                parentEdge[i] = -1;
            }

            Queue<int> q = new Queue<int>();
            q.Enqueue(s);
            parentVertex[s] = s;

            while (q.Count > 0)
            {
                int v = q.Dequeue();

                for (int i = 0; i < graph[v].Count; i++)
                {
                    Edge e = graph[v][i];

                    if (e.Original && e.Flow > 0 && parentVertex[e.To] == -1)
                    {
                        parentVertex[e.To] = v;
                        parentEdge[e.To] = i;
                        q.Enqueue(e.To);
                    }
                }
            }

            if (parentVertex[t] == -1) 
            {
                return null;
            }

        List<int> path = new List<int>();
        int cur = t;

            while (cur != s)
            {
                path.Add(cur);
                int pv = parentVertex[cur];
        int pe = parentEdge[cur];

        graph[pv][pe].Flow -= 1;
                cur = pv;
            }

    path.Add(s);
            path.Reverse();

            return path;
        }

static void Main(string[] args)
{
    string[] first = Console.ReadLine().Split();
    int n = int.Parse(first[0]);
    int m = int.Parse(first[1]);
    int s = int.Parse(first[2]) - 1;
    int t = int.Parse(first[3]) - 1;

    graph = new List<Edge>[n];
    for (int i = 0; i < n; i++)
    {
        graph[i] = new List<Edge>();
    }

    level = new int[n];
    ptr = new int[n];

    for (int i = 0; i < m; i++)
    {
        string[] parts = Console.ReadLine().Split();
        int x = int.Parse(parts[0]) - 1;
        int y = int.Parse(parts[1]) - 1;

        AddEdge(x, y);
    }

    int maxFlow = Dinic(s, t);

    if (maxFlow < 2)
    {
        Console.WriteLine("NO");
        return;
    }

    List<int> path1 = FindPath(s, t, n);
    List<int> path2 = FindPath(s, t, n);

    Console.WriteLine("YES");

    for (int i = 0; i < path1.Count; i++)
    {
        if (i > 0)
        {
            Console.Write(" ");
        }
        Console.Write(path1[i] + 1);
    }
    Console.WriteLine();

    for (int i = 0; i < path2.Count; i++)
    {
        if (i > 0)
        {
            Console.Write(" ");
        }
        Console.Write(path2[i] + 1);
    }
    Console.WriteLine();
}
    }
}