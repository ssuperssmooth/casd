using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp40
{
    internal class Program
    {
        static long mod1 = 1000000007;
        static long mod2 = 1000000009;
        static long p = 911382323;

        static bool EqualSubstrings(long[] h1, long[] h2, long[] pow1, long[] pow2, int l1, int r1, int l2, int r2)
        {
            long hash11 = (h1[r1 + 1] - (h1[l1] * pow1[r1 - l1 + 1]) % mod1 + mod1) % mod1;
            long hash12 = (h1[r2 + 1] - (h1[l2] * pow1[r2 - l2 + 1]) % mod1 + mod1) % mod1;

            if (hash11 != hash12)
            {
                return false;
            }

            long hash21 = (h2[r1 + 1] - (h2[l1] * pow2[r1 - l1 + 1]) % mod2 + mod2) % mod2;
            long hash22 = (h2[r2 + 1] - (h2[l2] * pow2[r2 - l2 + 1]) % mod2 + mod2) % mod2;

            return hash21 == hash22;
        }

        static int CompareSubstrings(string t, long[] h1, long[] h2, long[] pow1, long[] pow2, int a, int b, int len)
        {
            int left = 0;
            int right = len;

            while (left < right)
            {
                int mid = (left + right + 1) / 2;

                if (EqualSubstrings(h1, h2, pow1, pow2, a, a + mid - 1, b, b + mid - 1))
                {
                    left = mid;
                }
                else
                {
                    right = mid - 1;
                }
            }

            int lcp = left;

            if (lcp == len)
            {
                return 0;
            }

            if (t[a + lcp] < t[b + lcp])
            {
                return -1;
            }
            else
            {
                return 1;
            }
        }

        static void Main(string[] args)
        {
            string s = Console.ReadLine();
            string t = Console.ReadLine();

            int n = s.Length;
            int m = t.Length;

            int maxStates = 2 * n + 5;

            int[,] next = new int[maxStates, 26];
            int[] link = new int[maxStates];
            int[] len = new int[maxStates];

            for (int i = 0; i < maxStates; i++)
            {
                link[i] = -1;

                for (int j = 0; j < 26; j++)
                {
                    next[i, j] = -1;
                }
            }

            int size = 1;
            int last = 0;

            for (int i = 0; i < n; i++)
            {
                int c = s[i] - 'a';
                int cur = size;
                size++;

                len[cur] = len[last] + 1;

                int pState = last;

                while (pState != -1 && next[pState, c] == -1)
                {
                    next[pState, c] = cur;
                    pState = link[pState];
                }

                if (pState == -1)
                {
                    link[cur] = 0;
                }
                else
                {
                    int q = next[pState, c];

                    if (len[pState] + 1 == len[q])
                    {
                        link[cur] = q;
                    }
                    else
                    {
                        int clone = size;
                        size++;

                        len[clone] = len[pState] + 1;
                        link[clone] = link[q];

                        for (int j = 0; j < 26; j++)
                        {
                            next[clone, j] = next[q, j];
                        }

                        while (pState != -1 && next[pState, c] == q)
                        {
                            next[pState, c] = clone;
                            pState = link[pState];
                        }

                        link[q] = clone;
                        link[cur] = clone;
                    }
                }

                last = cur;
            }

            int[] matchLen = new int[m];
            int v = 0;
            int currentLen = 0;
            int best = 0;

            for (int i = 0; i < m; i++)
            {
                int c = t[i] - 'a';

                if (next[v, c] != -1)
                {
                    v = next[v, c];
                    currentLen++;
                }
                else
                {
                    while (v != -1 && next[v, c] == -1)
                    {
                        v = link[v];
                    }

                    if (v == -1)
                    {
                        v = 0;
                        currentLen = 0;
                    }
                    else
                    {
                        currentLen = len[v] + 1;
                        v = next[v, c];
                    }
                }

                matchLen[i] = currentLen;

                if (currentLen > best)
                {
                    best = currentLen;
                }
            }

            if (best == 0)
            {
                Console.WriteLine("");
                return;
            }

            long[] h1 = new long[m + 1];
            long[] h2 = new long[m + 1];
            long[] pow1 = new long[m + 1];
            long[] pow2 = new long[m + 1];

            pow1[0] = 1;
            pow2[0] = 1;

            for (int i = 0; i < m; i++)
            {
                h1[i + 1] = (h1[i] * p + (t[i] - 'a' + 1)) % mod1;
                h2[i + 1] = (h2[i] * p + (t[i] - 'a' + 1)) % mod2;
                pow1[i + 1] = (pow1[i] * p) % mod1;
                pow2[i + 1] = (pow2[i] * p) % mod2;
            }

            int bestStart = -1;

            for (int i = 0; i < m; i++)
            {
                if (matchLen[i] >= best)
                {
                    int start = i - best + 1;

                    if (bestStart == -1)
                    {
                        bestStart = start;
                    }
                    else
                    {
                        int cmp = CompareSubstrings(t, h1, h2, pow1, pow2, start, bestStart, best);

                        if (cmp < 0)
                        {
                            bestStart = start;
                        }
                    }
                }
            }

            Console.WriteLine(t.Substring(bestStart, best));
        }
    }
}
