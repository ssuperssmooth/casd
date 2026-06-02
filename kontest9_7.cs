using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp40
{
    internal class Program
    {
        class Node
        {
            public int[] Next;
            public int[] Go;
            public int Link;
            public int Parent;
            public int ParentChar;
            public bool Term;
            public List<int> Ids;

            public Node()
            {
                Next = new int[26];
                Go = new int[26];
                for (int i = 0; i < 26; i++)
                {
                    Next[i] = -1;
                    Go[i] = -1;
                }
                Link = -1;
                Parent = -1;
                ParentChar = -1;
                Term = false;
                Ids = new List<int>();
            }
        }

        static List<Node> trie = new List<Node>();

        static int GetLink(int v)
        {
            if (trie[v].Link == -1)
            {
                if (v == 0 || trie[v].Parent == 0)
                {
                    trie[v].Link = 0;
                }
                else
                {
                    trie[v].Link = Go(GetLink(trie[v].Parent), trie[v].ParentChar);
                }
            }

            return trie[v].Link;
        }

        static int Go(int v, int c)
        {
            if (trie[v].Go[c] == -1)
            {
                if (trie[v].Next[c] != -1)
                {
                    trie[v].Go[c] = trie[v].Next[c];
                }
                else
                {
                    if (v == 0)
                    {
                        trie[v].Go[c] = 0;
                    }
                    else
                    {
                        trie[v].Go[c] = Go(GetLink(v), c);
                    }
                }
            }

            return trie[v].Go[c];
        }

        static void AddString(string s, int id)
        {
            int v = 0;

            for (int i = 0; i < s.Length; i++)
            {
                int c = s[i] - 'a';

                if (trie[v].Next[c] == -1)
                {
                    Node node = new Node();
                    node.Parent = v;
                    node.ParentChar = c;
                    trie[v].Next[c] = trie.Count;
                    trie.Add(node);
                }

                v = trie[v].Next[c];
            }

            trie[v].Term = true;
            trie[v].Ids.Add(id);
        }

        static void Main(string[] args)
        {
            int n = int.Parse(Console.ReadLine());
            string[] patterns = new string[n];

            trie.Add(new Node());

            for (int i = 0; i < n; i++)
            {
                patterns[i] = Console.ReadLine();
                AddString(patterns[i], i);
            }

            string text = Console.ReadLine();
            bool[] found = new bool[n];
            int[] count = new int[trie.Count];

            int v = 0;

            for (int i = 0; i < text.Length; i++)
            {
                int c = text[i] - 'a';
                v = Go(v, c);
                count[v]++;
            }

            List<int>[] tree = new List<int>[trie.Count];
            for (int i = 0; i < trie.Count; i++)
            {
                tree[i] = new List<int>();
            }

            for (int i = 1; i < trie.Count; i++)
            {
                int p = GetLink(i);
                tree[p].Add(i);
            }

            int[] order = new int[trie.Count];
            int size = 0;
            Stack<int> stack = new Stack<int>();
            stack.Push(0);

            while (stack.Count > 0)
            {
                int cur = stack.Pop();
                order[size] = cur;
                size++;

                for (int i = 0; i < tree[cur].Count; i++)
                {
                    stack.Push(tree[cur][i]);
                }
            }

            for (int i = size - 1; i > 0; i--)
            {
                int cur = order[i];
                int p = GetLink(cur);
                count[p] += count[cur];
            }

            for (int i = 0; i < trie.Count; i++)
            {
                if (trie[i].Term && count[i] > 0)
                {
                    for (int j = 0; j < trie[i].Ids.Count; j++)
                    {
                        found[trie[i].Ids[j]] = true;
                    }
                }
            }

            for (int i = 0; i < n; i++)
            {
                if (found[i])
                {
                    Console.WriteLine("YES");
                }
                else
                {
                    Console.WriteLine("NO");
                }
            }
        }
    }
}