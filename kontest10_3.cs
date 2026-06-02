using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp40
{
    internal class Program
    {
        class EventData
        {
            public int Time;
            public int X;
            public int Y;
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

                if (match[to] == -1 || Dfs(match[to]))
                {
                    match[to] = v;
                    return true;
                }
            }

            return false;
        }

        static int GetMinutes(string s)
        {
            string[] parts = s.Split(':');
            int h = int.Parse(parts[0]);
            int m = int.Parse(parts[1]);
            return h * 60 + m;
        }

        static void Main(string[] args)
        {
            string[] first = Console.ReadLine().Split();
            int n = int.Parse(first[0]);
            int v = int.Parse(first[1]);

            EventData[] eventsArray = new EventData[n];

            for (int i = 0; i < n; i++)
            {
                string[] parts = Console.ReadLine().Split();

                eventsArray[i] = new EventData();
                eventsArray[i].Time = GetMinutes(parts[0]);
                eventsArray[i].X = int.Parse(parts[1]);
                eventsArray[i].Y = int.Parse(parts[2]);
            }

            Array.Sort(eventsArray, (a, b) => a.Time.CompareTo(b.Time));

            graph = new List<int>[n];
            for (int i = 0; i < n; i++)
            {
                graph[i] = new List<int>();
            }

            for (int i = 0; i < n; i++)
            {
                for (int j = i + 1; j < n; j++)
                {
                    double dx = eventsArray[i].X - eventsArray[j].X;
                    double dy = eventsArray[i].Y - eventsArray[j].Y;
                    double dist = Math.Sqrt(dx * dx + dy * dy);
                    double timeNeed = dist / v * 60.0;
                    int timeDiff = eventsArray[j].Time - eventsArray[i].Time;

                    if (timeNeed <= timeDiff + 1e-9)
                    {
                        graph[i].Add(j);
                    }
                }
            }

            match = new int[n];
            for (int i = 0; i < n; i++)
            {
                match[i] = -1;
            }

            int matching = 0;

            for (int i = 0; i < n; i++)
            {
                used = new bool[n];

                if (Dfs(i))
                {
                    matching++;
                }
            }

            Console.WriteLine(n - matching);
        }
    }
}