using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp40
{
    internal class Program
    {
        static void Dfs(int v, List<int>[] graph, bool[] used, List<int> order)
        {
            used[v] = true;

            for (int i = 0; i < graph[v].Count; i++)
            {
                int to = graph[v][i];

                if (!used[to])
                {
                    Dfs(to, graph, used, order);
                }
            }

            order.Add(v);
        }

        static void Main(string[] args)
        {
            string[] first = Console.ReadLine().Split();
            int n = int.Parse(first[0]);
            int m = int.Parse(first[1]);
            int s = int.Parse(first[2]);

            List<int>[] graph = new List<int>[n + 1];
            for (int i = 1; i <= n; i++)
            {
                graph[i] = new List<int>();
            }

            for (int i = 0; i < m; i++)
            {
                string[] parts = Console.ReadLine().Split();
                int u = int.Parse(parts[0]);
                int v = int.Parse(parts[1]);
                graph[u].Add(v);
            }

            bool[] used = new bool[n + 1];
            List<int> order = new List<int>();

            for (int i = 1; i <= n; i++)
            {
                if (!used[i])
                {
                    Dfs(i, graph, used, order);
                }
            }

            bool[] win = new bool[n + 1];

            for (int i = 0; i < order.Count; i++)
            {
                int v = order[i];

                win[v] = false;

                for (int j = 0; j < graph[v].Count; j++)
                {
                    int to = graph[v][j];

                    if (!win[to])
                    {
                        win[v] = true;
                        break;
                    }
                }
            }

            if (win[s])
            {
                Console.WriteLine("First player wins");
            }
            else
            {
                Console.WriteLine("Second player wins");
            }
        }
    }
}