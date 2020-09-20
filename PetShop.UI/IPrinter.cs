using PetShop.Core.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace PetShop.UI
{
    public interface IPrinter
    {
        public void StartMenu();

        public void PrintAllPets();

        public void AddPet();

        public void DeletePet();

        public void EditPet();
        public void SearchByType();
        public void Get5CheapestPets();
        public int ShowMenu(string[] menuItems, MenuTypes type);
    }
}