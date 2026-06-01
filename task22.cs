using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;

namespace Task21_22
{
    internal class Program
    {
        static void Main(string[] args)
        {
            MyHashMap<string, int> map = new MyHashMap<string, int>();
            string[] lines = File.ReadAllLines("input.txt");
            Regex regex = new Regex(@"</?[A-Za-z][A-Za-z0-9]*>");
            for (int i = 0; i < lines.Length; i++)
            {
                MatchCollection matches = regex.Matches(lines[i]);
                foreach (Match match in matches)
                {
                    string tag = match.Value;
                    tag = tag.Replace("<", "");
                    tag = tag.Replace(">", "");
                    tag = tag.Replace("/", "");
                    tag = tag.ToLower();
                    int count = map.Get(tag);
                    map.Put(tag, count + 1);
                }
            }
            HashSet<Entry<string, int>> entries = map.EntrySet();
            foreach (Entry<string, int> entry in entries)
            {
                Console.WriteLine(entry.key + " " + entry.value);
            }
        }
    }


    public class Entry<K, V>
    {
        public K key { get; set; }
        public V value { get; set; }
        public Entry(K key, V value)
        {
            this.key = key;
            this.value = value;
        }
    }
    public class MyHashMap<K, V>
    {
        private LinkedList<Entry<K, V>>[] table;
        private int size;
        private float loadFactor;

        public MyHashMap()
        {
            table = new LinkedList<Entry<K, V>>[16];
            size = 0;
            loadFactor = 0.75f;
        }
        public MyHashMap(int initialCapacity)
        {
            if (initialCapacity <= 0) throw new ArgumentException("Начальная емкость должна быть больше 0");
            table = new LinkedList<Entry<K, V>>[initialCapacity];
            size = 0;
            loadFactor = 0.75f;
        }
        public MyHashMap(int initialCapacity, float loadFactor)
        {
            if (initialCapacity <= 0) throw new ArgumentException("Начальная емкость должна быть больше 0");
            if (loadFactor <= 0) throw new ArgumentException("Коэффициент загрузки должен быть больше 0");
            table = new LinkedList<Entry<K, V>>[initialCapacity];
            size = 0;
            this.loadFactor = loadFactor;
        }
        public void Clear()
        {
            table = new LinkedList<Entry<K, V>>[table.Length];
            size = 0;
        }
        public bool ContainsKey(object key)
        {
            return GetEntry(key) != null;
        }
        public bool ContainsValue(object value)
        {
            for (int i = 0; i < table.Length; i++)
            {
                if (table[i] != null)
                {
                    foreach (Entry<K, V> entry in table[i])
                    {
                        if (Equals(entry.value, value)) return true;
                    }
                }
            }
            return false;
        }
        public HashSet<Entry<K, V>> EntrySet()
        {
            HashSet<Entry<K, V>> set = new HashSet<Entry<K, V>>();
            for (int i = 0; i < table.Length; i++)
            {
                if (table[i] != null)
                {
                    foreach (Entry<K, V> entry in table[i]) set.Add(entry);
                }
            }
            return set;
        }
        public V Get(object key)
        {
            Entry<K, V> entry = GetEntry(key);
            if (entry == null) return default(V);
            return entry.value;
        }
        public bool IsEmpty()
        {
            return size == 0;
        }
        public HashSet<K> KeySet()
        {
            HashSet<K> set = new HashSet<K>();
            for (int i = 0; i < table.Length; i++)
            {
                if (table[i] != null)
                {
                    foreach (Entry<K, V> entry in table[i]) set.Add(entry.key);
                }
            }
            return set;
        }
        public V Put(K key, V value)
        {
            if ((float)(size + 1) / table.Length > loadFactor) Resize();
            int index = GetIndex(key);
            if (table[index] == null) table[index] = new LinkedList<Entry<K, V>>();
            foreach (Entry<K, V> entry in table[index])
            {
                if (Equals(entry.key, key))
                {
                    V oldValue = entry.value;
                    entry.value = value;
                    return oldValue;
                }
            }
            table[index].AddLast(new Entry<K, V>(key, value));
            size++;
            return default(V);
        }
        public V Remove(object key)
        {
            int index = GetIndex(key);
            if (table[index] == null) return default(V);
            LinkedListNode<Entry<K, V>> current = table[index].First;
            while (current != null)
            {
                if (Equals(current.Value.key, key))
                {
                    V oldValue = current.Value.value;
                    table[index].Remove(current);
                    size--;
                    return oldValue;
                }
                current = current.Next;
            }
            return default(V);
        }
        public int Size()
        {
            return size;
        }
        private Entry<K, V> GetEntry(object key)
        {
            int index = GetIndex(key);
            if (table[index] == null) return null;
            foreach (Entry<K, V> entry in table[index])
            {
                if (Equals(entry.key, key)) return entry;
            }
            return null;
        }
        private int GetIndex(object key)
        {
            int hash;
            if (key == null) hash = 0;
            else hash = key.GetHashCode();
            if (hash < 0) hash = -hash;
            return hash % table.Length;
        }
        private void Resize()
        {
            LinkedList<Entry<K, V>>[] oldTable = table;
            table = new LinkedList<Entry<K, V>>[oldTable.Length * 2];
            size = 0;
            for (int i = 0; i < oldTable.Length; i++)
            {
                if (oldTable[i] != null)
                {
                    foreach (Entry<K, V> entry in oldTable[i]) Put(entry.key, entry.value);
                }
            }
        }
    }
}

