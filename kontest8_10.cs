using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp40
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string[] first = Console.ReadLine().Split();
            int n = int.Parse(first[0]);
            int m = int.Parse(first[1]);

            List<int>[] graph = new List<int>[n + 1];
            int[] indegree = new int[n + 1];

            for (int i = 1; i <= n; i++)
            {
                graph[i] = new List<int>();
            }

            for (int i = 0; i < m; i++)
            {
                string[] parts = Console.ReadLine().Split();
                int x = int.Parse(parts[0]);
                int y = int.Parse(parts[1]);

                graph[x].Add(y);
                indegree[y]++;
            }

            Queue<int> q = new Queue<int>();
            List<int> topo = new List<int>();

            for (int i = 1; i <= n; i++)
            {
                if (indegree[i] == 0)
                {
                    q.Enqueue(i);
                }
            }

            while (q.Count > 0)
            {
                int v = q.Dequeue();
                topo.Add(v);

                for (int i = 0; i < graph[v].Count; i++)
                {
                    int to = graph[v][i];
                    indegree[to]--;

                    if (indegree[to] == 0)
                    {
                        q.Enqueue(to);
                    }
                }
            }

            int[] grundy = new int[n + 1];
            int[] used = new int[n + 1];
            int timer = 0;

            for (int i = topo.Count - 1; i >= 0; i--)
            {
                int v = topo[i];
                timer++;

                for (int j = 0; j < graph[v].Count; j++)
                {
                    int to = graph[v][j];
                    used[grundy[to]] = timer;
                }

                int mex = 0;
                while (used[mex] == timer)
                {
                    mex++;
                }

                grundy[v] = mex;
            }

            for (int i = 1; i <= n; i++)
            {
                Console.WriteLine(grundy[i]);
            }
        }
    }
}