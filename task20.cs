using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zadacha20_CS
{
    internal class Program
    {
        class KruskalEdge
        {
            public int U;
            public int V;
            public long W;

            public KruskalEdge(int u, int v, long w)
            {
                U = u;
                V = v;
                W = w;
            }
        }

        class FlowEdge
        {
            public int To;
            public int Rev;
            public long Capacity;
            public long Flow;

            public FlowEdge(int to, int rev, int capacity)
            {
                To = to;
                Rev = rev;
                Capacity = capacity;
                Flow = 0;
            }
        }

        class OriginalFlowEdge
        {
            public int From;
            public int To;
            public FlowEdge Edge;

            public OriginalFlowEdge(int from, int to, FlowEdge edge)
            {
                From = from;
                To = to;
                Edge = edge;
            }
        }

        static int[] parent;
        static int[] rank;

        static List<FlowEdge>[] flowGraph;
        static long[] excess;
        static int[] height;
        static int[] used;
        static int flowN;
        static int source;
        static int sink;

        static bool[,] cliqueGraph;
        static List<int> bestClique;
        static int cliqueN;

        static int Find(int v)
        {
            if (parent[v] == v)
            {
                return v;
            }

            parent[v] = Find(parent[v]);
            return parent[v];
        }

        static bool Union(int a, int b)
        {
            a = Find(a);
            b = Find(b);

            if (a == b)
            {
                return false;
            }

            if (rank[a] < rank[b])
            {
                int temp = a;
                a = b;
                b = temp;
            }

            parent[b] = a;

            if (rank[a] == rank[b])
            {
                rank[a]++;
            }

            return true;
        }

        static int CompareKruskalEdges(KruskalEdge a, KruskalEdge b)
        {
            if (a.W < b.W)
            {
                return -1;
            }

            if (a.W > b.W)
            {
                return 1;
            }

            return 0;
        }

        static void SolveKruskalExample(int n, List<KruskalEdge> edges)
        {
            Console.WriteLine("Вершин: " + n);
            Console.WriteLine("Рёбра:");

            for (int i = 0; i < edges.Count; i++)
            {
                Console.WriteLine(edges[i].U + " " + edges[i].V + " " + edges[i].W);
            }

            edges.Sort(CompareKruskalEdges);

            parent = new int[n + 1];
            rank = new int[n + 1];

            for (int i = 1; i <= n; i++)
            {
                parent[i] = i;
                rank[i] = 0;
            }

            List<KruskalEdge> answer = new List<KruskalEdge>();
            long totalWeight = 0;

            for (int i = 0; i < edges.Count; i++)
            {
                KruskalEdge edge = edges[i];

                if (Union(edge.U, edge.V))
                {
                    answer.Add(edge);
                    totalWeight += edge.W;
                }
            }

            if (answer.Count != n - 1)
            {
                Console.WriteLine("Ответ: остовного дерева нет");
            }
            else
            {
                Console.WriteLine("Вес минимального остовного дерева: " + totalWeight);
                Console.WriteLine("Рёбра минимального остовного дерева:");

                for (int i = 0; i < answer.Count; i++)
                {
                    Console.WriteLine(answer[i].U + " " + answer[i].V + " " + answer[i].W);
                }
            }

            Console.WriteLine();
        }

        static void RunKruskalExamples()
        {
            Console.WriteLine("Задача 9. Алгоритм Крускала");
            Console.WriteLine();

            Console.WriteLine("Пример 1");
            List<KruskalEdge> edges1 = new List<KruskalEdge>();
            edges1.Add(new KruskalEdge(1, 2, 1));
            edges1.Add(new KruskalEdge(1, 3, 4));
            edges1.Add(new KruskalEdge(2, 3, 2));
            edges1.Add(new KruskalEdge(2, 4, 5));
            edges1.Add(new KruskalEdge(3, 4, 3));
            SolveKruskalExample(4, edges1);

            Console.WriteLine("Пример 2");
            List<KruskalEdge> edges2 = new List<KruskalEdge>();
            edges2.Add(new KruskalEdge(1, 2, 10));
            edges2.Add(new KruskalEdge(1, 3, 6));
            edges2.Add(new KruskalEdge(1, 4, 5));
            edges2.Add(new KruskalEdge(2, 4, 15));
            edges2.Add(new KruskalEdge(3, 4, 4));
            SolveKruskalExample(4, edges2);

            Console.WriteLine("Пример 3");
            List<KruskalEdge> edges3 = new List<KruskalEdge>();
            edges3.Add(new KruskalEdge(1, 2, 1));
            edges3.Add(new KruskalEdge(2, 3, 2));
            edges3.Add(new KruskalEdge(4, 5, 3));
            SolveKruskalExample(5, edges3);
        }

        static FlowEdge AddFlowEdge(int from, int to, int capacity)
        {
            FlowEdge a = new FlowEdge(to, flowGraph[to].Count, capacity);
            FlowEdge b = new FlowEdge(from, flowGraph[from].Count, 0);

            flowGraph[from].Add(a);
            flowGraph[to].Add(b);

            return a;
        }

        static long Residual(FlowEdge edge)
        {
            return edge.Capacity - edge.Flow;
        }

        static void Push(long v, FlowEdge edge)
        {
            long value = Math.Min(excess[v], Residual(edge));

            edge.Flow += value;
            flowGraph[edge.To][edge.Rev].Flow -= value;

            excess[v] -= value;
            excess[edge.To] += value;
        }

        static bool Relabel(int v)
        {
            int minHeight = int.MaxValue;

            for (int i = 0; i < flowGraph[v].Count; i++)
            {
                FlowEdge edge = flowGraph[v][i];

                if (Residual(edge) > 0 && height[edge.To] < minHeight)
                {
                    minHeight = height[edge.To];
                }
            }

            if (minHeight == int.MaxValue)
            {
                return false;
            }

            height[v] = minHeight + 1;
            return true;
        }

        static void Discharge(int v)
        {
            while (excess[v] > 0)
            {
                if (used[v] == flowGraph[v].Count)
                {
                    bool ok = Relabel(v);

                    if (!ok)
                    {
                        return;
                    }

                    used[v] = 0;
                }
                else
                {
                    FlowEdge edge = flowGraph[v][used[v]];

                    if (Residual(edge) > 0 && height[v] == height[edge.To] + 1)
                    {
                        Push(v, edge);
                    }
                    else
                    {
                        used[v]++;
                    }
                }
            }
        }

        static long MaxFlow()
        {
            height[source] = flowN;

            for (int i = 0; i < flowGraph[source].Count; i++)
            {
                FlowEdge edge = flowGraph[source][i];

                if (edge.Capacity > 0)
                {
                    long value = edge.Capacity;

                    edge.Flow += value;
                    flowGraph[edge.To][edge.Rev].Flow -= value;

                    excess[edge.To] += value;
                    excess[source] -= value;
                }
            }

            while (true)
            {
                int v = -1;

                for (int i = 1; i <= flowN; i++)
                {
                    if (i != source && i != sink && excess[i] > 0)
                    {
                        v = i;
                        break;
                    }
                }

                if (v == -1)
                {
                    break;
                }

                Discharge(v);
            }

            return excess[sink];
        }

        static void SolveFlowExample(int n, int s, int t, int[,] edges)
        {
            flowN = n;
            source = s;
            sink = t;

            flowGraph = new List<FlowEdge>[flowN + 1];

            for (int i = 1; i <= flowN; i++)
            {
                flowGraph[i] = new List<FlowEdge>();
            }

            List<OriginalFlowEdge> originalEdges = new List<OriginalFlowEdge>();

            Console.WriteLine("Вершин: " + n);
            Console.WriteLine("Источник: " + s);
            Console.WriteLine("Сток: " + t);
            Console.WriteLine("Рёбра:");

            for (int i = 0; i < edges.GetLength(0); i++)
            {
                int from = edges[i, 0];
                int to = edges[i, 1];
                int capacity = edges[i, 2];

                Console.WriteLine(from + " " + to + " " + capacity);

                FlowEdge edge = AddFlowEdge(from, to, capacity);
                originalEdges.Add(new OriginalFlowEdge(from, to, edge));
            }

            excess = new long[flowN + 1];
            height = new int[flowN + 1];
            used = new int[flowN + 1];

            long answer = MaxFlow();

            Console.WriteLine("Максимальный поток: " + answer);
            Console.WriteLine("Поток по исходным рёбрам:");

            for (int i = 0; i < originalEdges.Count; i++)
            {
                OriginalFlowEdge edge = originalEdges[i];
                Console.WriteLine(edge.From + " " + edge.To + " " + edge.Edge.Flow);
            }

            Console.WriteLine();
        }

        static void RunFlowExamples()
        {
            Console.WriteLine("Задача 12. Алгоритм проталкивания предпотока");
            Console.WriteLine();

            Console.WriteLine("Пример 1");
            int[,] edges1 = new int[,]
            {
                { 1, 2, 10 },
                { 1, 3, 5 },
                { 2, 3, 15 },
                { 2, 4, 10 },
                { 3, 4, 10 }
            };
            SolveFlowExample(4, 1, 4, edges1);

            Console.WriteLine("Пример 2");
            int[,] edges2 = new int[,]
            {
                { 1, 2, 16 },
                { 1, 3, 13 },
                { 2, 3, 10 },
                { 3, 2, 4 },
                { 2, 4, 12 },
                { 4, 3, 9 },
                { 3, 5, 14 },
                { 5, 4, 7 },
                { 4, 6, 20 },
                { 5, 6, 4 }
            };
            SolveFlowExample(6, 1, 6, edges2);

            Console.WriteLine("Пример 3");
            int[,] edges3 = new int[,]
            {
                { 1, 2, 8 },
                { 1, 3, 7 },
                { 2, 4, 5 },
                { 3, 4, 3 },
                { 2, 3, 2 },
                { 4, 5, 10 }
            };
            SolveFlowExample(5, 1, 5, edges3);
        }

        static List<int> IntersectWithNeighbors(List<int> list, int v)
        {
            List<int> result = new List<int>();

            for (int i = 0; i < list.Count; i++)
            {
                int vertex = list[i];

                if (cliqueGraph[v, vertex])
                {
                    result.Add(vertex);
                }
            }

            return result;
        }

        static void BronKerbosch(List<int> r, List<int> p, List<int> x)
        {
            if (p.Count == 0 && x.Count == 0)
            {
                if (r.Count > bestClique.Count)
                {
                    bestClique = new List<int>(r);
                }

                return;
            }

            if (r.Count + p.Count <= bestClique.Count)
            {
                return;
            }

            List<int> copyP = new List<int>(p);

            for (int i = 0; i < copyP.Count; i++)
            {
                int v = copyP[i];

                List<int> newR = new List<int>(r);
                newR.Add(v);

                List<int> newP = IntersectWithNeighbors(p, v);
                List<int> newX = IntersectWithNeighbors(x, v);

                BronKerbosch(newR, newP, newX);

                p.Remove(v);
                x.Add(v);

                if (r.Count + p.Count <= bestClique.Count)
                {
                    return;
                }
            }
        }

        static void SolveCliqueExample(int n, int[,] edges)
        {
            cliqueN = n;
            cliqueGraph = new bool[cliqueN + 1, cliqueN + 1];

            Console.WriteLine("Вершин: " + n);
            Console.WriteLine("Рёбра:");

            for (int i = 0; i < edges.GetLength(0); i++)
            {
                int u = edges[i, 0];
                int v = edges[i, 1];

                Console.WriteLine(u + " " + v);

                cliqueGraph[u, v] = true;
                cliqueGraph[v, u] = true;
            }

            List<int> r = new List<int>();
            List<int> p = new List<int>();
            List<int> x = new List<int>();

            for (int i = 1; i <= cliqueN; i++)
            {
                p.Add(i);
            }

            bestClique = new List<int>();

            BronKerbosch(r, p, x);

            Console.WriteLine("Размер максимальной клики: " + bestClique.Count);
            Console.WriteLine("Вершины максимальной клики:");

            string answer = "";

            for (int i = 0; i < bestClique.Count; i++)
            {
                if (i > 0)
                {
                    answer += " ";
                }

                answer += bestClique[i];
            }

            Console.WriteLine(answer);
            Console.WriteLine();
        }

        static void RunCliqueExamples()
        {
            Console.WriteLine("Задача 15. Алгоритм Брона-Кербоша");
            Console.WriteLine();

            Console.WriteLine("Пример 1");
            int[,] edges1 = new int[,]
            {
                { 1, 2 },
                { 1, 3 },
                { 2, 3 },
                { 3, 4 },
                { 4, 5 }
            };
            SolveCliqueExample(5, edges1);

            Console.WriteLine("Пример 2");
            int[,] edges2 = new int[,]
            {
                { 1, 2 },
                { 1, 3 },
                { 1, 4 },
                { 2, 3 },
                { 2, 4 },
                { 3, 4 },
                { 4, 5 },
                { 5, 6 }
            };
            SolveCliqueExample(6, edges2);

            Console.WriteLine("Пример 3");
            int[,] edges3 = new int[,]
            {
                { 1, 2 },
                { 2, 3 },
                { 3, 4 },
                { 4, 5 },
                { 5, 6 }
            };
            SolveCliqueExample(6, edges3);
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Выберите задачу:");
            Console.WriteLine("9 - Минимальное остовное дерево. Алгоритм Крускала");
            Console.WriteLine("12 - Максимальный поток. Алгоритм проталкивания предпотока");
            Console.WriteLine("15 - Максимальная клика. Алгоритм Брона-Кербоша");

            string choice = Console.ReadLine();

            Console.WriteLine();

            if (choice == "9")
            {
                RunKruskalExamples();
            }
            else if (choice == "12")
            {
                RunFlowExamples();
            }
            else if (choice == "15")
            {
                RunCliqueExamples();
            }
            else
            {
                Console.WriteLine("Такой задачи нет");
            }
        }
    }
}