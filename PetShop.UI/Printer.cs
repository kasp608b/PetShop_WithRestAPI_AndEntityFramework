using PetShop.Core.ApplicationService;
using PetShop.Core.ApplicationService.Implementations;
using PetShop.Core.DomainService;
using PetShop.Core.Entities;
using PetShop.Core.Entities.Enums;
using PetShop.Core.HelperClasses.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace PetShop.UI
{
    public class Printer : IPrinter
    {
        private IPetService _petService;
        private IParser _parser;
        public Printer(IPetService petService, IParser parser)
        {
            _petService = petService;
            _parser = parser;
        }

        public void StartMenu()
        {
            int selection;

            string[] menuItems =
            {
                "List All Pets",
                "Add Pet",
                "Delete Pet",
                "Edit Pet",
                "Search by type",
                "Sort pets by type",
                "Get the 5 cheapest pets",
                "Exit"
            };
            try
            {
                selection = ShowMenu(menuItems, MenuTypes.Main);
            }
            catch (InvalidDataException e)
            {
                Console.WriteLine("Something went wrong with menuTypes\n" + e);
                selection = menuItems.Length;
            }
            
            bool exit = false;

            while (exit == false)
            {
                switch (selection)
                {
                    case 1:

                        Console.Clear();
                        Console.WriteLine("List of pets\n");
                        PrintAllPets();
                        break;

                    case 2:

                        Console.Clear();
                        Console.WriteLine("Add Pet\n");
                        AddPet();
                        break;

                    case 3:

                        Console.Clear();
                        Console.WriteLine("Delete Pet\n");
                        DeletePet();
                        break;

                    case 4:

                        Console.Clear();
                        Console.WriteLine("Edit Pet\n");
                        EditPet();
                        break;

                    case 5:

                        Console.Clear();
                        Console.WriteLine("Search by type\n");
                        SearchByType();
                        break;

                    case 6:

                        Console.Clear();
                        Console.WriteLine("Sort pets by price\n");
                        SortByPrice();
                        break;

                    case 7:

                        Console.Clear();
                        Console.WriteLine("Get the five cheapest pets");
                        Get5CheapestPets();
                        break;

                    default:
                        Console.Clear();
                        Console.WriteLine("Bye bye!");
                        exit = true;
                        break;
                }

                if (exit == false)
                {
                    Console.WriteLine("Press any key to go back to menu");
                    Console.ReadLine();
                    selection = ShowMenu(menuItems, MenuTypes.Main);
                }

            }
        }

        public void PrintAllPets()
        {
            foreach (Pet pet in _petService.GetPets())
            {
                Console.WriteLine(pet.ToString());
            }
        }

        public void AddPet()
        {
            int selection;
            string name;
            PetType type;
            DateTime birthDate;
            DateTime soldDate;
            string color;
            string previousOwner;
            double price;

            Console.WriteLine("Input valid pet name\n");

            name = Console.ReadLine();

            string[] menuItems =
            {
                "Cat",
                "Dog",
                "Fish",
                "Goat",
                "Tiger",
                "Parrot",
            };

            try
            {
                selection = ShowMenu(menuItems, MenuTypes.PetTypes);
            }
            catch (InvalidDataException e)
            {
                Console.WriteLine("Something went wrong with menuTypes\n" + e);
                selection = menuItems.Length;
            }

            switch (selection)
            {
                case 1:
                    type = PetType.Cat;
                    break;

                case 2:
                    type = PetType.Dog;
                    break;

                case 3:
                    type = PetType.Fish;
                    break;

                case 4:
                    type = PetType.Goat;
                    break;

                case 5:
                    type = PetType.Parrot;
                    break;

                case 6:
                    type = PetType.Tiger;
                    break;
                default:
                    type = PetType.Kakorot;
                    break;
            }

            Console.WriteLine("Input valid birthdate for pet\n");
            Console.WriteLine("Format is day/month/year\n");
            while (!_parser.IsDateParsable(Console.ReadLine(), out birthDate))
            {
                Console.WriteLine("Not a valid date, please try again");
            }

            Console.WriteLine("Input valid solddate for pet\n");
            Console.WriteLine("Format is day/month/year\n");
            while (!_parser.IsDateParsable(Console.ReadLine(), out soldDate))
            {
                Console.WriteLine("Not a valid date, please try again");
            }

            Console.WriteLine("Input at valid color for pet\n");
            color = Console.ReadLine();

            Console.WriteLine("Input at valid previous owner for pet\n");
            previousOwner = Console.ReadLine();

            Console.WriteLine("Input a valid price for pet\n");

            while (!_parser.IsDoubleParsable(Console.ReadLine(), out price))
            {
                Console.WriteLine("Not a valid price, please try again");
            }

            _petService.AddPet(name, type, birthDate, soldDate, color, previousOwner, price);
        }

        public void DeletePet()
        {
            try
            {
                Console.WriteLine("A list of all the pets\n");
                PrintAllPets();

                Console.WriteLine("Input valid pet id");

                int parsedId;
                while (!_parser.IsIntParsable(Console.ReadLine(), out parsedId))
                {
                    Console.WriteLine("Invalid id, please try again");
                }

                Console.WriteLine($"Succesfully deleted {_petService.DeletePet(parsedId).Name.ToString()}");


            }
            catch (InvalidDataException e)
            {
                Console.WriteLine("Something went wrong with the the pet deletetion\n" + e);
            }
        }

        public void EditPet()
        {
            try
            {
                Console.WriteLine("A list of all the pets\n");
                PrintAllPets();

                Console.WriteLine("Input valid pet id");

                int parsedId;
                while (!_parser.IsIntParsable(Console.ReadLine(), out parsedId))
                {
                    Console.WriteLine("Invalid id, please try again");
                }

                int selection;
                string name;
                PetType type;
                DateTime birthDate;
                DateTime soldDate;
                string color;
                string previousOwner;
                double price;

                Console.WriteLine("Input valid pet name\n");

                name = Console.ReadLine();

                string[] menuItems =
                {
                    "Cat",
                    "Dog",
                    "Fish",
                    "Goat",
                    "Tiger",
                    "Parrot",
                };

                try
                {
                    selection = ShowMenu(menuItems, MenuTypes.PetTypes);
                }
                catch (InvalidDataException e)
                {
                    Console.WriteLine("Something went wrong with menuTypes\n" + e);
                    selection = menuItems.Length;
                }

                switch (selection)
                {
                    case 1:
                        type = PetType.Cat;
                        break;

                    case 2:
                        type = PetType.Dog;
                        break;

                    case 3:
                        type = PetType.Fish;
                        break;

                    case 4:
                        type = PetType.Goat;
                        break;

                    case 5:
                        type = PetType.Parrot;
                        break;

                    case 6:
                        type = PetType.Tiger;
                        break;
                    default:
                        type = PetType.Kakorot;
                        break;
                }

                Console.WriteLine("Input valid birthdate for pet\n");
                Console.WriteLine("Format is day/month/year\n");
                while (!_parser.IsDateParsable(Console.ReadLine(), out birthDate))
                {
                    Console.WriteLine("Not a valid date, please try again");
                }

                Console.WriteLine("Input valid solddate for pet\n");
                Console.WriteLine("Format is day/month/year\n");
                while (!_parser.IsDateParsable(Console.ReadLine(), out soldDate))
                {
                    Console.WriteLine("Not a valid date, please try again");
                }

                Console.WriteLine("Input at valid color for pet\n");
                color = Console.ReadLine();

                Console.WriteLine("Input at valid previous owner for pet\n");
                previousOwner = Console.ReadLine();

                Console.WriteLine("Input a valid price for pet\n");

                while (!_parser.IsDoubleParsable(Console.ReadLine(), out price))
                {
                    Console.WriteLine("Not a valid price, please try again");
                }

                Console.WriteLine($"Succesfully edited {_petService.EditPet(parsedId, name, type, birthDate, soldDate, color, previousOwner, price).ToString()}");
            }
            catch (InvalidDataException e)
            {
                Console.WriteLine("Something went wrong with the the pet deletetion\n" + e);
            }
        }

        public void SearchByType()
        {
            int selection;
            PetType type;
            string[] menuItems =
               {
                    "Cat",
                    "Dog",
                    "Fish",
                    "Goat",
                    "Tiger",
                    "Parrot",
                };

            try
            {
                selection = ShowMenu(menuItems, MenuTypes.PetTypes);
            }
            catch (InvalidDataException e)
            {
                Console.WriteLine("Something went wrong with menuTypes\n" + e);
                selection = menuItems.Length;
            }

            switch (selection)
            {
                case 1:
                    type = PetType.Cat;
                    break;

                case 2:
                    type = PetType.Dog;
                    break;

                case 3:
                    type = PetType.Fish;
                    break;

                case 4:
                    type = PetType.Goat;
                    break;

                case 5:
                    type = PetType.Parrot;
                    break;

                case 6:
                    type = PetType.Tiger;
                    break;
                default:
                    type = PetType.Kakorot;
                    break;
            }

            try
            {
                foreach (Pet pet in _petService.SearchByType(type))
                {
                    Console.WriteLine(pet.ToString());
                }
            }
            catch (InvalidDataException e)
            {
                Console.WriteLine("Something went wrong with search\n" + e);
            }

        }

        public void SortByPrice()
        {
            foreach (Pet pet in _petService.SortPetsByPrice())
            {
                Console.WriteLine(pet.ToString());
            } 
        }

        public void Get5CheapestPets()
        {
            if(_petService.SortPetsByPrice().Count >= 5)
            {
                for (int i = 0; i <= 4; i++)
                {
                    Console.WriteLine(_petService.SortPetsByPrice().ElementAt(i).ToString());
                }
            }
            else 
            {
                foreach (Pet pet in _petService.SortPetsByPrice())
                {
                    Console.WriteLine(pet.ToString());
                }
            }
           
        }

        public int ShowMenu(string[] menuItems, MenuTypes type)
        {
            Console.Clear();

            switch (type)
            {
                case MenuTypes.Main:
                    Console.WriteLine("Select what you want to do:\n");
                    break;
                case MenuTypes.Properties:
                    Console.WriteLine("Choose which property to perform action on:\n");
                    break;
                case MenuTypes.PetTypes:
                    Console.WriteLine("Select a pet type:\n");
                    break;
                default:
                    throw new InvalidDataException("Wrong enum given");
            }


            for (int i = 0; i < menuItems.Length; i++)
            {
                Console.WriteLine($"{(i + 1)} : { menuItems[i]}");
            }
            int selection;


            while (!_parser.IsIntParsable(Console.ReadLine(), out selection)
               //OR
               || selection < 1
               || selection > menuItems.Length)
            {
                Console.WriteLine("You need to select a number between 1-" + menuItems.Length);
            }

            Console.WriteLine("Selection: " + selection);
            return selection;
        }
    }
}
