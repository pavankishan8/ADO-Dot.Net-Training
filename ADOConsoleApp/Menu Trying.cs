using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ADOConsoleApp
{
    public class Option
    {
        public string Name { get; }
        public Action Selected { get; }

        public Option(string name, Action selected)
        {
            Name = name;
            Selected = selected;
        }
    }
    class Menu_Trying
    {
        private static List<Option> options;

        static void Main(string[] args)
        {
            options = new List<Option>
            {
                new Option("Calculator", () => Calculator()),
                new Option("Options", () => WriteTemporaryMessage("Settings")),
                new Option("Exit", () => Environment.Exit(0)),
            };

            int index = 0;

            WriteMenu(options, options[index]);

            ConsoleKeyInfo keyinfo;
            do
            {
                keyinfo = Console.ReadKey();

               
                if (keyinfo.Key == ConsoleKey.DownArrow)
                {
                    if (index + 1 < options.Count)
                    {
                        index++;
                        WriteMenu(options, options[index]);
                    }
                }
                if (keyinfo.Key == ConsoleKey.UpArrow)
                {
                    if (index - 1 >= 0)
                    {
                        index--;
                        WriteMenu(options, options[index]);
                    }
                }
                
                if (keyinfo.Key == ConsoleKey.Enter)
                {
                    options[index].Selected.Invoke();
                    index = 0;
                }
            }
            while (keyinfo.Key != ConsoleKey.X);

            Console.ReadKey();

        }

        static void Calculator()
        {
            int x, y;
            char ope;

            Console.Write("Input first number: ");
            x = Convert.ToInt32(Console.ReadLine());
            Console.Write("Input operation: ");
            ope = Convert.ToChar(Console.ReadLine());
            Console.Write("Input second number: ");
            y = Convert.ToInt32(Console.ReadLine());

            if (ope == '+')
            {
                Console.WriteLine(x + y);
            }
            else if (ope == '-')
            {
                Console.WriteLine(x - y);
            }
            else if (ope == '*')
            {
                Console.WriteLine(x * y);
            }
            else if (ope == '/')
            {
                Console.WriteLine(x / y);
            }
            else
            {
                Console.WriteLine("Wrong Character");
            }
            
        }
        static void WriteTemporaryMessage(string message)
        {
            Console.Clear();
            Console.WriteLine(message);
            Thread.Sleep(3000);
            WriteMenu(options, options.First());
        }

        static void WriteMenu(List<Option> options, Option selectedOption)
        {
            Console.Clear();

            foreach (Option option in options)
            {
                if (option == selectedOption)
                {
                    Console.Write("> ");
                }
                else
                {
                    Console.Write(" ");
                }

                Console.WriteLine(option.Name);
            }
        }
    }

}

