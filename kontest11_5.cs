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
            public int Rev;
            public int Cap;
            public int Flow;

            public Edge(int to, int rev, int cap)
            {
                To = to;
                Rev = rev;
                Cap = cap;
                Flow = 0;
            }
        }

        static List<Edge>[] graph;
        static int[] level;
        static int[] ptr;
        static bool[] used;

        static int INF = 1000000000;

        static void AddEdge(int from, int to, int cap)
        {
            Edge forward = new Edge(to, graph[to].Count, cap);
            Edge backward = new Edge(from, graph[from].Count, 0);

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
                    int pushed = Dfs(s, t, INF);

                    if (pushed == 0)
                    {
                        break;
                    }

                    flow += pushed;
                }
            }

            return flow;
        }

        static void DfsResidual(int v)
        {
            used[v] = true;

            for (int i = 0; i < graph[v].Count; i++)
            {
                Edge e = graph[v][i];

                if (!used[e.To] && e.Flow < e.Cap)
                {
                    DfsResidual(e.To);
                }
            }
        }

        static int GetInId(int x, int y, int n)
        {
            return (x * n + y) * 2;
        }

        static int GetOutId(int x, int y, int n)
        {
            return (x * n + y) * 2 + 1;
        }

        static void Main(string[] args)
        {
            string[] first = Console.ReadLine().Split();
            int m = int.Parse(first[0]);
            int n = int.Parse(first[1]);

            char[][] map = new char[m][];
            for (int i = 0; i < m; i++)
            {
                map[i] = Console.ReadLine().ToCharArray();
            }

            int totalNodes = m * n * 2;
            graph = new List<Edge>[totalNodes];
            for (int i = 0; i < totalNodes; i++)
            {
                graph[i] = new List<Edge>();
            }

            level = new int[totalNodes];
            ptr = new int[totalNodes];

            int source = -1;
            int sink = -1;

            int[] dx = { -1, 1, 0, 0 };
            int[] dy = { 0, 0, -1, 1 };

            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (map[i][j] == '#')
                    {
                        continue;
                    }

                    int inId = GetInId(i, j, n);
                    int outId = GetOutId(i, j, n);

                    int cap = INF;

                    if (map[i][j] == '.')
                    {
                        cap = 1;
                    }

                    AddEdge(inId, outId, cap);

                    if (map[i][j] == 'A')
                    {
                        source = outId;
                    }

                    if (map[i][j] == 'B')
                    {
                        sink = inId;
                    }
                }
            }

            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (map[i][j] == '#')
                    {
                        continue;
                    }

                    int outId = GetOutId(i, j, n);

                    for (int k = 0; k < 4; k++)
                    {
                        int ni = i + dx[k];
                        int nj = j + dy[k];

                        if (ni < 0 || ni >= m || nj < 0 || nj >= n)
                        {
                            continue;
                        }

                        if (map[ni][nj] == '#')
                        {
                            continue;
                        }

                        int nextInId = GetInId(ni, nj, n);
                        AddEdge(outId, nextInId, INF);
                    }
                }
            }

            int maxFlow = Dinic(source, sink);

            if (maxFlow >= INF)
            {
                Console.WriteLine(-1);
                return;
            }

            used = new bool[totalNodes];
            DfsResidual(source);

            int answer = 0;

            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (map[i][j] != '.')
                    {
                        continue;
                    }

                    int inId = GetInId(i, j, n);
                    int outId = GetOutId(i, j, n);

                    if (used[inId] && !used[outId])
                    {
                        map[i][j] = '+';
                        answer++;
                    }
                }
            }

            Console.WriteLine(answer);
            for (int i = 0; i < m; i++)
            {
                Console.WriteLine(new string(map[i]));
            }
        }
    }
}
