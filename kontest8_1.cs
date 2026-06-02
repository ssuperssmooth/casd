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
            int n = int.Parse(Console.ReadLine());

            int[,] a = new int[n, n];

            for (int i = 0; i < n; i++)
            {
                string[] s = Console.ReadLine().Split();
                for (int j = 0; j < n; j++)
                {
                    a[i, j] = int.Parse(s[j]);
                }
            }

            for (int k = 0; k < n; k++)
            {
                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        if (a[i, j] > a[i, k] + a[k, j])
                        {
                            a[i, j] = a[i, k] + a[k, j];
                        }
                    }
                }
            }

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    Console.Write(a[i, j]);
                    if (j < n - 1)
                    {
                        Console.Write(" ");
                    }
                }
                Console.WriteLine();
            }
        }
    }
}