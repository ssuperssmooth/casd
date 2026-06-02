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
            string s = Console.ReadLine();
            int n = s.Length;

            int maxSize = 2 * n + 5;

            int[,] next = new int[maxSize, 26];
            int[] link = new int[maxSize];
            int[] len = new int[maxSize];

            for (int i = 0; i < maxSize; i++)
            {
                link[i] = -1;
            }

            int size = 1;
            int last = 0;

            for (int i = 0; i < n; i++)
            {
                int c = s[i] - 'a';
                int cur = size;
                size++;

                len[cur] = len[last] + 1;

                int p = last;

                while (p != -1 && next[p, c] == 0)
                {
                    next[p, c] = cur;
                    p = link[p];
                }

                if (p == -1)
                {
                    link[cur] = 0;
                }
                else
                {
                    int q = next[p, c];

                    if (len[p] + 1 == len[q])
                    {
                        link[cur] = q;
                    }
                    else
                    {
                        int clone = size;
                        size++;

                        len[clone] = len[p] + 1;
                        link[clone] = link[q];

                        for (int j = 0; j < 26; j++)
                        {
                            next[clone, j] = next[q, j];
                        }

                        while (p != -1 && next[p, c] == q)
                        {
                            next[p, c] = clone;
                            p = link[p];
                        }

                        link[q] = clone;
                        link[cur] = clone;
                    }
                }

                last = cur;
            }

            long answer = 0;

            for (int v = 1; v < size; v++)
            {
                answer += len[v] - len[link[v]];
            }

            Console.WriteLine(answer);
        }
    }
}