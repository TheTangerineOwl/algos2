using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace algos2
{
    // 2 способ (Вершина хранит указатель на начало односвязного списка.
    // Элемент списка содержит пару: символ, указатель.
    // Поиск пути - обычный перебор списка
    class Node : INode
    {
        char symbol;
        List<Node> branches = new();
        bool isKey = false;

        public char Value {
            get { return symbol; }
            set
            {
                symbol = value;
            }
        }
        public bool IsKey
        {
            get { return isKey; }
            set
            {
                isKey = value;
            }
        }

        public bool HasChild(char value)
        {
            foreach (Node node in branches)
            {
                if (node.symbol == value)
                    return true;
            }
            return false;
        }

        public INode AddChild(char value)
        {
            Node child = new Node();
            child.symbol = value;
            branches.Add(child);
            return child;
        }

        public INode? GetChild(char value)
        {
            foreach (Node node in branches)
            {
                if (node.symbol == value)
                    return node;
            }
            return null;
        }

        public List<INode> GetDescendants()
        {
            List<INode> desc = new();
            foreach (Node child in branches)
            {
                desc.Add(child);
                desc.AddRange(child.GetDescendants());
            }
            return desc;
        }

        public List<string> GetWords(string parentWord = "")
        {
            List<string> pref = new();
            if (branches.Count == 0 && IsKey)
                //return [parentWord + Value.ToString()];
                return [parentWord];
            foreach (Node child in branches)
            {
                //if (child.IsKey)
                //pref.Add(parentWord + child.Value);
                pref.AddRange(child.GetWords(parentWord + child.Value));
            }
            return pref;
        }
    }

    class Trie
    {
        Node root = new();

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

        //public void Insert(string key, char value)
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
                    return null;///////////////
                node = node.GetChild(ch);
            }
            foreach (Node d in node.GetDescendants())
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
                    return null;///////////////
                node = node.GetChild(ch);
            }
            foreach (string d in node.GetWords(key))
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
                    values.Add(value.Split(' ')[1]);
                }
            }
            return values;
        }

        static void Main()
        {
            Trie tree = new();
            List<string> values = ReadFile("words.txt");
            foreach (string word in values)
                tree.Insert(word);//, word[^1]);

            while (true)
            {
                Console.Write("Введите префикс для поиска (пустая строка для завершения работы): ");
                string? input;
                while ((input = Console.ReadLine()) == null)
                    Console.Write("Попробуйте еще раз: ");
                if (input == "")
                    break;

                List<string>? search = tree.SearchWords(input);
                if (search == null)
                    Console.WriteLine("Не найдено результатов");
                else
                    foreach (var word in search)
                        Console.WriteLine(word);
            }
        }
    }
}