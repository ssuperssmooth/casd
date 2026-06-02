using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp40
{
    internal class Program
    {
        static int n;
        static int m;
        static int a;
        static int b;
        static char[][] field;
        static int[,] leftId;
        static int[,] rightId;
        static List<int>[] graph;
        static int[] match;
        static bool[] used;

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
            a = int.Parse(first[2]);
            b = int.Parse(first[3]);

            field = new char[n][];
            for (int i = 0; i < n; i++)
            {
                field[i] = Console.ReadLine().ToCharArray();
            }

            int freeCount = 0;

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    if (field[i][j] == '*')
                    {
                        freeCount++;
                    }
                }
            }

            if (a >= 2 * b)
            {
                Console.WriteLine((long)freeCount * b);
                return;
            }

            leftId = new int[n, m];
            rightId = new int[n, m];

            int leftCount = 0;
            int rightCount = 0;

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    if (field[i][j] == '*')
                    {
                        if ((i + j) % 2 == 0)
                        {
                            leftCount++;
                            leftId[i, j] = leftCount;
                        }
                        else
                        {
                            rightCount++;
                            rightId[i, j] = rightCount;
                        }
                    }
                }
            }

            graph = new List<int>[leftCount + 1];
            for (int i = 1; i <= leftCount; i++)
            {
                graph[i] = new List<int>();
            }

            int[] dx = { -1, 1, 0, 0 };
            int[] dy = { 0, 0, -1, 1 };

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    if (field[i][j] == '*' && (i + j) % 2 == 0)
                    {
                        int v = leftId[i, j];

                        for (int k = 0; k < 4; k++)
                        {
                            int ni = i + dx[k];
                            int nj = j + dy[k];

                            if (ni >= 0 && ni < n && nj >= 0 && nj < m)
                            {
                                if (field[ni][nj] == '*')
                                {
                                    int to = rightId[ni, nj];
                                    if (to != 0)
                                    {
                                        graph[v].Add(to);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            match = new int[rightCount + 1];
            int matching = 0;

            for (int i = 1; i <= leftCount; i++)
            {
                used = new bool[leftCount + 1];

                if (Dfs(i))
                {
                    matching++;
                }
            }
            long answer = (long)matching * a + (long)(freeCount - 2 * matching) * b;
            Console.WriteLine(answer);
        }
    }
}