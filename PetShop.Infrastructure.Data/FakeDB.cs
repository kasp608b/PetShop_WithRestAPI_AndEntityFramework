using PetShop.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using PetShop.Core.Entities.Entities;
using PetShop.Core.Entities.Entities.Business;
using PetType = PetShop.Core.Entities.Entities.Business.PetType;

namespace PetShop.Infrastructure.Data
{
    public static class FakeDB
    {
        private static int _petID = 1;
        private static int _ownerID = 1;
        private static int _petTypeID = 1;
        public static List<Pet> _pets = new List<Pet>();
        public static List<Owner> _owners = new List<Owner>();
        public static List<PetType> _petTypes = new List<PetType>();

        public static Pet AddPet(Pet pet)
        {
            pet.ID = _petID++;
            _pets.Add(pet);
            return pet;
        }

        public static Owner AddOwner(Owner owner)
        {
            owner.ID = _ownerID++;
            _owners.Add(owner);
            return owner;
        }

        public static PetType AddPetTypes(PetType petType)
        {
            petType.ID = _petTypeID++;
            _petTypes.Add(petType);
            return petType;
        }

    }

}
