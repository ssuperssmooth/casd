using System;

namespace ConsoleApp40
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string s = Console.ReadLine();
            int n = s.Length;
            int[] prefixFunction = new int[n];

            for (int i = 1; i < n; i++)
            {
                int j = prefixFunction[i - 1];

                while (j > 0 && s[i] != s[j])
                {
                    j = prefixFunction[j - 1];
                }

                if (s[i] == s[j])
                {
                    j++;
                }

                prefixFunction[i] = j;
            }

            for (int i = 0; i < n; i++)
            {
                Console.Write(prefixFunction[i] + " ");
            }
        }
    }
}