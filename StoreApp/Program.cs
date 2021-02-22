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
                        AddToCartMenu();
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
            List<Product> products = dataStore.GetAllProducts();
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
            Console.Clear();
            userInterface.PrintText(products[index].ToString());
            userInterface.PrintText("\nAvailable At:");
            foreach(Location loc in dataStore.GetAvailableLocations(products[index])) {
                userInterface.PrintText("\t" + loc.LocationName + "\t(" + dataStore.GetLocationInventory(loc, products[index]) + " in stock)");
            }
            userInterface.PrintResult("");
        }

        static void AddToCartMenu() {
            userInterface.PrintText("Enter the number associated with any product to add it to your cart");
            userInterface.PrintText("Or press enter to continue without adding anything");
            List<Product> products;
            if (cart.Location != null) {
                products = dataStore.GetAvailableProducts(cart.Location);
            } else {
                products = dataStore.GetAllProducts();
            }
            for (int i = 0; i < products.Count; i++) {
                userInterface.PrintText("[" + i + "] " + products[i].ProductName);
            }
            if (products.Count == 0) {
                userInterface.PrintText("There are no products to show");
            }
            string input = userInterface.GetLine();
            int index;
            if (!int.TryParse(input, out index))
                return;
            if (index < 0 || index >= products.Count)
                return;
            Console.Clear();
            if (cart.Location == null) {
                LocationSelectMenu(products[index]);
            }
            Console.Clear();
            if (cart.Location == null) {
                return;
            }
            int inventory = dataStore.GetLocationInventory(cart.Location, products[index]);
            int nProduct;
            do {
                userInterface.PrintText("How many of this product would you like to buy?");
                userInterface.PrintText("Please enter a number in the range [ 1 , " + inventory + " ]");
                input = userInterface.GetLine();
                if (!int.TryParse(input, out nProduct)) {
                    Console.Clear();
                    userInterface.PrintResult("Canceled adding to cart");
                    return;
                }
            } while (nProduct < 1 || nProduct > inventory);
            Item item = new Item();
            item.Product = products[index];
            item.Quantity = nProduct;
            cart.Items.Add(item);
            dataStore.UpdateLocationInventory(cart.Location, products[index], -nProduct);
            Console.Clear();
            userInterface.PrintResult("Successfully added " + nProduct + " of " + products[index].ProductName + " to cart");
        }

        static void LocationSelectMenu(Product product) {
            userInterface.PrintText("Enter the number corresponding to the location you want to order from");
            userInterface.PrintText("Or press enter to return to the main menu");
            List<Location> locations = dataStore.GetAvailableLocations(product);
            for (int i = 0; i < locations.Count; i++) {
                userInterface.PrintText("[" + i + "] " + locations[i].LocationName + "\t(" + dataStore.GetLocationInventory(locations[i], product) + " in stock)");
            }
            if (locations.Count == 0) {
                userInterface.PrintText("There are no locations to show");
            }
            string input = userInterface.GetLine();
            int index;
            if (!int.TryParse(input, out index))
                return;
            if (index < 0 || index >= locations.Count)
                return;
            cart.Location = locations[index];
        }
    }
}
