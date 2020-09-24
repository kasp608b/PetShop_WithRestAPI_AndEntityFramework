using System;
using System.Collections.Generic;

namespace PetShop.Core.Entities.Entities.Business
{
    public class Owner
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTime BirthDate { get; set; }
        public ICollection<Pet> Pets { get; set; }

        public override string ToString()
        {
            return $"ID = {Id.ToString()}, Name = {Name.ToString()}, BirthDate = {BirthDate.ToString()}, Email = {Email.ToString()},\n";
        }
    }
}
