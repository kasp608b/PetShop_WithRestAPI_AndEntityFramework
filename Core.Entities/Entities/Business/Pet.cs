using System;

namespace PetShop.Core.Entities.Entities.Business
{
    public class Pet
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int PetTypeID { get; set; }
        public DateTime BirthDate { get; set; }
        public DateTime SoldDate { get; set; }
        public string Color { get; set; }
        public int PreviousOwnerID { get; set; }
        public double Price { get; set; }

        public override string ToString()
        {
            return $"ID = {ID.ToString()}, Name = {Name.ToString()}, Type = {PetTypeID.ToString()}, BirthDate = {BirthDate.ToString()}, SoldDate = {SoldDate.ToString()}, Color = {Color.ToString()}, PreviousOwner = {PreviousOwnerID.ToString()}, Price = {Price.ToString()},\n";
        }
    }
}
