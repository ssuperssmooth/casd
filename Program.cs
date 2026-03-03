using System;
using System.Collections.Generic;

public delegate int TreeMapComparator<in K>(K a, K b);

public sealed class MyTreeMap<K, V>
{
    public sealed class Entry : IEquatable<Entry>
    {
        public K Key { get; }
        public V Value { get; internal set; }

        public Entry(K key, V value)
        {
            Key = key;
            Value = value;
        }

        public bool Equals(Entry other)
        {
            if (ReferenceEquals(other, null)) return false;
            return EqualityComparer<K>.Default.Equals(Key, other.Key) &&
                   EqualityComparer<V>.Default.Equals(Value, other.Value);
        }

        public override bool Equals(object obj) => obj is Entry e && Equals(e);

        public override int GetHashCode()
        {
            int h1 = Key == null ? 0 : EqualityComparer<K>.Default.GetHashCode(Key);
            int h2 = Value == null ? 0 : EqualityComparer<V>.Default.GetHashCode(Value);
            unchecked { return (h1 * 397) ^ h2; }
        }

        public override string ToString() => $"{Key}={Value}";
    }

    private sealed class Node
    {
        public K Key;
        public V Value;
        public Node Left;
        public Node Right;
        public Node Parent;

        public Node(K key, V value, Node parent)
        {
            Key = key;
            Value = value;
            Parent = parent;
        }

        public Entry ToEntry() => new Entry(Key, Value);
    }

    private readonly IComparer<K> comparator;
    private Node root;
    private int size;

    public MyTreeMap()
    {
        comparator = Comparer<K>.Default;
        ValidateComparerForNaturalOrder();
    }

    public MyTreeMap(TreeMapComparator<K> comp)
    {
        if (comp == null) throw new ArgumentNullException(nameof(comp));
        comparator = Comparer<K>.Create((a, b) => comp(a, b));
    }

    public void clear()
    {
        root = null;
        size = 0;
    }

    public bool containsKey(object key)
    {
        if (key == null) throw new ArgumentNullException(nameof(key));
        if (key is not K k) return false;
        return FindNode(k) != null;
    }

    public bool containsValue(object value)
    {
        if (root == null) return false;
        return ContainsValueDfs(root, value);
    }

    public ISet<Entry> entrySet()
    {
        var set = new HashSet<Entry>();
        InOrder(root, n => set.Add(n.ToEntry()));
        return set;
    }

    public V get(object key)
    {
        if (key == null) throw new ArgumentNullException(nameof(key));
        if (key is not K k) return default;
        var n = FindNode(k);
        return n == null ? default : n.Value;
    }

    public bool isEmpty() => size == 0;

    public ISet<K> keySet()
    {
        var set = new HashSet<K>();
        InOrder(root, n => set.Add(n.Key));
        return set;
    }

    public V put(K key, V value)
    {
        if (key == null) throw new ArgumentNullException(nameof(key));

        if (root == null)
        {
            root = new Node(key, value, null);
            size = 1;
            return default;
        }

        Node cur = root;
        Node parent = null;
        int cmp = 0;

        while (cur != null)
        {
            parent = cur;
            cmp = Compare(key, cur.Key);
            if (cmp < 0) cur = cur.Left;
            else if (cmp > 0) cur = cur.Right;
            else
            {
                var old = cur.Value;
                cur.Value = value;
                return old;
            }
        }

        var node = new Node(key, value, parent);
        if (cmp < 0) parent.Left = node;
        else parent.Right = node;
        size++;
        return default;
    }

    public V remove(object key)
    {
        if (key == null) throw new ArgumentNullException(nameof(key));
        if (key is not K k) return default;

        var node = FindNode(k);
        if (node == null) return default;

        var old = node.Value;
        DeleteNode(node);
        size--;
        return old;
    }

    public int size() => size;
    public K firstKey()
    {
        if (root == null) throw new InvalidOperationException("Map is empty.");
        return MinNode(root).Key;
    }

    public K lastKey()
    {
        if (root == null) throw new InvalidOperationException("Map is empty.");
        return MaxNode(root).Key;
    }

    public MyTreeMap<K, V> headMap(K end)
    {
        if (end == null) throw new ArgumentNullException(nameof(end));
        var m = CloneEmpty();
        InOrder(root, n =>
        {
            if (Compare(n.Key, end) < 0) m.put(n.Key, n.Value);
        });
        return m;
    }

    public MyTreeMap<K, V> subMap(K start, K end)
    {
        if (start == null) throw new ArgumentNullException(nameof(start));
        if (end == null) throw new ArgumentNullException(nameof(end));
        if (Compare(start, end) > 0) throw new ArgumentException("start must be <= end.");

        var m = CloneEmpty();
        InOrder(root, n =>
        {
            if (Compare(n.Key, start) >= 0 && Compare(n.Key, end) < 0) m.put(n.Key, n.Value);
        });
        return m;
    }

    public MyTreeMap<K, V> tailMap(K start)
    {
        if (start == null) throw new ArgumentNullException(nameof(start));
        var m = CloneEmpty();
        InOrder(root, n =>
        {
            if (Compare(n.Key, start) > 0) m.put(n.Key, n.Value);
        });
        return m;
    }

    public Entry lowerEntry(K key) => NavEntry(key, Relation.Lower);
    public Entry floorEntry(K key) => NavEntry(key, Relation.Floor);
    public Entry higherEntry(K key) => NavEntry(key, Relation.Higher);
    public Entry ceilingEntry(K key) => NavEntry(key, Relation.Ceiling);

    public K lowerKey(K key) => (lowerEntry(key)?.Key);
    public K floorKey(K key) => (floorEntry(key)?.Key);
    public K higherKey(K key) => (higherEntry(key)?.Key);
    public K ceilingKey(K key) => (ceilingEntry(key)?.Key);

    public Entry pollFirstEntry()
    {
        if (root == null) return null;
        var n = MinNode(root);
        var e = n.ToEntry();
        DeleteNode(n);
        size--;
        return e;
    }

    public Entry pollLastEntry()
    {
        if (root == null) return null;
        var n = MaxNode(root);
        var e = n.ToEntry();
        DeleteNode(n);
        size--;
        return e;
    }

    public Entry firstEntry() => root == null ? null : MinNode(root).ToEntry();
    public Entry lastEntry() => root == null ? null : MaxNode(root).ToEntry();

    private enum Relation { Lower, Floor, Higher, Ceiling }

    private Entry NavEntry(K key, Relation rel)
    {
        if (key == null) throw new ArgumentNullException(nameof(key));
        Node cur = root;
        Node candidate = null;

        while (cur != null)
        {
            int cmp = Compare(key, cur.Key);
            if (cmp == 0)
            {
                if (rel == Relation.Floor || rel == Relation.Ceiling) return cur.ToEntry();
                if (rel == Relation.Lower)
                {
                    var p = Predecessor(cur);
                    return p?.ToEntry();
                }
                else
                {
                    var s = Successor(cur);
                    return s?.ToEntry();
                }
            }

            if (cmp < 0)
            {
                if (rel == Relation.Higher || rel == Relation.Ceiling) candidate = cur;
                cur = cur.Left;
            }
            else
            {
                if (rel == Relation.Lower || rel == Relation.Floor) candidate = cur;
                cur = cur.Right;
            }
        }

        return candidate?.ToEntry();
    }

    private int Compare(K a, K b) => comparator.Compare(a, b);

    private void ValidateComparerForNaturalOrder()
    {
        _ = comparator;
    }

    private Node FindNode(K key)
    {
        Node cur = root;
        while (cur != null)
        {
            int cmp = Compare(key, cur.Key);
            if (cmp < 0) cur = cur.Left;
            else if (cmp > 0) cur = cur.Right;
            else return cur;
        }
        return null;
    }
    private static Node MinNode(Node n)
    {
        while (n.Left != null) n = n.Left;
        return n;
    }

    private static Node MaxNode(Node n)
    {
        while (n.Right != null) n = n.Right;
        return n;
    }

    private static Node Successor(Node n)
    {
        if (n.Right != null) return MinNode(n.Right);
        var p = n.Parent;
        var cur = n;
        while (p != null && cur == p.Right)
        {
            cur = p;
            p = p.Parent;
        }
        return p;
    }

    private static Node Predecessor(Node n)
    {
        if (n.Left != null) return MaxNode(n.Left);
        var p = n.Parent;
        var cur = n;
        while (p != null && cur == p.Left)
        {
            cur = p;
            p = p.Parent;
        }
        return p;
    }

    private void DeleteNode(Node node)
    {
        if (node.Left != null && node.Right != null)
        {
            var s = Successor(node);
            node.Key = s.Key;
            node.Value = s.Value;
            node = s;
        }

        Node child = node.Left ?? node.Right;

        if (child != null) child.Parent = node.Parent;

        if (node.Parent == null)
        {
            root = child;
        }
        else if (node == node.Parent.Left)
        {
            node.Parent.Left = child;
        }
        else
        {
            node.Parent.Right = child;
        }

        node.Left = null;
        node.Right = null;
        node.Parent = null;
    }

    private static void InOrder(Node n, Action<Node> action)
    {
        if (n == null) return;
        InOrder(n.Left, action);
        action(n);
        InOrder(n.Right, action);
    }

    private bool ContainsValueDfs(Node n, object value)
    {
        if (n == null) return false;
        if (EqualityComparer<V>.Default.Equals(n.Value, value is V vv ? vv : default) && (value is V || value == null && n.Value == null))
            return true;
        return ContainsValueDfs(n.Left, value) || ContainsValueDfs(n.Right, value);
    }

    private MyTreeMap<K, V> CloneEmpty()
    {
        if (comparator == Comparer<K>.Default)
            return new MyTreeMap<K, V>();
        return new MyTreeMap<K, V>((a, b) => comparator.Compare(a, b));
    }
}

public static class Program
{
    public static void Main()
    {
        var map = new MyTreeMap<int, string>();
        map.put(5, "five");
        map.put(2, "two");
        map.put(8, "eight");
        map.put(1, "one");
        map.put(3, "three");

        Console.WriteLine(map.size());
        Console.WriteLine(map.firstKey());
        Console.WriteLine(map.lastKey());
        Console.WriteLine(map.get(3));

        Console.WriteLine(map.lowerEntry(3));
        Console.WriteLine(map.floorEntry(3));
        Console.WriteLine(map.higherEntry(3));
        Console.WriteLine(map.ceilingEntry(3));

        var head = map.headMap(5);
        foreach (var e in head.entrySet()) Console.WriteLine(e);

        var sub = map.subMap(2, 8);
        foreach (var e in sub.entrySet()) Console.WriteLine(e);

        var tail = map.tailMap(2);
        foreach (var e in tail.entrySet()) Console.WriteLine(e);

        Console.WriteLine(map.pollFirstEntry());
        Console.WriteLine(map.pollLastEntry());
        Console.WriteLine(map.size());

        map.remove(5);
        Console.WriteLine(map.containsKey(5));
        Console.WriteLine(map.containsValue("two"));
        map.clear();
        Console.WriteLine(map.isEmpty());

        var rev = new MyTreeMap<int, string>((a, b) => b.CompareTo(a));
        rev.put(10, "ten");
        rev.put(7, "seven");
        rev.put(12, "twelve");
        Console.WriteLine(rev.firstKey());
        Console.WriteLine(rev.lastKey());
    }
}