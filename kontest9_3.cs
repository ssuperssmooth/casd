using System;

namespace ConsoleApp40
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string s = Console.ReadLine();
            int n = s.Length;
            int[] z = new int[n];

            z[0] = 0; 

            int l = 0, r = 0;
            for (int i = 1; i < n; i++)
            {
                if (i <= r)
                {
                    z[i] = Math.Min(r - i + 1, z[i - l]);
                }

                while (i + z[i] < n && s[z[i]] == s[i + z[i]])
                {
                    z[i]++;
                }

                if (i + z[i] - 1 > r)
                {
                    l = i;
                    r = i + z[i] - 1;
                }
            }

            for (int i = 1; i < n; i++)
            {
                Console.Write(z[i] + " ");
            }
        }
    }
}