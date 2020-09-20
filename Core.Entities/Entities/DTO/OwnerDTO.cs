using System;
using System.Collections.Generic;
using PetShop.Core.Entities.Entities.Business;

namespace PetShop.Core.Entities.Entities.DTO
{
    public class OwnerDTO
    {
        public OwnerDTO(int id, string name, string email, DateTime birthDate, List<Pet> pets)
        {
            ID = id;
            Name = name;
            Email = email;
            BirthDate = birthDate;
            Pets = pets;
        }

        public int ID { get; }
        public string Name { get; }
        public string Email { get; }
        public DateTime BirthDate { get; }
        public List<Pet> Pets { get; }

        public override string ToString()
        {
            return $"ID = {ID.ToString()}, Name = {Name.ToString()}, BirthDate = {BirthDate.ToString()}, Email = {Email.ToString()},\n";
        }
    }
}