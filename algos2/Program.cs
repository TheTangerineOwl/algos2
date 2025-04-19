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

        /*
        int charCount = 0;
        int wordCount = 0;
        int innerNodeCount = 0;
        int branchCount = 0;
        int avPath = 0;

        public int CharCount
        {
            get => charCount;
        }

        public int WordCount
        {
            get => wordCount;
        }

        public int InnerNodeCount
        {
            get => innerNodeCount;
        }

        public int BranchCount
        {
            get => branchCount;
        }

        public int AveragePathCount
        {
            get => avPath;
        }*/

        public void Insert(string key)
        {
            INode node = root;
            for (int i = 0; i < key.Length; i++)
            {
                char ch = key[i];
                if (!node.HasChild(ch))
                    node = node.AddChild(ch);
                else
                    node = node.GetChild(ch);
            }
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
            Trie tree = new(true);
            List<string> values = ReadFile("words.txt");
            foreach (string word in values)
                tree.Insert(word);

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
                        foreach (var word in search)
                            Console.WriteLine(word);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
}