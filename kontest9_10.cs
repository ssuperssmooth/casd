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
            string input = Console.ReadLine();
            string s = input + "$";
            int n = s.Length;

            int[] p = new int[n];
            int[] c = new int[n];

            int alphabet = 256;
            int[] cnt = new int[Math.Max(alphabet, n)];

            for (int i = 0; i < n; i++)
            {
                cnt[(int)s[i]]++;
            }

            for (int i = 1; i < alphabet; i++)
            {
                cnt[i] += cnt[i - 1];
            }

            for (int i = 0; i < n; i++)
            {
                int ch = (int)s[i];
                cnt[ch]--;
                p[cnt[ch]] = i;
            }

            c[p[0]] = 0;
            int classes = 1;

            for (int i = 1; i < n; i++)
            {
                if (s[p[i]] != s[p[i - 1]])
                {
                    classes++;
                }

                c[p[i]] = classes - 1;
            }

            int[] pn = new int[n];
            int[] cn = new int[n];

            int k = 0;
            while ((1 << k) < n)
            {
                for (int i = 0; i < n; i++)
                {
                    pn[i] = p[i] - (1 << k);
                    if (pn[i] < 0)
                    {
                        pn[i] += n;
                    }
                }

                for (int i = 0; i < classes; i++)
                {
                    cnt[i] = 0;
                }

                for (int i = 0; i < n; i++)
                {
                    cnt[c[pn[i]]]++;
                }

                for (int i = 1; i < classes; i++)
                {
                    cnt[i] += cnt[i - 1];
                }

                for (int i = n - 1; i >= 0; i--)
                {
                    int x = pn[i];
                    int cl = c[x];
                    cnt[cl]--;
                    p[cnt[cl]] = x;
                }

                cn[p[0]] = 0;
                classes = 1;

                for (int i = 1; i < n; i++)
                {
                    int cur1 = c[p[i]];
                    int cur2 = c[(p[i] + (1 << k)) % n];
                    int prev1 = c[p[i - 1]];
                    int prev2 = c[(p[i - 1] + (1 << k)) % n];

                    if (cur1 != prev1 || cur2 != prev2)
                    {
                        classes++;
                    }

                    cn[p[i]] = classes - 1;
                }

                int[] temp = c;
                c = cn;
                cn = temp;

                k++;
            }

            int[] rank = new int[n];
            for (int i = 0; i < n; i++)
            {
                rank[p[i]] = i;
            }

            int[] lcp = new int[n];
            int value = 0;

            for (int i = 0; i < n - 1; i++)
            {
                int r = rank[i];

                if (r == 0)
                {
                    value = 0;
                    continue;
                }

                int j = p[r - 1];

                while (i + value < n && j + value < n && s[i + value] == s[j + value])
                {
                    value++;
                }

                lcp[r] = value;

                if (value > 0)
                {
                    value--;
                }
            }

            StringBuilder answer = new StringBuilder();

            for (int i = 1; i < n; i++)
            {
                answer.Append(p[i] + 1);
                if (i < n - 1)
                {
                    answer.Append(' ');
                }
            }

            answer.AppendLine();

            for (int i = 2; i < n; i++)
            {
                answer.Append(lcp[i]);
                if (i < n - 1)
                {
                    answer.Append(' ');
                }
            }

            Console.Write(answer.ToString());
        }
    }
}