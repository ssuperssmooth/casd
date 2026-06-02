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
            public int Cost;

            public Edge(int to, int rev, int cap, int cost)
            {
                To = to;
                Rev = rev;
                Cap = cap;
                Cost = cost;
            }
        }

        static void AddEdge(List<Edge>[] graph, int from, int to, int cap, int cost)
        {
            graph[from].Add(new Edge(to, graph[to].Count, cap, cost));
            graph[to].Add(new Edge(from, graph[from].Count - 1, 0, -cost));
        }

        static void Main(string[] args)
        {
            string[] first = Console.ReadLine().Split();
            int m = int.Parse(first[0]);
            int k = int.Parse(first[1]);
            int n = int.Parse(first[2]);

            int t = int.Parse(Console.ReadLine());

            bool[,] bad = new bool[m + 1, k + 1];

            for (int i = 0; i < t; i++)
            {
                string[] parts = Console.ReadLine().Split();
                int green = int.Parse(parts[0]);
                int yellowGlobal = int.Parse(parts[1]);
                int yellow = yellowGlobal - m;

                bad[green, yellow] = true;
            }

            int q = int.Parse(Console.ReadLine());

            bool[] mustGreen = new bool[m + 1];
            bool[] mustYellow = new bool[k + 1];
            int mustCount = 0;

            if (q > 0)
            {
                string[] parts = Console.ReadLine().Split();

                for (int i = 0; i < q; i++)
                {
                    int x = int.Parse(parts[i]);

                    if (x <= m)
                    {
                        mustGreen[x] = true;
                    }
                    else
                    {
                        mustYellow[x - m] = true;
                    }

                    mustCount++;
                }
            }
            else
            {
                Console.ReadLine();
            }

            int mustGreenCount = 0;
            int mustYellowCount = 0;

            for (int i = 1; i <= m; i++)
            {
                if (mustGreen[i])
                {
                    mustGreenCount++;
                }
            }

            for (int i = 1; i <= k; i++)
            {
                if (mustYellow[i])
                {
                    mustYellowCount++;
                }
            }

            if (mustGreenCount > n || mustYellowCount > n)
            {
                Console.WriteLine("NO");
                return;
            }

            int source = 0;
            int greenStart = 1;
            int yellowStart = greenStart + m;
            int sink = yellowStart + k;
            int vertexCount = sink + 1;

            List<Edge>[] graph = new List<Edge>[vertexCount];
            for (int i = 0; i < vertexCount; i++)
            {
                graph[i] = new List<Edge>();
            }

            for (int i = 1; i <= m; i++)
            {
                int cost = 0;
                if (mustGreen[i])
                {
                    cost = -1;
                }

                AddEdge(graph, source, greenStart + i - 1, 1, cost);
            }

            for (int i = 1; i <= m; i++)
            {
                for (int j = 1; j <= k; j++)
                {
                    if (!bad[i, j])
                    {
                        AddEdge(graph, greenStart + i - 1, yellowStart + j - 1, 1, 0);
                    }
                }
            }

            for (int j = 1; j <= k; j++)
            {
                int cost = 0;
                if (mustYellow[j])
                {
                    cost = -1;
                }

                AddEdge(graph, yellowStart + j - 1, sink, 1, cost);
            }

            int flow = 0;
            int costSum = 0; while (flow < n)
            {
                int[] dist = new int[vertexCount];
                int[] prevV = new int[vertexCount];
                int[] prevE = new int[vertexCount];
                bool[] inQueue = new bool[vertexCount];

                for (int i = 0; i < vertexCount; i++)
                {
                    dist[i] = int.MaxValue;
                    prevV[i] = -1;
                    prevE[i] = -1;
                }

                Queue<int> queue = new Queue<int>();
                dist[source] = 0;
                queue.Enqueue(source);
                inQueue[source] = true;

                while (queue.Count > 0)
                {
                    int v = queue.Dequeue();
                    inQueue[v] = false;

                    for (int i = 0; i < graph[v].Count; i++)
                    {
                        Edge e = graph[v][i];

                        if (e.Cap > 0 && dist[v] != int.MaxValue && dist[e.To] > dist[v] + e.Cost)
                        {
                            dist[e.To] = dist[v] + e.Cost;
                            prevV[e.To] = v;
                            prevE[e.To] = i;

                            if (!inQueue[e.To])
                            {
                                inQueue[e.To] = true;
                                queue.Enqueue(e.To);
                            }
                        }
                    }
                }

                if (dist[sink] == int.MaxValue)
                {
                    break;
                }

                int add = 1;
                int cur = sink;

                while (cur != source)
                {
                    int pv = prevV[cur];
                    int pe = prevE[cur];

                    if (graph[pv][pe].Cap < add)
                    {
                        add = graph[pv][pe].Cap;
                    }

                    cur = pv;
                }

                cur = sink;

                while (cur != source)
                {
                    int pv = prevV[cur];
                    int pe = prevE[cur];
                    Edge e = graph[pv][pe];

                    e.Cap -= add;
                    graph[cur][e.Rev].Cap += add;

                    cur = pv;
                }

                flow += add;
                costSum += dist[sink] * add;
            }

            if (flow < n || costSum != -mustCount)
            {
                Console.WriteLine("NO");
                return;
            }

            List<string> answer = new List<string>();

            for (int i = 1; i <= m; i++)
            {
                int v = greenStart + i - 1;

                for (int j = 0; j < graph[v].Count; j++)
                {
                    Edge e = graph[v][j];

                    if (e.To >= yellowStart && e.To < yellowStart + k && e.Cap == 0)
                    {
                        int yellow = e.To - yellowStart + 1;
                        answer.Add(i + " " + (m + yellow));
                    }
                }
            }

            Console.WriteLine("YES");
            for (int i = 0; i < answer.Count; i++)
            {
                Console.WriteLine(answer[i]);
            }
        }
    }
}