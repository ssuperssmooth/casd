using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ConsoleApp40
{
    internal class Program
    {
        class FastScanner
        {
            private readonly Stream stream = Console.OpenStandardInput();
            private readonly byte[] buffer = new byte[1 << 16];
            private int len = 0;
            private int ptr = 0;

            private int Read()
            {
                if (ptr >= len)
                {
                    len = stream.Read(buffer, 0, buffer.Length);
                    ptr = 0;
                    if (len == 0)
                    {
                        return -1;
                    }
                }

                return buffer[ptr++];
            }

            public int NextInt()
            {
                int c = Read();

                while (c <= 32)
                {
                    c = Read();
                }

                int sign = 1;
                if (c == '-')
                {
                    sign = -1;
                    c = Read();
                }

                int value = 0;
                while (c > 32)
                {
                    value = value * 10 + c - '0';
                    c = Read();
                }

                return value * sign;
            }
        }

        static void Main(string[] args)
        {
            FastScanner fs = new FastScanner();
            int t = fs.NextInt();
            List<string> answer = new List<string>();

            for (int test = 0; test < t; test++)
            {
                int n = fs.NextInt();
                int m = fs.NextInt();

                List<int>[] rev = new List<int>[n + 1];
                int[] outDeg = new int[n + 1];

                for (int i = 1; i <= n; i++)
                {
                    rev[i] = new List<int>();
                }

                for (int i = 0; i < m; i++)
                {
                    int a = fs.NextInt();
                    int b = fs.NextInt();

                    rev[b].Add(a);
                    outDeg[a]++;
                }

                int[] state = new int[n + 1];
                int[] cnt = new int[n + 1];
                Queue<int> q = new Queue<int>();

                for (int i = 1; i <= n; i++)
                {
                    cnt[i] = outDeg[i];

                    if (outDeg[i] == 0)
                    {
                        state[i] = 2;
                        q.Enqueue(i);
                    }
                }

                while (q.Count > 0)
                {
                    int v = q.Dequeue();

                    for (int i = 0; i < rev[v].Count; i++)
                    {
                        int u = rev[v][i];

                        if (state[u] != 0)
                        {
                            continue;
                        }

                        if (state[v] == 2)
                        {
                            state[u] = 1;
                            q.Enqueue(u);
                        }
                        else
                        {
                            cnt[u]--;

                            if (cnt[u] == 0)
                            {
                                state[u] = 2;
                                q.Enqueue(u);
                            }
                        }
                    }
                }

                for (int i = 1; i <= n; i++)
                {
                    if (state[i] == 1)
                    {
                        answer.Add("FIRST");
                    }
                    else if (state[i] == 2)
                    {
                        answer.Add("SECOND");
                    }
                    else
                    {
                        answer.Add("DRAW");
                    }
                }

                if (test != t - 1)
                {
                    answer.Add("");
                }
            }

            Console.Write(string.Join("\n", answer));
        }
    }
}