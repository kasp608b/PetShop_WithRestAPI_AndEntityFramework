using System;
using System.Collections.Generic;

namespace PetShop.Core.Entities.Entities.Business
{
    public class Pet
    {
        public int PetId { get; set; }
        public string Name { get; set; }
        public PetType PetType { get; set; }
        public DateTime BirthDate { get; set; }
        public DateTime SoldDate { get; set; }
        public Owner Owner { get; set; }
        public double Price { get; set; }

        public ICollection<PetColor> PetColors { get; set; }

        public override string ToString()
        {
            return $"ID = {PetId.ToString()}, Name = {Name.ToString()}, Type = {PetType.ToString()}, BirthDate = {BirthDate.ToString()}, SoldDate = {SoldDate.ToString()}, PreviousOwner = {Owner.ToString()}, Price = {Price.ToString()},\n";
        }
    }
}
