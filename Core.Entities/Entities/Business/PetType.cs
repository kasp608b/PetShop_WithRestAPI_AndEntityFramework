using System.Collections.Generic;

namespace PetShop.Core.Entities.Entities.Business
{
    public class PetType
    {
        public int PetTypeId { get; set; }
        public string Name { get; set; }
        public ICollection<Pet> Pets { get; set; }

        public override string ToString()
        {
            return $"ID = {PetTypeId.ToString()}, Name = {Name.ToString()}\n";
        }
    }
}
