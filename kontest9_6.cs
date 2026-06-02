using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp40
{
    internal class Program
    {
        static ulong[] pow;
        static ulong[][] h;
        static string[] arr;

        static ulong GetHash(int id, int l, int r)
        {
            return h[id][r + 1] - h[id][l] * pow[r - l + 1];
        }

        static bool Check(int len, int baseIndex, out string found)
        {
            found = "";

            if (len == 0)
            {
                return true;
            }

            Dictionary<ulong, int> pos = new Dictionary<ulong, int>();
            HashSet<ulong> current = new HashSet<ulong>();

            for (int i = 0; i + len <= arr[baseIndex].Length; i++)
            {
                ulong value = GetHash(baseIndex, i, i + len - 1);

                if (!pos.ContainsKey(value))
                {
                    pos[value] = i;
                }

                current.Add(value);
            }

            for (int i = 0; i < arr.Length; i++)
            {
                if (i == baseIndex)
                {
                    continue;
                }

                HashSet<ulong> next = new HashSet<ulong>();

                for (int j = 0; j + len <= arr[i].Length; j++)
                {
                    ulong value = GetHash(i, j, j + len - 1);

                    if (current.Contains(value))
                    {
                        next.Add(value);
                    }
                }

                current = next;

                if (current.Count == 0)
                {
                    return false;
                }
            }

            foreach (ulong value in current)
            {
                int start = pos[value];
                string candidate = arr[baseIndex].Substring(start, len);
                bool ok = true;

                for (int i = 0; i < arr.Length; i++)
                {
                    if (!arr[i].Contains(candidate))
                    {
                        ok = false;
                        break;
                    }
                }

                if (ok)
                {
                    found = candidate;
                    return true;
                }
            }

            return false;
        }

        static void Main(string[] args)
        {
            int k = int.Parse(Console.ReadLine());
            arr = new string[k];

            int minLen = int.MaxValue;
            int baseIndex = 0;

            for (int i = 0; i < k; i++)
            {
                arr[i] = Console.ReadLine();

                if (arr[i].Length < minLen)
                {
                    minLen = arr[i].Length;
                    baseIndex = i;
                }
            }

            pow = new ulong[minLen + 1];
            pow[0] = 1;
            ulong p = 911382323;

            for (int i = 1; i <= minLen; i++)
            {
                pow[i] = pow[i - 1] * p;
            }

            h = new ulong[k][];

            for (int i = 0; i < k; i++)
            {
                h[i] = new ulong[arr[i].Length + 1];

                for (int j = 0; j < arr[i].Length; j++)
                {
                    h[i][j + 1] = h[i][j] * p + (ulong)(arr[i][j] - 'a' + 1);
                }
            }

            int left = 0;
            int right = minLen;
            string answer = "";

            while (left <= right)
            {
                int mid = (left + right) / 2;
                string current;

                if (Check(mid, baseIndex, out current))
                {
                    answer = current;
                    left = mid + 1;
                }
                else
                {
                    right = mid - 1;
                }
            }

            Console.WriteLine(answer);
        }
    }
}