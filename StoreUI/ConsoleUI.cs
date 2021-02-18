using System;

namespace StoreUI
{
    /// <summary>
    /// View for the Store application that reads to and writes from the console
    /// </summary>
    public class ConsoleUI : IUserInterface
    {
        public void PrintText(string text) {
            Console.WriteLine(text);
        }

        public string GetLine(string prompt) {
            Console.Write(prompt);
            return Console.ReadLine();
        }

        public string GetLine() {
            return GetLine("");
        }

        public void PrintResult(string text) {
            Console.Clear();
            Console.WriteLine(text);
            Console.Write("Press enter to continue:");
            Console.ReadLine();
            Console.Clear();
        }
    }
}