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
                Ids = new List<int>();
            }
        }

        static List<Node> trie = new List<Node>();

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

        static int AddString(string s)
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

            return v;
        }

        static void Main(string[] args)
        {
            int n = int.Parse(Console.ReadLine());

            trie.Add(new Node());

            int[] endVertex = new int[n];

            for (int i = 0; i < n; i++)
            {
                string s = Console.ReadLine();
                int v = AddString(s);
                endVertex[i] = v;
                trie[v].Ids.Add(i);
            }

            string text = Console.ReadLine();

            long[] count = new long[trie.Count];
            int vNow = 0;

            for (int i = 0; i < text.Length; i++)
            {
                int c = text[i] - 'a';
                vNow = Go(vNow, c);
                count[vNow]++;
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
                int v = stack.Pop();
                order[size] = v;
                size++;

                for (int i = 0; i < tree[v].Count; i++)
                {
                    stack.Push(tree[v][i]);
                }
            }

            for (int i = size - 1; i > 0; i--)
            {
                int v = order[i];
                int p = GetLink(v);
                count[p] += count[v];
            }
            for(int i = 0; i < n; i++)
            {
                Console.WriteLine(count[endVertex[i]]);
            }
        }
    }
}