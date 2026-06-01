using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.IO;

namespace ConsoleApp40
{
    internal class Program
    {
        public static List<string> GetSortedWords(string line)
        {
            List<string> words = new List<string>();

            string[] parts = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < parts.Length; i++)
            {
                words.Add(parts[i]);
            }

            words.Sort((a, b) => a.Length.CompareTo(b.Length));

            return words;
        }
        public static int CompareLines(string line1, string line2)
        {
            List<string> words1 = GetSortedWords(line1);
            List<string> words2 = GetSortedWords(line2);

            int min = Math.Min(words1.Count, words2.Count);

            for (int i = 0; i < min; i++)
            {
                if (words1[i].Length < words2[i].Length)
                    return -1;

                if (words1[i].Length > words2[i].Length)
                    return 1;
            }

            if (words1.Count < words2.Count)
                return -1;

            if (words1.Count > words2.Count)
                return 1;

            return 0;
        }
        static void Main(string[] args)
        {
            Console.WriteLine("Выберите номер задачи: 21 25 26 27");
            int task = int.Parse(Console.ReadLine());
            if (task == 21)
            {
                MyHashMap<string, int> map = new MyHashMap<string, int>();
                /* тест задания 21*/
                map.Put("one", 1);
                map.Put("two", 2);
                map.Put("three", 3);
                Console.WriteLine(map.Get("one"));
                Console.WriteLine(map.Get("two"));
                Console.WriteLine(map.Get("three"));
                map.Put("one", 100);
                Console.WriteLine(map.Get("one"));
                Console.WriteLine(map.containsKey("two"));
                map.Remove("two");
                Console.WriteLine(map.containsKey("two"));
                Console.WriteLine(map.size());
            }

            if (task == 25)
            {
                MyHashSet<int> set = new MyHashSet<int>();
                set.add(10);
                set.add(20);
                set.add(30);
                set.add(10);
                Console.WriteLine(set.size()); // 3
                Console.WriteLine(set.contains(20)); // True
                set.remove(20);
                Console.WriteLine(set.contains(20)); // False
                object[] arr = set.toArray();
                for (int i = 0; i < arr.Length; i++)
                {
                    Console.WriteLine(arr[i]);
                }
            }

            if (task == 26)
            {
                string inputFileName = "/Users/m/Documents/JetBrains Rider/c#/ConsoleApp13/ConsoleApp13/input26.txt";
                string outputFileName = "/Users/m/Documents/JetBrains Rider/c#/ConsoleApp13/ConsoleApp13/output26.txt";

                if (!File.Exists(inputFileName))
                {
                    Console.WriteLine("Файл input.txt не найден.");
                    return;
                }

                MyHashSet<string> set = new MyHashSet<string>();

                StreamReader reader = new StreamReader(inputFileName);

                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (line.Trim().Length > 0)
                    {
                        bool exists = false;

                        string[] currentLines = set.toArray(new string[0]);

                        for (int i = 0; i < currentLines.Length; i++)
                        {
                            if (CompareLines(currentLines[i], line) == 0)
                            {
                                exists = true;
                                break;
                            }
                        }

                        if (exists == false)
                        {
                            set.add(line);
                        }
                    }
                }

                reader.Close();

                string[] arr = set.toArray(new string[0]);

                for (int i = 0; i < arr.Length - 1; i++)
                {
                    for (int j = 0; j < arr.Length - 1 - i; j++)
                    {
                        if (CompareLines(arr[j], arr[j + 1]) > 0)
                        {
                            string temp = arr[j];
                            arr[j] = arr[j + 1];
                            arr[j + 1] = temp;
                        }
                    }
                }

                StreamWriter writer = new StreamWriter(outputFileName);

                for (int i = 0; i < arr.Length; i++)
                {
                    writer.WriteLine(arr[i]);
                }

                writer.Close();

                Console.WriteLine("Задача 26 выполнена.Запись в output26.txt");
            }
            if (task == 27)
            {
                string inputFileName = "/Users/m/Documents/JetBrains Rider/c#/ConsoleApp13/ConsoleApp13/input27.txt";
                string outputFileName = "/Users/m/Documents/JetBrains Rider/c#/ConsoleApp13/ConsoleApp13/output27.txt";

                if (!File.Exists(inputFileName))
                {
                    Console.WriteLine("Файл input.txt не найден.");
                    return;
                }

                MyHashSet<string> wordsSet = new MyHashSet<string>();

                StreamReader reader = new StreamReader(inputFileName);

                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    MatchCollection matches = Regex.Matches(line, "[A-Za-z]+");

                    for (int i = 0; i < matches.Count; i++)
                    {
                        string word = matches[i].Value.ToLower();
                        wordsSet.add(word);
                    }
                }

                reader.Close();

                string[] words = wordsSet.toArray(new string[0]);

                Array.Sort(words);

                StreamWriter writer = new StreamWriter(outputFileName);

                for (int i = 0; i < words.Length; i++)
                {
                    writer.WriteLine(words[i]);
                }

                writer.Close();

                Console.WriteLine("Задача 27 выполнена. Запись в output27.txt");
            }
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
    class MyHashSet<T>
    {
        private MyHashMap<T, object> map;
        private readonly object fakeValue = new object();

        public MyHashSet()
        {
            map = new MyHashMap<T, object>(16, 0.75f);
        }

        public MyHashSet(T[] a)
        {
            map = new MyHashMap<T, object>(16, 0.75f);
            addAll(a);
        }

        public MyHashSet(int initialCapacity, float loadFactor)
        {
            map = new MyHashMap<T, object>(initialCapacity, loadFactor);
        }

        public MyHashSet(int initialCapacity)
        {
            map = new MyHashMap<T, object>(initialCapacity, 0.75f);
        }

        public bool add(T e)
        {
            if (map.containsKey(e))
            {
                return false;
            }

            map.Put(e, fakeValue);
            return true;
        }

        public bool addAll(T[] a)
        {
            bool changed = false;

            if (a == null)
            {
                return false;
            }

            for (int i = 0; i < a.Length; i++)
            {
                bool result = add(a[i]);

                if (result == true)
                {
                    changed = true;
                }
            }

            return changed;
        }

        public void clear()
        {
            map.clear();
        }



        public bool containsAll(T[] a)
        {
            if (a == null)
            {
                return false;
            }

            for (int i = 0; i < a.Length; i++)
            {
                if (contains(a[i]) == false)
                {
                    return false;
                }
            }

            return true;
        }

        public bool isEmpty()
        {
            return map.isEmpty();
        }

        public bool contains(object o)
        {
            if (o is T)
            {
                T value = (T)o;
                return map.containsKey(value);
            }

            return false;
        }

        public bool remove(object o)
        {
            if (contains(o) == false)
            {
                return false;
            }

            T value = (T)o;
            map.Remove(value);

            return true;
        }

        public bool removeAll(T[] a)
        {
            bool changed = false;

            if (a == null)
            {
                return false;
            }

            for (int i = 0; i < a.Length; i++)
            {
                bool result = remove(a[i]);

                if (result == true)
                {
                    changed = true;
                }
            }

            return changed;
        }

        public bool retainAll(T[] a)
        {
            bool changed = false;

            if (a == null)
            {
                if (isEmpty() == false)
                {
                    clear();
                    return true;
                }

                return false;
            }

            T[] keys = map.keySet();

            for (int i = 0; i < keys.Length; i++)
            {
                bool found = false;

                for (int j = 0; j < a.Length; j++)
                {
                    if (object.Equals(keys[i], a[j]))
                    {
                        found = true;
                        break;
                    }
                }

                if (found == false)
                {
                    remove(keys[i]);
                    changed = true;
                }
            }

            return changed;
        }

        public int size()
        {
            return map.size();
        }

        public object[] toArray()
        {
            T[] keys = map.keySet();
            object[] result = new object[keys.Length];

            for (int i = 0; i < keys.Length; i++)
            {
                result[i] = keys[i];
            }

            return result;
        }

        public T[] toArray(T[] a)
        {
            T[] keys = map.keySet();

            if (a == null || a.Length < keys.Length)
            {
                a = new T[keys.Length];
            }

            for (int i = 0; i < keys.Length; i++)
            {
                a[i] = keys[i];
            }

            if (a.Length > keys.Length)
            {
                a[keys.Length] = default(T);
            }

            return a;
        }
    }
}