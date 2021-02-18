using System;
using System.Collections.Generic;
using StoreModels;
using StoreUI;
using StoreData;

namespace StoreApp
{
    class Program
    {
        static IUserInterface userInterface;
        static IDataStore dataStore;
        static Order cart = null;
        static Customer currentUser = null;
        static void Main(string[] args)
        {
            userInterface = new ConsoleUI();
            dataStore = new MemoryDataStore();
            bool exit = false;
            while (!exit) {
                Login();
                exit = MainMenu();
            }
        }
        static void Login() {
            string name = userInterface.GetLine("Enter your name: ");
            currentUser = dataStore.GetCustomer(name);
            if (currentUser == null) {
                currentUser = new Customer();
                currentUser.Name = name;
                dataStore.AddCustomer(name, currentUser);
            }
            cart = new Order();
        }

        static bool MainMenu() {
            //Console.Clear();
            while (currentUser != null) {
                userInterface.PrintText("Welcome, " + currentUser.Name);
                userInterface.PrintText(
                    "Please choose an option to continue...\n" +
                    "[0] View cart\n" +
                    "[1] Add item to cart\n" +
                    "[2] Search for items\n" +
                    "[3] Check Out\n" +
                    "[4] Log Out\n" +
                    "[5] Exit"
                );
                switch(userInterface.GetLine()) {
                    case "0":
                        Console.Clear();
                        userInterface.PrintText("TODO");
                        break;
                    case "1":
                        Console.Clear();
                        userInterface.PrintText("TODO");
                        break;
                    case "2":
                        Console.Clear();
                        ListProductsMenu();
                        break;
                    case "3":
                        Console.Clear();
                        userInterface.PrintText("TODO");
                        break;
                    case "4":
                        Console.Clear();
                        currentUser = null;
                        break;
                    case "5":
                        Console.Clear();
                        return true;
                    default:
                        Console.Clear();
                        userInterface.PrintText("Invalid input, please enter one of the given options");
                        break;
                }
            }
            return false;
        }

        static void ListProductsMenu() {
            userInterface.PrintText("Enter the number associated with any product to view more information");
            userInterface.PrintText("Or press enter to continue");
            List<Product> products = dataStore.GetProducts();
            for (int i = 0; i < products.Count; i++) {
                userInterface.PrintText("[" + i + "] " + products[i].ProductName);
            }
            if (products.Count == 0) {
                userInterface.PrintText("There are no products in the data store");
            }
            string input = userInterface.GetLine();
            int index;
            if (!int.TryParse(input, out index))
                return;
            if (index < 0 || index >= products.Count)
                return;
            userInterface.PrintResult(products[index].ToString());
        }
    }
}
