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
            string p = Console.ReadLine();
            string t = Console.ReadLine();

            string s = p + "#" + t;
            int[] pi = new int[s.Length];
            List<int> ans = new List<int>();

            for (int i = 1; i < s.Length; i++)
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

                if (pi[i] == p.Length)
                {
                    ans.Add(i - 2 * p.Length + 1);
                }
            }

            Console.WriteLine(ans.Count);

            for (int i = 0; i < ans.Count; i++)
            {
                Console.Write(ans[i]);

                if (i < ans.Count - 1)
                {
                    Console.Write(" ");
                }
            }
        }
    }
}