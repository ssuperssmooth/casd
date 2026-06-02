using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp40
{
    internal class Program
    {
        static List<int>[] graph;
        static int[] match;
        static bool[] used;
        static int n, m;

        static bool Dfs(int v)
        {
            if (used[v])
            {
                return false;
            }

            used[v] = true;

            for (int i = 0; i < graph[v].Count; i++)
            {
                int to = graph[v][i];

                if (match[to] == 0 || Dfs(match[to]))
                {
                    match[to] = v;
                    return true;
                }
            }

            return false;
        }

        static void Main(string[] args)
        {
            string[] first = Console.ReadLine().Split();
            n = int.Parse(first[0]);
            m = int.Parse(first[1]);

            graph = new List<int>[n + 1];
            for (int i = 1; i <= n; i++)
            {
                graph[i] = new List<int>();
            }

            for (int i = 1; i <= n; i++)
            {
                string[] parts = Console.ReadLine().Split();

                for (int j = 0; j < parts.Length; j++)
                {
                    int x = int.Parse(parts[j]);

                    if (x == 0)
                    {
                        break;
                    }

                    graph[i].Add(x);
                }
            }

            match = new int[m + 1];

            for (int i = 1; i <= n; i++)
            {
                used = new bool[n + 1];
                Dfs(i);
            }

            List<string> answer = new List<string>();

            for (int i = 1; i <= m; i++)
            {
                if (match[i] != 0)
                {
                    answer.Add(match[i] + " " + i);
                }
            }

            Console.WriteLine(answer.Count);

            for (int i = 0; i < answer.Count; i++)
            {
                Console.WriteLine(answer[i]);
            }
        }
    }
}