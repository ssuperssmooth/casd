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

        class GameInfo
        {
            public int TeamA;
            public int TeamB;
            public int Node;

            public GameInfo(int teamA, int teamB, int node)
            {
                TeamA = teamA;
                TeamB = teamB;
                Node = node;
            }
        }

        static List<Edge>[] graph;
        static int[] level;
        static int[] ptr;

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

        static int GetPoints(char c)
        {
            if (c == 'W')
            {
                return 3;
            }
            if (c == 'w')
            {
                return 2;
            }
            if (c == 'l')
            {
                return 1;
            }
            return 0;
        }

        static void SetResult(char[][] table, int a, int b, int pointsA)
        {
            if (pointsA == 3)
            {
                table[a][b] = 'W';
                table[b][a] = 'L';
            }
            else if (pointsA == 2)
            {
                table[a][b] = 'w';
                table[b][a] = 'l';
            }
            else if (pointsA == 1)
            {
                table[a][b] = 'l';
                table[b][a] = 'w';
            }
            else
            {
                table[a][b] = 'L';
                table[b][a] = 'W';
            }
        }

        static void Main(string[] args)
        {
            int n = int.Parse(Console.ReadLine());

            char[][] table = new char[n][];
            for (int i = 0; i < n; i++)
            {
                table[i] = Console.ReadLine().ToCharArray();
            }

            string[] last = Console.ReadLine().Split();
            int[] need = new int[n];
            for (int i = 0; i < n; i++)
            {
                need[i] = int.Parse(last[i]);
            }

            int[] fixedPoints = new int[n];
            List<GameInfo> games = new List<GameInfo>();

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (table[i][j] == 'W' || table[i][j] == 'w' || table[i][j] == 'l' || table[i][j] == 'L')
                    {
                        fixedPoints[i] += GetPoints(table[i][j]);
                    }
                }
            }

            int[] remain = new int[n];
            for (int i = 0; i < n; i++)
            {
                remain[i] = need[i] - fixedPoints[i];
            }

            int gameCount = 0;
            for (int i = 0; i < n; i++)
            {
                for (int j = i + 1; j < n; j++)
                {
                    if (table[i][j] == '.')
                    {
                        gameCount++;
                    }
                }
            }

            int source = 0;
            int firstGameNode = 1;
            int firstTeamNode = firstGameNode + gameCount;
            int sink = firstTeamNode + n;
            int totalNodes = sink + 1;

            graph = new List<Edge>[totalNodes];
            for (int i = 0; i < totalNodes; i++)
            {
                graph[i] = new List<Edge>();
            }

            level = new int[totalNodes];
            ptr = new int[totalNodes];

            int currentGameNode = firstGameNode;
            int totalPointsInGames = 0;

            for (int i = 0; i < n; i++)
            {
                for (int j = i + 1; j < n; j++)
                {
                    if (table[i][j] == '.')
                    {
                        int node = currentGameNode;
                        currentGameNode++;

                        games.Add(new GameInfo(i, j, node));

                        AddEdge(source, node, 3);
                        AddEdge(node, firstTeamNode + i, 3);
                        AddEdge(node, firstTeamNode + j, 3);

                        totalPointsInGames += 3;
                    }
                }
            }

            for (int i = 0; i < n; i++)
            {
                AddEdge(firstTeamNode + i, sink, remain[i]);
            }

            int flow = Dinic(source, sink);

            for (int g = 0; g < games.Count; g++)
            {
                int node = games[g].Node;
                int a = games[g].TeamA;
                int b = games[g].TeamB;

                int pointsA = 0;

                for (int i = 0; i < graph[node].Count; i++)
                {
                    Edge e = graph[node][i];

                    if (e.To == firstTeamNode + a)
                    {
                        pointsA = e.Flow;
                        break;
                    }
                }

                SetResult(table, a, b, pointsA);
            }

            for (int i = 0; i < n; i++)
            {
                Console.WriteLine(new string(table[i]));
            }
        }
    }
}