using System.Collections.Generic;
using PetShop.Core.Entities.Entities.Business;

namespace PetShop.Core.DomainService
{
    public interface IPetColorRepository
    {
        public PetColor AddPetColor(PetColor petColorToAdd);
        public List<PetColor> GetAllPetColors();
    }
}