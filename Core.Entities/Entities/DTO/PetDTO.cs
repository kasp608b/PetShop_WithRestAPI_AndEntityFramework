using System;
using PetShop.Core.Entities.Entities.Business;

namespace PetShop.Core.Entities.Entities.DTO
{
    public class PetDTO
    {
        public PetDTO(int id, string name, PetType petType, DateTime birthDate, DateTime soldDate, string color, Owner previousOwner, double price)
        {
            ID = id;
            Name = name;
            PetType = petType;
            BirthDate = birthDate;
            SoldDate = soldDate;
            Color = color;
            PreviousOwner = previousOwner;
            Price = price;
        }

        public int ID { get; }
        public string Name { get; }
        public PetType PetType { get; }
        public DateTime BirthDate { get; }
        public DateTime SoldDate { get; }
        public string Color { get; }
        public Owner PreviousOwner { get; }
        public double Price { get; }

        public override string ToString()
        {
            return $"ID = {ID.ToString()}, Name = {Name.ToString()}, Type = {PetType.ToString()}, BirthDate = {BirthDate.ToString()}, SoldDate = {SoldDate.ToString()}, Color = {Color.ToString()}, PreviousOwner = {PreviousOwner.ToString()}, Price = {Price.ToString()},\n";
        }
    }
}