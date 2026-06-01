using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace ConsoleApp40
{
    public class BinaryTreeSearch<K, D> where K : IComparable<K>
    {
        private int Size;
        private Node root;
        private readonly IComparer<K> comparator;

        private class Node
        {
            public D Data;
            public K Key;
            public Node left;
            public Node right;

            public Node(K key, D data)
            {
                Key = key;
                Data = data;
                left = null;
                right = null;
            }
        }

        //1
        public BinaryTreeSearch() : this(Comparer<K>.Default) { }

        //2
        public BinaryTreeSearch(IComparer<K> comparator)
        {
            this.comparator = comparator;
            root = null;
            Size = 0;
        }

        //3
        public void Clear()
        {
            root = null;
            Size = 0;
        }

        //4
        public bool ContainsKey(K key)
        {
            Node current = root;
            while (current != null)
            {
                int comp = comparator.Compare(key, current.Key);
                if (comp < 0) current = current.left;
                else if (comp > 0) current = current.right;
                else return true;
            }
            return false;
        }

        //5
        public bool ContainsValue(D value)
        {
            bool found;
            ContainsValueRec(root, value, out found);
            return found;
        }

        private void ContainsValueRec(Node node, D value, out bool found)
        {
            if (node == null)
            {
                found = false;
                return;
            }

            if (EqualityComparer<D>.Default.Equals(node.Data, value))
            {
                found = true;
                return;
            }

            bool leftFound;
            ContainsValueRec(node.left, value, out leftFound);
            if (leftFound)
            {
                found = true;
                return;
            }

            bool rightFound;
            ContainsValueRec(node.right, value, out rightFound);
            found = rightFound;
        }

        //6
        public List<KeyValuePair<K, D>> EntrySet()
        {
            var list = new List<KeyValuePair<K, D>>();
            AddList(root, list);
            return list;
        }

        private void AddList(Node node, List<KeyValuePair<K, D>> list)
        {
            if (node == null) return;
            AddList(node.left, list);
            list.Add(new KeyValuePair<K, D>(node.Key, node.Data));
            AddList(node.right, list);
        }

        //7
        public D Get(K key)
        {
            return GetValue(root, key);
        }

        private D GetValue(Node node, K key)
        {
            if (node == null) return default;

            int comp = comparator.Compare(key, node.Key);
            if (comp < 0) return GetValue(node.left, key);
            if (comp > 0) return GetValue(node.right, key);
            return node.Data;
        }

        //8
        public bool IsEmpty()
        {
            return Size == 0;
        }

        //9
        public List<K> KeySet()
        {
            List<K> set = new List<K>();
            AddKeySet(root, set);
            return set;
        }

        private void AddKeySet(Node node, List<K> set)
        {
            if (node == null) return;
            AddKeySet(node.left, set);
            set.Add(node.Key);
            AddKeySet(node.right, set);
        }

        //10
        public void Put(K k, D data)
        {
            Node newNode = new Node(k, data);
            if (root == null)
            {
                root = newNode;
                Size++;
                return;
            }

            bool added = AddRec(root, newNode);
            if (added) Size++;
        }

        private bool AddRec(Node current, Node newNode)
        {
            int comp = comparator.Compare(newNode.Key, current.Key);

            if (comp < 0)
            {
                if (current.left == null)
                {
                    current.left = newNode;
                    return true;
                }
                return AddRec(current.left, newNode);
            }
            else if (comp > 0)
            {
                if (current.right == null)
                {
                    current.right = newNode;
                    return true;
                }
                return AddRec(current.right, newNode);
            }
            else
            {

                current.Data = newNode.Data;
                return false;
            }
        }

        //11
        public void Remove(K key)
        {
            bool removed;
            root = RemoveRec(root, key, out removed);
            if (removed) Size--;
        }

        private Node RemoveRec(Node current, K key, out bool removed)
        {
            if (current == null)
            {
                removed = false;
                return null;
            }

            int comp = comparator.Compare(key, current.Key);

            if (comp < 0)
            {
                current.left = RemoveRec(current.left, key, out removed);
                return current;
            }
            else if (comp > 0)
            {
                current.right = RemoveRec(current.right, key, out removed);
                return current;
            }
            else
            {
                removed = true;

                if (current.left == null && current.right == null) return null;


                if (current.left == null) return current.right;
                if (current.right == null) return current.left;


                Node min = FindMin(current.right);
                current.Key = min.Key;
                current.Data = min.Data;

                bool dummy;
                current.right = RemoveRec(current.right, min.Key, out dummy);
                return current;
            }
        }

        private Node FindMin(Node node)
        {
            while (node.left != null) node = node.left;
            return node;
        }

        //12
        public int size()
        {
            return Size;
        }

        //13
        public K firstKey()
        {
            if (root == null) throw new InvalidOperationException("Tree is empty");

            Node current = root;
            while (current.left != null) current = current.left;
            return current.Key;
        }

        //14
        public K lastKey()
        {
            if (root == null) throw new InvalidOperationException("Tree is empty");

            Node current = root;
            while (current.right != null) current = current.right;
            return current.Key;
        }

        //15
        public BinaryTreeSearch<K, D> headMap(K end)
        {

            var newTree = new BinaryTreeSearch<K, D>(comparator);
            AddHeadMap(root, newTree, end);
            return newTree;
        }

        private void AddHeadMap(Node node, BinaryTreeSearch<K, D> tree, K end)
        {
            if (node == null) return;

            int comp = comparator.Compare(node.Key, end);

            if (comp < 0)
            {
                tree.Put(node.Key, node.Data);
                AddHeadMap(node.left, tree, end);
                AddHeadMap(node.right, tree, end);
            }
            else
            {

                AddHeadMap(node.left, tree, end);
            }
        }

        //16
        public BinaryTreeSearch<K, D> SubMap(K start, K end)
        {
            if (start == null || end == null) throw new InvalidOperationException();
            if (comparator.Compare(start, end) > 0) throw new InvalidOperationException();


            var newTree = new BinaryTreeSearch<K, D>(comparator);
            AddSubMap(root, newTree, start, end);
            return newTree;
        }

        private void AddSubMap(Node node, BinaryTreeSearch<K, D> tree, K start, K end)
        {
            if (node == null) return;

            int c1 = comparator.Compare(node.Key, start);
            int c2 = comparator.Compare(node.Key, end);

            if (c1 >= 0 && c2 < 0)
                tree.Put(node.Key, node.Data);

            if (c1 > 0)
                AddSubMap(node.left, tree, start, end);

            if (c2 < 0)
                AddSubMap(node.right, tree, start, end);
        }

        //17
        public BinaryTreeSearch<K, D> tailMap(K start)
        {

            var newTree = new BinaryTreeSearch<K, D>(comparator);
            AddTailMap(root, newTree, start);
            return newTree;
        }

        private void AddTailMap(Node node, BinaryTreeSearch<K, D> tree, K start)
        {
            if (node == null) return;

            int comp = comparator.Compare(node.Key, start);

            if (comp > 0)
            {
                tree.Put(node.Key, node.Data);
                AddTailMap(node.left, tree, start);
                AddTailMap(node.right, tree, start);
            }
            else
            {

                AddTailMap(node.right, tree, start);
            }
        }

        //18
        public KeyValuePair<K, D> lowerEntry(K key)
        {
            Node current = root;
            KeyValuePair<K, D> pair = default;

            while (current != null)
            {
                int comp = comparator.Compare(key, current.Key);

                if (comp > 0)
                {
                    pair = new KeyValuePair<K, D>(current.Key, current.Data);
                    current = current.right;
                }
                else
                {
                    current = current.left;
                }
            }

            return pair;
        }

        //19
        public KeyValuePair<K, D> floorEntry(K key)
        {
            Node current = root;
            KeyValuePair<K, D> pair = default;

            while (current != null)
            {
                int comp = comparator.Compare(key, current.Key);

                if (comp > 0)
                {
                    pair = new KeyValuePair<K, D>(current.Key, current.Data);
                    current = current.right;
                }
                else if (comp == 0)
                {
                    return new KeyValuePair<K, D>(current.Key, current.Data);
                }
                else
                {
                    current = current.left;
                }
            }

            return pair;
        }

        //20
        public KeyValuePair<K, D> higherEntry(K key)
        {
            Node current = root;
            KeyValuePair<K, D> pair = default;

            while (current != null)
            {
                int comp = comparator.Compare(key, current.Key);

                if (comp < 0)
                {
                    pair = new KeyValuePair<K, D>(current.Key, current.Data);
                    current = current.left;
                }
                else
                {
                    current = current.right;
                }
            }

            return pair;
        }

        //21
        public KeyValuePair<K, D> ceilingEntry(K key)
        {
            Node current = root;
            KeyValuePair<K, D> pair = default;

            while (current != null)
            {
                int comp = comparator.Compare(key, current.Key);

                if (comp <= 0)
                {
                    pair = new KeyValuePair<K, D>(current.Key, current.Data);
                    current = current.left;
                }
                else
                {
                    current = current.right;
                }
            }

            return pair;
        }

        //22
        public K lowerKey(K key)
        {
            Node current = root;
            K result = default;

            while (current != null)
            {
                int comp = comparator.Compare(key, current.Key);

                if (comp > 0)
                {
                    result = current.Key;
                    current = current.right;
                }
                else
                {
                    current = current.left;
                }
            }

            return result;
        }

        //23
        public K floorKey(K key)
        {
            Node current = root;
            K result = default;

            while (current != null)
            {
                int comp = comparator.Compare(key, current.Key);

                if (comp > 0)
                {
                    result = current.Key;
                    current = current.right;
                }
                else if (comp == 0)
                {
                    return current.Key;
                }
                else
                {
                    current = current.left;
                }
            }

            return result;
        }

        //24
        public K higherKey(K key)
        {
            Node current = root;
            K result = default;

            while (current != null)
            {
                int comp = comparator.Compare(key, current.Key);

                if (comp < 0)
                {
                    result = current.Key;
                    current = current.left;
                }
                else
                {
                    current = current.right;
                }
            }

            return result;
        }

        //25
        public K ceilingKey(K key)
        {
            Node current = root;
            K result = default;

            while (current != null)
            {
                int comp = comparator.Compare(key, current.Key);

                if (comp <= 0)
                {
                    result = current.Key;
                    current = current.left;
                }
                else
                {
                    current = current.right;
                }
            }

            return result;
        }

        //26
        public KeyValuePair<K, D> pollFirstEntry()
        {
            if (root == null) return default;

            Node parent = null;
            Node current = root;

            while (current.left != null)
            {
                parent = current;
                current = current.left;
            }

            var pair = new KeyValuePair<K, D>(current.Key, current.Data);


            if (parent == null)
                root = current.right;
            else
                parent.left = current.right;

            Size--;
            return pair;
        }

        //27
        public KeyValuePair<K, D> pollLastEntry()
        {
            if (root == null) return default;

            Node parent = null;
            Node current = root;

            while (current.right != null)
            {
                parent = current;
                current = current.right;
            }

            var pair = new KeyValuePair<K, D>(current.Key, current.Data);


            if (parent == null)
                root = current.left;
            else
                parent.right = current.left;

            Size--;
            return pair;
        }

        //28
        public KeyValuePair<K, D> firstEntry()
        {
            if (root == null) return default;

            Node current = root;
            while (current.left != null) current = current.left;
            return new KeyValuePair<K, D>(current.Key, current.Data);
        }

        //29
        public KeyValuePair<K, D> lastEntry()
        {
            if (root == null) return default;

            Node current = root;
            while (current.right != null) current = current.right;
            return new KeyValuePair<K, D>(current.Key, current.Data);
        }
    }
    class MyHashMap<K, V>
    {
        private Entry[] table;
        private int count;
        private float loadfactor;
        public class Entry
        {
            public K Key;
            public V Value;
            public Entry next;


            public Entry(K Key, V Value)
            {
                this.Key = Key;
                this.Value = Value;
                this.next = null;

            }
        }

        public MyHashMap()
        {
            this.table = new Entry[16];
            this.count = 0;
            this.loadfactor = 0.75f;
        }

        public MyHashMap(int initialCapacity)
        {
            this.table = new Entry[initialCapacity];
            this.count = 0;
            this.loadfactor = 0.75f;
        }
        public MyHashMap(int initialCapacity, float loadFactor)
        {
            this.table = new Entry[initialCapacity];
            this.count = 0;
            this.loadfactor = loadFactor;
        }

        private int getIndex(object key)
        {
            if (key == null)
            {
                return 0;
            }
            int hash = key.GetHashCode();
            int index = hash % this.table.Length;
            if (index < 0)
            {
                index = -index;
            }
            return index;
        }

        private bool keyEquals(object key1, object key2)
        {
            if (key1 == null && key2 == null)
            {
                return true;
            }

            if (key1 == null || key2 == null)
            {
                return false;
            }
            return key1.Equals(key2);
        }

        public void Put(K key, V value)
        {
            if (count + 1 > table.Length * loadfactor)
            {
                resize();
            }

            int index = getIndex(key);
            if (table[index] == null)
            {
                table[index] = new Entry(key, value);
                count++;
            }
            else
            {
                Entry current = table[index];
                while (true)
                {
                    if (keyEquals(current.Key, key))
                    {
                        current.Value = value;
                        return;
                    }
                    if (current.next == null)
                    {
                        break;
                    }
                    current = current.next;
                }
                current.next = new Entry(key, value);
                count++;
            }
        }
        private void resize()
        {
            Entry[] oldTable = table;

            table = new Entry[oldTable.Length * 2];
            count = 0;

            for (int i = 0; i < oldTable.Length; i++)
            {
                Entry current = oldTable[i];

                while (current != null)
                {
                    Put(current.Key, current.Value);
                    current = current.next;
                }
            }
        }
        public V Get(K key)
        {
            int index = getIndex(key);
            Entry current = table[index];
            while (current != null)
            {
                if (keyEquals(current.Key, key))
                {
                    return current.Value;
                }
                current = current.next;
            }
            return default(V);
        }

        public bool containsKey(K key)
        {
            int index = getIndex(key);
            Entry current = table[index];
            while (current != null)
            {
                if (keyEquals(current.Key, key))
                {
                    return true;
                }
                current = current.next;
            }
            return false;
        }

        public V Remove(K key)
        {
            int index = getIndex(key);
            Entry current = table[index];
            Entry prev = null;
            while (current != null)
            {
                if (keyEquals(current.Key, key))
                {
                    V value_cur = current.Value;
                    if (table[index] == current)
                    {
                        table[index] = current.next;

                    }
                    else
                    {
                        prev.next = current.next;

                    }
                    count--;
                    return value_cur;
                }
                prev = current;
                current = current.next;
            }
            return default(V);
        }

        public bool containsValue(V value)
        {
            for (int i = 0; i < table.Length; i++)
            {
                Entry current = table[i];
                while (current != null)
                {
                    if (keyEquals(current.Value, value))
                    {
                        return true;
                    }
                    current = current.next;
                }


            }
            return false;
        }

        public K[] keySet()
        {
            K[] keys = new K[count];
            int index = 0;
            for (int i = 0; i < table.Length; i++)
            {
                Entry current = table[i];
                while (current != null)
                {
                    keys[index] = current.Key;
                    current = current.next;
                    index++;
                }
            }
            return keys;
        }
        public Entry[] entrySet()
        {
            Entry[] entries = new Entry[count];
            int index = 0;

            for (int i = 0; i < table.Length; i++)
            {
                Entry current = table[i];

                while (current != null)
                {
                    entries[index] = current;
                    index++;

                    current = current.next;
                }
            }

            return entries;
        }

        public bool isEmpty()
        {
            return count == 0;
        }
        public int size()
        {
            return count;
        }
        public void clear()
        {
            table = new Entry[table.Length];
            count = 0;
        }



    }
    internal class Program
    {
        static int[] MakeKeys(int n)
        {
            int[] keys = new int[n];

            for (int i = 0; i < n; i++)
            {
                keys[i] = i;
            }

            Random random = new Random(1);

            for (int i = 0; i < n; i++)
            {
                int j = random.Next(n);

                int temp = keys[i];
                keys[i] = keys[j];
                keys[j] = temp;
            }

            return keys;
        }
        static void Main(string[] args)
        {
            int[] sizes = { 100000, 1000000, 10000000, 100000000 };
            int runs = 20;

            Console.WriteLine("Operation;Size;MyHashMap average ms;BinaryTreeSearch average ms");

            for (int i = 0; i < sizes.Length; i++)
            {
                int n = sizes[i];

                TestPut(n, runs);
                TestGet(n, runs);
                TestRemove(n, runs);
            }
        }

        static void TestPut(int n, int runs)
        {
            double hashTime = 0;
            double treeTime = 0;

            for (int r = 0; r < runs; r++)
            {
                MyHashMap<int, int> hashMap = new MyHashMap<int, int>();

                Stopwatch sw = Stopwatch.StartNew();

                for (int i = 0; i < n; i++)
                {
                    hashMap.Put(i, i);
                }

                sw.Stop();
                hashTime += sw.Elapsed.TotalMilliseconds;
            }

            for (int r = 0; r < runs; r++)
            {
                BinaryTreeSearch<int, int> treeMap = new BinaryTreeSearch<int, int>();

                Stopwatch sw = Stopwatch.StartNew();

                int[] keys = MakeKeys(n);

                for (int i = 0; i < n; i++)
                {
                    treeMap.Put(keys[i], keys[i]);
                }

                sw.Stop();
                treeTime += sw.Elapsed.TotalMilliseconds;
            }

            Console.WriteLine("Put;" + n + ";" + hashTime / runs + ";" + treeTime / runs);
        }

        static void TestGet(int n, int runs)
        {
            double hashTime = 0;
            double treeTime = 0;

            int[] keys = MakeKeys(n);

            MyHashMap<int, int> hashMap = new MyHashMap<int, int>();
            BinaryTreeSearch<int, int> treeMap = new BinaryTreeSearch<int, int>();

            for (int i = 0; i < n; i++)
            {
                hashMap.Put(keys[i], keys[i]);
                treeMap.Put(keys[i], keys[i]);
            }

            for (int r = 0; r < runs; r++)
            {
                int sum = 0;

                Stopwatch sw = Stopwatch.StartNew();

                for (int i = 0; i < n; i++)
                {
                    sum += hashMap.Get(keys[i]);
                }

                sw.Stop();
                hashTime += sw.Elapsed.TotalMilliseconds;
            }

            for (int r = 0; r < runs; r++)
            {
                int sum = 0;

                Stopwatch sw = Stopwatch.StartNew();

                for (int i = 0; i < n; i++)
                {
                    sum += treeMap.Get(keys[i]);
                }

                sw.Stop();
                treeTime += sw.Elapsed.TotalMilliseconds;
            }

            Console.WriteLine("get;" + n + ";" + hashTime / runs + ";" + treeTime / runs);
        }

        static void TestRemove(int n, int runs)
        {
            double hashTime = 0;
            double treeTime = 0;

            int[] keys = MakeKeys(n);

            for (int r = 0; r < runs; r++)
            {
                MyHashMap<int, int> hashMap = new MyHashMap<int, int>();

                for (int i = 0; i < n; i++)
                {
                    hashMap.Put(keys[i], keys[i]);
                }

                Stopwatch sw = Stopwatch.StartNew();

                for (int i = 0; i < n; i++)
                {
                    hashMap.Remove(keys[i]);
                }

                sw.Stop();
                hashTime += sw.Elapsed.TotalMilliseconds;
            }

            for (int r = 0; r < runs; r++)
            {
                BinaryTreeSearch<int, int> treeMap = new BinaryTreeSearch<int, int>();

                for (int i = 0; i < n; i++)
                {
                    treeMap.Put(keys[i], keys[i]);
                }

                Stopwatch sw = Stopwatch.StartNew();

                for (int i = 0; i < n; i++)
                {
                    treeMap.Remove(keys[i]);
                }

                sw.Stop();
                treeTime += sw.Elapsed.TotalMilliseconds;
            }

            Console.WriteLine("remove;" + n + ";" + hashTime / runs + ";" + treeTime / runs);
        }




    }
}