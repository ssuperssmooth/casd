using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


namespace задача_26
{
    public class Entry<T, P>
    {
        public T id;
        public P value;

        public Entry(T id, P value)
        {
            this.id = id;
            this.value = value;
        }
    }
    public class MyHashMap<T, P>
    {
        public LinkedList<Entry<T, P>>[] table;
        public int size;
        public int count;
        public float loadFactor;

        public MyHashMap()
        {
            table = new LinkedList<Entry<T, P>>[16];
            size = 16;
            count = 0;
            loadFactor = 0.75f;
        }
        public MyHashMap(int initialCapacity)
        {
            table = new LinkedList<Entry<T, P>>[initialCapacity];
            size = initialCapacity;
            count = 0;
            loadFactor = 0.75f;
        }
        public MyHashMap(int initialCapacity, float loadFactor)
        {
            table = new LinkedList<Entry<T, P>>[initialCapacity];
            size = initialCapacity;
            count = 0;
            this.loadFactor = loadFactor;
        }
        public void recize()
        {
            var old = table;
            int newsize = size * 2;

            table = new LinkedList<Entry<T, P>>[newsize];
            size = newsize;
            count = 0;

            foreach (var buck in old)
            {
                if (buck != null)
                {
                    foreach (var entry in buck)
                    {
                        put(entry.id, entry.value);
                    }
                }
            }


        }
        public int hashCode(T key)
        {
            return Math.Abs(key.GetHashCode()) % size;
        }
        public void clear()
        {
            table = new LinkedList<Entry<T, P>>[size];
            count = 0;
        }
        public bool containsKey(object key)
        {
            if (key == null)
                return false;
            T tKEY = (T)key;

            int hashkey = hashCode(tKEY);
            if (table[hashkey] == null)
                return false;

            foreach (var entry in table[hashkey])
            {
                if (entry.id.Equals(tKEY))
                {
                    return true;
                }
            }
            return false;
        }
        public bool containsValue(object value)
        {
            if (value == null)
                return false;
            for (int i = 0; i < size; i++)
            {
                if (table[i] != null)
                {
                    foreach (var valuee in table[i])
                    {
                        if (valuee.value.Equals(value))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        public List<Entry<T, P>> entrySet()
        {
            List<Entry<T, P>> res = new List<Entry<T, P>>();

            for (int i = 0; i < size; i++)
            {
                if (table[i] != null)
                {
                    foreach (var entry in table[i])
                    {
                        res.Add(entry);
                    }
                }

            }
            return res;
        }
        public P get(object key)
        {
            if (key == null)
                return default(P);

            T tKey = (T)key;
            int hashKey = hashCode(tKey);

            if (table[hashKey] != null)
            {
                foreach (var entry in table[hashKey])
                {
                    if (entry.id.Equals(tKey))
                    {
                        return entry.value;
                    }
                }
            }
            return default(P);
        }
        public bool isEmpty()
        {
            if (count == 0) return false;
            return true;

        }
        public List<T> keySet()
        {
            List<T> res = new List<T>();

            for (int i = 0; i < size; i++)
            {
                if (table[i] != null)
                {
                    foreach (var entry in table[i])
                    {
                        res.Add(entry.id);
                    }
                }
            }
            return res;
        }
        public bool put(T key, P value)
        {
            if (key == null)
                return false;


            if (count >= size * loadFactor)
            {
                recize();
            }

            int hashKey = hashCode(key);
            if (table[hashKey] == null)
            {
                table[hashKey] = new LinkedList<Entry<T, P>>();
            }

            foreach (var entry in table[hashKey])
            {
                if (entry.id.Equals(key))
                {
                    entry.value = value;
                    return true;
                }
            }
            table[hashKey].AddLast(new Entry<T, P>(key, value));
            count++;
            return true;

        }
        public bool remove(object key)
        {
            if (key == null) return false;

            T tkey = (T)key;

            int hashKey = hashCode(tkey);
            if (table[hashKey] != null)
            {
                foreach (var entry in table[hashKey])
                {
                    if (entry.id.Equals(tkey))
                    {
                        table[hashKey].Remove(entry);
                        count--;
                        return true;
                    }
                }
            }
            return false;
        }
        public int Size()
        {
            return count;
        }
    }

    public class MyHashSet<E>
    {
        private MyHashMap<E, object> map;
        private static readonly object PRESENT = new object();

        public MyHashSet()
        {
            map = new MyHashMap<E, object>(16, 0.75f);

        }
        public MyHashSet(E[] a)
        {
            map = new MyHashMap<E, object>();
            foreach (E elem in a)
            {
                map.put(elem, elem);
            }

        }
        public MyHashSet(int InitialCapac, float LoadFac)
        {
            map = new MyHashMap<E, object>(InitialCapac, LoadFac);
        }
        public MyHashSet(int initCpac)
        {
            map = new MyHashMap<E, object>(initCpac, 0.75f);
        }
        public bool add(E e)
        {
            if (map.containsKey(e))
            {
                Console.WriteLine("Такой элем уже есть");
                return false;

            }
            map.put(e, PRESENT);
            return true;
        }
        public void AddAll(E[] a)
        {
            foreach (E elem in a)
            {
                add(elem);
            }
        }
        public void Clear()
        {
            map.clear();
        }
        public bool Contains(object o)
        {
            return map.containsKey(o);
        }
        public void ContAll(E[] a)
        {
            foreach (E elem in a)
            {
                map.containsKey(elem);
            }
        }
        public void IsEmpt()
        {
            map.isEmpty();
        }
        public bool Remove(object o)
        {
            if (map.containsKey(o))
            {
                map.remove(o);
                return true;
            }
            return false;
        }
        public void RemoveAll(E[] a)
        {
            foreach (E elem in a)
            {
                Remove(elem);
            }
        }
        public bool retainAll(E[] a)
        {
            MyHashSet<E> mapp = new MyHashSet<E>();
            foreach (E elem in a)
            {
                mapp.add(elem);
            }

            E[] curr = toArray();
            bool changed = false;

            foreach (E item in curr)
            {
                if (!mapp.Contains(item))
                {
                    Remove(item);
                    changed = true;
                }
            }
            return changed;
        }
        public int size()
        {
            return map.Size();
        }
        public E[] toArray()
        {
            E[] result = new E[size()];
            int index = 0;
            var keys = map.keySet();
            foreach (E key in keys)
            {
                result[index++] = key;
            }
            return result;
        }

        public E[] toArray(E[] a)
        {
            if (a == null)
                a = new E[size()];

            int i = 0;
            foreach (E key in map.keySet())
                if (i < a.Length)
                    a[i++] = key;

            return a;
        }
    }
    public class StringToNum
    {
        public string origStr;
        public List<int> lenghts;

        public StringToNum(string str)
        {
            origStr = str;
            lenghts = SortLen(str);
        }
        public List<int> SortLen(string str)
        {
            string[] strBezProb = str.Split(' ');
            List<int> resLen = new List<int>();
            foreach (string word in strBezProb)
            {
                if (word.Length > 0)
                {
                    resLen.Add(word.Length);
                }
            }
            resLen.Sort();

            return resLen;
        }
        public override bool Equals(object obj)
        {
            StringToNum stroka = (StringToNum)obj;

            if (this.lenghts.Count() == stroka.lenghts.Count())
            {
                for (int i = 0; i < this.lenghts.Count(); i++)
                {
                    if (this.lenghts[i] != stroka.lenghts[i])
                    {
                        return false;
                    }
                }
            }
            if (this.lenghts.Count() != stroka.lenghts.Count())
            {
                return false;
            }
            return true;
        }
        public override int GetHashCode()
        {
            int hash = 17;

            foreach (int leng in lenghts)
            {
                hash = hash * 31 + leng;
            }

            return hash;

        }


    }


    internal class Program
    {
        static int ComparStrok(StringToNum a, StringToNum b)
        {
            List<int> lensA = a.lenghts;
            List<int> lensB = b.lenghts;

            int minLen = Math.Min(lensA.Count, lensB.Count);

            for (int i = 0; i < minLen; i++)
            {
                if (lensA[i] > lensB[i])
                {
                    return 1;
                }
                if (lensA[i] < lensB[i])
                {
                    return -1;
                }

            }
            if (lensA.Count > lensB.Count) return 1;
            if (lensA.Count < lensB.Count) return -1;

            return 0;

        }
        static void Main(string[] args)
        {
            string[] lines = File.ReadAllLines("input.txt");

            MyHashSet<StringToNum> set = new MyHashSet<StringToNum>();
            foreach (string line in lines)
            {
                set.add(new StringToNum(line));
            }

            StringToNum[] uniq = set.toArray();

            Array.Sort(uniq, ComparStrok);
            string[] result = new string[uniq.Length];
            for (int i = 0; i < uniq.Length; i++)
            {
                result[i] = uniq[i].origStr;
            }


            File.WriteAllLines("output.txt", result);
        }
    }
}