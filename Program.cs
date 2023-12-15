using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleTreeIterator
{
    public class Node
    {
        public int Value { get; set; }
        public List<Node> Children { get; } = new List<Node>();

        public Node(int value)
        {
            Value = value;
        }

        // Método para adicionar um filho a este nó
        public void AddChild(int value)
        {
            Children.Add(new Node(value));
        }
    }

    public class TreeIterator
    {
        private readonly Node _root;
        private List<Node> _visitedNodes = new List<Node>();
        private int _currentIndex = -1;

        public TreeIterator(Node root)
        {
            _root = root;
        }

        public void Reset()
        {
            _visitedNodes.Clear();
            _currentIndex = -1;
        }

        public bool MoveNext()
        {
            _currentIndex++;
            return _currentIndex < _visitedNodes.Count;
        }

        public Node Current
        {
            get
            {
                if (_currentIndex >= 0 && _currentIndex < _visitedNodes.Count)
                {
                    return _visitedNodes[_currentIndex];
                }
                else
                {
                    throw new InvalidOperationException("Iterator is positioned before the first element or after the last element.");
                }
            }
        }

        public List<Node> GetVisitedNodes()
        {
            return _visitedNodes;
        }

        public void IterateTopToBottom()
        {
            Reset();
            TopToBottomIterate(_root);
        }

        public void IterateBottomToTopInBranch(Node branch)
        {
            Reset();
            BottomToTopIterateInBranch(branch);
        }

        private void TopToBottomIterate(Node node)
        {
            _visitedNodes.Add(node);

            foreach (var child in node.Children)
            {
                TopToBottomIterate(child);
            }
        }

        private void BottomToTopIterateInBranch(Node node)
        {
            _visitedNodes.Add(node);

            foreach (var child in node.Children)
            {
                BottomToTopIterateInBranch(child);
            }
        }
    }

    public class TreePrinter
    {
        public static void PrintTree(Node node, List<Node> visitedNodes, string indent = "", bool last = true)
        {
            Console.Write(indent);
            if (last)
            {
                Console.Write("\\-");
                indent += "  ";
            }
            else
            {
                Console.Write("|-");
                indent += "| ";
            }

            if (visitedNodes.Contains(node))
            {
                Console.ForegroundColor = ConsoleColor.Green;
            }

            Console.WriteLine(node.Value);
            Console.ResetColor();

            for (int i = 0; i < node.Children.Count; i++)
            {
                PrintTree(node.Children[i], visitedNodes, indent, i == node.Children.Count - 1);
            }
        }
    }
    internal class Program
    {
        private static Node AddTreeAndBranches()
        {
            // Criação da árvore de exemplo
            Node root = new Node(1);
            root.AddChild(2);
            root.AddChild(3);
            root.AddChild(12);
            root.AddChild(13);
            root.Children[0].AddChild(4);
            root.Children[0].AddChild(5);
            root.Children[1].AddChild(6);
            root.Children[1].AddChild(7);
            root.Children[0].AddChild(4);
            root.Children[0].AddChild(5);
            root.Children[1].AddChild(6);
            root.Children[1].AddChild(7);
            root.Children[0].Children[0].AddChild(8);
            root.Children[0].Children[0].AddChild(9);
            root.Children[1].Children[1].AddChild(10);
            root.Children[1].Children[1].AddChild(11);
            return root;

            
        }
        private static void PrintMenu()
        {
            Console.WriteLine("Escolha o modo de iteração:");
            Console.WriteLine("1. Top to Bottom");
            Console.WriteLine("2. Bottom to Top in Branch");
            Console.WriteLine("3. Exibir Resumo");
            Console.WriteLine("4. Simular exceção (Current antes de MoveNext)");
            Console.WriteLine("5. Sair");
        }
        private static bool MenuOptions(Node root, TreeIterator treeIterator, bool exit)
        {
            while (!exit)
            {
                PrintMenu();

                int choice;
                if (int.TryParse(Console.ReadLine(), out choice))
                {
                    switch (choice)
                    {
                        case 1:
                            Console.WriteLine("Top to Bottom:");
                            treeIterator.IterateTopToBottom();
                            break;
                        case 2:
                            Console.WriteLine("Bottom to Top in Branch:");
                            Console.WriteLine("Escolha a branch (1 to " + root.Children.Count + "): ");
                            int branchChoice;
                            if (int.TryParse(Console.ReadLine(), out branchChoice) && (branchChoice >= 1 && branchChoice <= root.Children.Count))
                            {
                                treeIterator.IterateBottomToTopInBranch(root.Children[branchChoice - 1]);
                            }
                            else
                            {
                                Console.WriteLine("Escolha de branch inválida.");
                                continue;
                            }
                            break;
                        case 3:
                            Console.WriteLine("Resumo:");
                            for (int i = 0; i < root.Children.Count; i++)
                            {
                                Console.WriteLine($"{i + 1}. Bottom to Top in Branch {i + 1}");
                            }
                            Console.WriteLine($"{root.Children.Count + 1}. Sair");
                            break;
                        case 4:
                            // Tentar acessar Current antes de chamar MoveNext
                            try
                            {
                                Console.WriteLine($"Tentando acessar Current: {treeIterator.Current.Value}");
                            }
                            catch (InvalidOperationException ex)
                            {
                                Console.WriteLine($"Exceção capturada: {ex.Message}");
                            }
                            continue;
                        case 5:
                            exit = true;
                            continue;
                        default:
                            Console.WriteLine("Escolha inválida.");
                            break;
                    }

                    if (choice == 1 || choice == 2)
                    {
                        // Imprimir a árvore
                        TreePrinter.PrintTree(root, treeIterator.GetVisitedNodes());
                        // Reiniciar para a próxima iteração
                        treeIterator.Reset();
                    }
                    else if (choice == 3)
                    {
                        continue; // Continue sem imprimir a árvore para o resumo
                    }

                    // Se a escolha for 3, continuamos sem imprimir a árvore
                    // Se a escolha for 1 ou 2, a árvore já foi impressa acima
                }
                else
                {
                    Console.WriteLine("Escolha inválida.");
                }
            }

            return exit;
        }
        static void Main(string[] args)
        {
            Node root = AddTreeAndBranches();
            TreeIterator treeIterator = new TreeIterator(root);
            bool exit = false;
            _ = MenuOptions(root, treeIterator, exit);
        }
    }
}
