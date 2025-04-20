using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace algos2
{
    public class Trie
    {
        INode root;

        public Trie(bool withArray)
        {
            if (withArray)
                root = new NodeArray();
            else
                root = new NodeList();
        }

        // Метрики для всего дерева
        public int TotalCharacters { get; private set; }

        public void PrintMetrics(TrieMetrics metrics)
        {
            metrics.CountStats(root, true);
            Console.WriteLine("\nМетрики префиксного дерева:");
            Console.WriteLine($"1. Общее количество символов: {TotalCharacters}");
            Console.WriteLine($"2. Количество слов (листовых вершин в дереве): {metrics.TotalWords}");
            Console.WriteLine($"3. Количество внутренних вершин: {metrics.InternalNodes}");
            Console.WriteLine($"4. Количество ветвлений (внутренних вершин из которых более одного пути): {metrics.BranchingNodes}");
            Console.WriteLine($"5. Среднее количество путей в вершинах ветвлений: {metrics.AvgBranching:F2}\n");
        }

        public void Insert(string key)
        {
            //TotalCharacters += key.Length;
            bool flagNew = false;
            INode node = root;
            for (int i = 0; i < key.Length; i++)
            {
                char ch = key[i];
                if (!node.HasChild(ch))
                {
                    node = node.AddChild(ch);
                    flagNew = true;
                }
                else
                    node = node.GetChild(ch);
            }
            if (flagNew)
                TotalCharacters += key.Length - 1;
            node.Value = key[^1];
            //node.IsKey = true;
        }

        public INode? Lookup(string key)
        {
            INode? node = root;
            for (int i = 0; i < key.Length; i++)
            {
                char ch = key[i];
                if (!node.HasChild(ch))
                    return null;
                node = node.GetChild(ch);
            }
            //if (node.IsKey)
            if (node.Value == '$')
                return node;
            else
                return null;
        }

        public List<INode>? Search(string key)
        {
            List<INode>? results = new();
            if (key.Length == 0)
                return results;
            INode? node = root;
            for (int i = 0; i < key.Length; i++)
            {
                char ch = key[i];
                if (!node.HasChild(ch))
                    return null;
                node = node.GetChild(ch);
            }
            foreach (INode d in node.GetDescendants())
                //if (d != null && d.IsKey)
                if (d != null && d.Value == '$')
                    results.Add(d);
            return results;
        }
        
        public List<string>? SearchWords(string key)
        {
            List<string>? results = new();
            //if (key.Length == 0)
                //return results;
            INode? node = root;
            for (int i = 0; i < key.Length; i++)
            {
                char ch = key[i];
                if (!node.HasChild(ch))
                    return null;
                node = node.GetChild(ch);
            }
            foreach (string d in node.GetWords(key, key.Length))
                results.Add(d);
            return results;
        }
    }

    class Program
    {
        public static List<string> ReadFile(string filename)
        {
            List<string> values = new();
            using (StreamReader file = new StreamReader(filename))
            {
                string? value;
                while ((value = file.ReadLine()) != null)
                {
                    //values.Add(value.Split(". ")[1]);
                    values.Add(value + '$');
                }
            }
            return values;
        }

        static void Main()
        {
            Trie tree = new(true);
            TrieMetrics metrics = new();
            List<string> values = ReadFile("words.txt");
            Stopwatch stopwatch1 = new();

            Console.WriteLine($"Загружено слов: {values.Count}");

            // Загрузка в дерево
            stopwatch1.Start();
            foreach (string word in values)
                tree.Insert(word);
            stopwatch1.Stop();
            TimeSpan ts = stopwatch1.Elapsed;
            Console.WriteLine("Результат построения дерева с массивом вершин " + ts.TotalMilliseconds + " мсек.");

            // Вывод метрик после загрузки
            tree.PrintMetrics(metrics);

            tree = new(false);

            // Загрузка в дерево
            stopwatch1.Reset();
            stopwatch1.Start();
            foreach (string word in values)
                tree.Insert(word);
            stopwatch1.Stop();
            ts = stopwatch1.Elapsed;
            Console.WriteLine("Результат построения дерева с односвязным списком вершин " + ts.TotalMilliseconds + " мсек.");

            tree.PrintMetrics(metrics);

            while (true)
            {
                Console.Write("Введите префикс для поиска (# для завершения работы): ");
                string? input;
                while ((input = Console.ReadLine()) == null)
                    Console.Write("Попробуйте еще раз: ");
                if (input == "#")
                    break;

                try
                {
                    List<string>? search = tree.SearchWords(input);
                    if (search == null || search.Count == 0)
                        Console.WriteLine("Не найдено результатов");
                    else
                    {
                        Console.WriteLine($"Найдено слов: {search.Count}");
                        foreach (var word in search)
                            Console.WriteLine(word);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
}