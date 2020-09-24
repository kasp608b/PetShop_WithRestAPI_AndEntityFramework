using System;

namespace PetShop.Core.Entities.Entities.Business
{
    public class Pet
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public PetType PetType { get; set; }
        public DateTime BirthDate { get; set; }
        public DateTime SoldDate { get; set; }
        public string Color { get; set; }
        public Owner Owner { get; set; }
        public double Price { get; set; }

        public override string ToString()
        {
            return $"ID = {Id.ToString()}, Name = {Name.ToString()}, Type = {PetType.ToString()}, BirthDate = {BirthDate.ToString()}, SoldDate = {SoldDate.ToString()}, Color = {Color.ToString()}, PreviousOwner = {Owner.ToString()}, Price = {Price.ToString()},\n";
        }
    }
}
