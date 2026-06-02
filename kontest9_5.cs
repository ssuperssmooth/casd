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
            int[] pi = new int[n];

            for (int i = 1; i < n; i++)
            {
                int j = pi[i - 1];

                while (j > 0 && s[i] != s[j])
                {
                    j = pi[j - 1];
                }

                if (s[i] == s[j])
                {
                    j++;
                }

                pi[i] = j;
            }

            int k = n - pi[n - 1];

            if (n % k == 0)
            {
                Console.WriteLine(k);
            }
            else
            {
                Console.WriteLine(n);
            }
        }
    }
}