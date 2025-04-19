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
        public int TotalWords => TrieMetrics.LeafNodesCount(root);
        public int InternalNodes => TrieMetrics.InternalNodesCount(root);
        public int BranchingNodes => TrieMetrics.BranchingNodesCount(root);
        public double AvgBranchingFactor => TrieMetrics.AvgBranchingFactor(root);

        public void PrintMetrics()
        {
            Console.WriteLine("\nМетрики префиксного дерева:");
            Console.WriteLine($"1. Общее количество символов: {TotalCharacters}");
            Console.WriteLine($"2. Количество слов (листовых вершин в дереве): {TotalWords}");
            Console.WriteLine($"3. Количество внутренних вершин: {InternalNodes}");
            Console.WriteLine($"4. Количество ветвлений (внутренних вершин из которых более одного пути): {BranchingNodes}");
            Console.WriteLine($"5. Среднее количество путей в вершинах ветвлений: {AvgBranchingFactor:F2}\n");
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
                TotalCharacters += key.Length;
            node.Value = key[^1];
            node.IsKey = true;
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
            if (node.IsKey)
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
                if (d != null && d.IsKey)
                    results.Add(d);
            return results;
        }
        
        public List<string>? SearchWords(string key)
        {
            List<string>? results = new();
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
                    values.Add(value);
                }
            }
            return values;
        }

        static void Main()
        {
            Trie tree = new(false);
            List<string> values = ReadFile("words.txt");

            Console.WriteLine($"Загружено слов: {values.Count}");

            // Загрузка в дерево

            foreach (string word in values)
                tree.Insert(word);

            // Вывод метрик после загрузки
            tree.PrintMetrics();

            while (true)
            {
                Console.Write("Введите префикс для поиска (пустая строка для завершения работы): ");
                string? input;
                while ((input = Console.ReadLine()) == null)
                    Console.Write("Попробуйте еще раз: ");
                if (input == "")
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