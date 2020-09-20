using System.Collections.Generic;
using PetShop.Core.Entities.Entities.Business;

namespace PetShop.Core.Entities.Entities.DTO
{
    public class PetTypeDTO
    {
        public PetTypeDTO(int id, string name, List<Pet> pets)
        {
            ID = id;
            Name = name;
            Pets = pets;
        }

        public int ID { get; }
        public string Name { get; }
        public List<Pet> Pets { get; }

        public override string ToString()
        {
            return $"ID = {ID.ToString()}, Name = {Name.ToString()}\n";
        }
    }
}