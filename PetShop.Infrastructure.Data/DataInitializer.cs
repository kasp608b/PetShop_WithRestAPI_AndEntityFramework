using PetShop.Core.DomainService;
using PetShop.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using PetShop.Core.Entities.Entities;
using PetShop.Core.Entities.Entities.Business;
using PetType = PetShop.Core.Entities.Entities.Business.PetType;

namespace PetShop.Infrastructure.Data
{
    public class DataInitializer
    {
       
        private IPetRepository _petRepository;
        private IOwnerRepository _ownerRepository;
        private IPetTypeRepository _petTypeRepository;
        public DataInitializer(IPetRepository petRepository, IOwnerRepository ownerRepository, IPetTypeRepository petTypeRepository)
        {
            _petRepository = petRepository;
            _ownerRepository = ownerRepository;
            _petTypeRepository = petTypeRepository;
        }

        public void InitData()
        {
            List<Pet> pets;
            List<Owner> owners;
            List<PetType> petTypes;

            pets = new List<Pet> {
                new Pet
                {
                    Name = "Jerry",
                    PetTypeID = 1,
                    BirthDate = DateTime.Now.AddYears(-12),
                    Color = "Blue",
                    PreviousOwnerID = 1,
                    Price = 50,
                    SoldDate = DateTime.Now.AddYears(-2),

                },
                new Pet
                {
                    Name = "Tom",
                    PetTypeID = 2,
                    BirthDate = DateTime.Now.AddYears(-22),
                    Color = "Red",
                    PreviousOwnerID = 2,
                    Price = 10,
                    SoldDate = DateTime.Now.AddYears(-5),
                },
                new Pet
                {
                    Name = "Cinc",
                    PetTypeID = 3,
                    BirthDate = DateTime.Now.AddYears(-1),
                    Color = "Purple",
                    PreviousOwnerID = 3,
                    Price = 100,
                    SoldDate = DateTime.Now.AddYears(-4),
                }
            };

            foreach (Pet pet in pets)
            {
                _petRepository.AddPet(pet);
            }

            owners = new List<Owner> {
                new Owner
                {
                    Name = "Harold",
                    BirthDate = DateTime.Now.AddYears(-40),
                    Email = "HaroldKork@gmail.uk" 
                },
                new Owner
                {
                    Name = "Carry",
                    BirthDate = DateTime.Now.AddYears(-30),
                    Email = "KarryOckthorp@gmail.uk"
                },
                new Owner
                {
                    Name = "Tom",
                    BirthDate = DateTime.Now.AddYears(-25),
                    Email = "TomYork@gmail.uk"
                }
            };

            foreach (Owner owner in owners)
            {
                _ownerRepository.AddOwner(owner);
            }

            petTypes = new List<PetType> {
                new PetType
                {
                    Name = "Cat",
                },
                new PetType
                {
                    Name = "Dog",
                },
                new PetType
                {
                    Name = "Bird",
                }
            };

            foreach (PetType petType in petTypes)
            {
                _petTypeRepository.AddPetType(petType);
            }
        }
    }
}
