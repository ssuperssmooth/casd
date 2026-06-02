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
        static int[] matchToGirl;
        static int[] matchToBoy;
        static bool[] used;
        static bool[] visBoy;
        static bool[] visGirl;

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

                if (matchToGirl[to] == 0 || Dfs(matchToGirl[to]))
                {
                    matchToGirl[to] = v;
                    matchToBoy[v] = to;
                    return true;
                }
            }

            return false;
        }

        static void DfsCover(int v)
        {
            if (visBoy[v])
            {
                return;
            }

            visBoy[v] = true;

            for (int i = 0; i < graph[v].Count; i++)
            {
                int to = graph[v][i];

                if (matchToBoy[v] != to && !visGirl[to])
                {
                    visGirl[to] = true;

                    if (matchToGirl[to] != 0)
                    {
                        DfsCover(matchToGirl[to]);
                    }
                }
            }
        }

        static void Main(string[] args)
        {
            int k = int.Parse(Console.ReadLine());
            StringBuilder answer = new StringBuilder();

            for (int test = 0; test < k; test++)
            {
                string[] first = Console.ReadLine().Split();
                int m = int.Parse(first[0]);
                int n = int.Parse(first[1]);

                bool[,] know = new bool[m + 1, n + 1];

                for (int i = 1; i <= m; i++)
                {
                    string[] parts = Console.ReadLine().Split();

                    for (int j = 0; j < parts.Length; j++)
                    {
                        int x = int.Parse(parts[j]);

                        if (x == 0)
                        {
                            break;
                        }

                        know[i, x] = true;
                    }
                }

                graph = new List<int>[m + 1];
                for (int i = 1; i <= m; i++)
                {
                    graph[i] = new List<int>();

                    for (int j = 1; j <= n; j++)
                    {
                        if (!know[i, j])
                        {
                            graph[i].Add(j);
                        }
                    }
                }

                matchToGirl = new int[n + 1];
                matchToBoy = new int[m + 1];

                for (int i = 1; i <= m; i++)
                {
                    used = new bool[m + 1];
                    Dfs(i);
                }

                visBoy = new bool[m + 1];
                visGirl = new bool[n + 1];

                for (int i = 1; i <= m; i++)
                {
                    if (matchToBoy[i] == 0)
                    {
                        DfsCover(i);
                    }
                }

                List<int> boys = new List<int>();
                List<int> girls = new List<int>();

                for (int i = 1; i <= m; i++)
                {
                    if (visBoy[i])
                    {
                        boys.Add(i);
                    }
                }

                for (int i = 1; i <= n; i++)
                {
                    if (!visGirl[i])
                    {
                        girls.Add(i);
                    }
                }

                answer.AppendLine((boys.Count + girls.Count).ToString());
                answer.AppendLine(boys.Count + " " + girls.Count);

                for (int i = 0; i < boys.Count; i++)
                {
                    if (i > 0)
                    {
                        answer.Append(" ");
                    }

                    answer.Append(boys[i]);
                }

                answer.AppendLine();

                for (int i = 0; i < girls.Count; i++)
                {
                    if (i > 0)
                    {
                        answer.Append(" ");
                    }

                    answer.Append(girls[i]);
                }

                answer.AppendLine();

                if (test != k - 1)
                {
                    answer.AppendLine();
                }
            }

            Console.Write(answer.ToString());
        }
    }
}