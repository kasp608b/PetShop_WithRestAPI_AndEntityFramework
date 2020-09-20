using System.Collections.Generic;

namespace PetShop.Core.Entities.Entities.Business
{
    public class PetType
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public override string ToString()
        {
            return $"ID = {ID.ToString()}, Name = {Name.ToString()}\n";
        }
    }
}
