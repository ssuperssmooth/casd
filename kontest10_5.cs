using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp40
{
    internal class Program
    {
        class Horizontal
        {
            public long Y;
            public long X1;
            public long X2;
        }

        class Vertical
        {
            public long X;
            public long Y1;
            public long Y2;
        }

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
            int n = int.Parse(Console.ReadLine());

            List<Horizontal> horizontals = new List<Horizontal>();
            List<Vertical> verticals = new List<Vertical>();

            for (int i = 0; i < n; i++)
            {
                string[] parts = Console.ReadLine().Split();
                long x1 = long.Parse(parts[0]);
                long y1 = long.Parse(parts[1]);
                long x2 = long.Parse(parts[2]);
                long y2 = long.Parse(parts[3]);

                if (y1 == y2)
                {
                    Horizontal h = new Horizontal();
                    h.Y = y1;
                    h.X1 = Math.Min(x1, x2);
                    h.X2 = Math.Max(x1, x2);
                    horizontals.Add(h);
                }
                else
                {
                    Vertical v = new Vertical();
                    v.X = x1;
                    v.Y1 = Math.Min(y1, y2);
                    v.Y2 = Math.Max(y1, y2);
                    verticals.Add(v);
                }
            }

            int hCount = horizontals.Count;
            int vCount = verticals.Count;

            graph = new List<int>[hCount + 1];
            for (int i = 1; i <= hCount; i++)
            {
                graph[i] = new List<int>();
            }

            for (int i = 0; i < hCount; i++)
            {
                for (int j = 0; j < vCount; j++)
                {
                    if (verticals[j].X >= horizontals[i].X1 && verticals[j].X <= horizontals[i].X2 &&
                        horizontals[i].Y >= verticals[j].Y1 && horizontals[i].Y <= verticals[j].Y2)
                    {
                        graph[i + 1].Add(j + 1);
                    }
                }
            }

            match = new int[vCount + 1];
            int matching = 0;

            for (int i = 1; i <= hCount; i++)
            {
                used = new bool[hCount + 1];

                if (Dfs(i))
                {
                    matching++;
                }
            }

            Console.WriteLine(n - matching);
        }
    }
}