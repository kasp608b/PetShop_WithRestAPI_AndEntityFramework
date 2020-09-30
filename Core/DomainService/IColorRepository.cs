using System.Collections.Generic;
using PetShop.Core.Entities.Entities.Business;

namespace PetShop.Core.DomainService
{
    public interface IColorRepository
    {
        public List<Color> GetAllColors();
        public Color AddColor(Color colorToAdd);
    }
}