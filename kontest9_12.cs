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
            int kNeed = int.Parse(Console.ReadLine());

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

            for (int i = n - 1; i >= 0; i--)
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

            int h = 0;
            while ((1 << h) < n)
            {
                for (int i = 0; i < n; i++)
                {
                    pn[i] = p[i] - (1 << h);
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
                    int cur2 = c[(p[i] + (1 << h)) % n];
                    int prev1 = c[p[i - 1]];
                    int prev2 = c[(p[i - 1] + (1 << h)) % n];

                    if (cur1 != prev1 || cur2 != prev2)
                    {
                        classes++;
                    }

                    cn[p[i]] = classes - 1;
                }

                int[] temp = c;
                c = cn;
                cn = temp;

                h++;
            }

            int countDistinct = 0;
            int lastClass = -1;

            for (int i = 0; i < n; i++)
            {
                int start = p[i];
                int curClass = c[start];

                if (curClass != lastClass)
                {
                    countDistinct++;

                    if (countDistinct == kNeed)
                    {
                        StringBuilder ans = new StringBuilder();

                        for (int j = 0; j < n; j++)
                        {
                            ans.Append(s[(start + j) % n]);
                        }

                        Console.WriteLine(ans.ToString());
                        return;
                    }

                    lastClass = curClass;
                }
            }

            Console.WriteLine("IMPOSSIBLE");
        }
    }
}