using System;
using System.Collections.Generic;
using PetShop.Core.ApplicationService;
using PetShop.Core.ApplicationService.Interfaces;
using PetShop.Core.DomainService;
using PetShop.Core.Entities.Entities.Business;

namespace PetShop.Infrastructure.Data.EntityFramework
{
    public class DataInitializer
    {
        private readonly IPetRepository _petRepository;
        private readonly IPetTypeRepository _petTypeRepository;
        private readonly IOwnerRepository _ownerRepository;

        public DataInitializer(IPetRepository petRepository, IPetTypeRepository petTypeRepository, IOwnerRepository ownerRepository)
        {
            _petRepository = petRepository;
            _petTypeRepository = petTypeRepository;
            _ownerRepository = ownerRepository;
        }

        public void InitData()
        {
            List<Pet> pets;


            Owner owner1 = new Owner
            {
                Name = "Harold",
                BirthDate = DateTime.Now.AddYears(-40),
                Email = "HaroldKork@gmail.uk"
            };

            Owner owner2 = new Owner
            {
                Name = "Carry",
                BirthDate = DateTime.Now.AddYears(-30),
                Email = "KarryOckthorp@gmail.uk"
            };

            Owner owner3 = new Owner
            {
                  Name = "Tom",
                  BirthDate = DateTime.Now.AddYears(-25),
                  Email = "TomYork@gmail.uk"
            };

            _ownerRepository.AddOwner(owner1);
            _ownerRepository.AddOwner(owner2);
            _ownerRepository.AddOwner(owner3);



            PetType petType1 = new PetType
              {
                  Name = "Cat",
              };

              PetType petType2 = new PetType
              {
                  Name = "Dog",
              };

              PetType petType3 = new PetType
              {
                  Name = "Bird",
              };

              _petTypeRepository.AddPetType(petType1);
              _petTypeRepository.AddPetType(petType2);
              _petTypeRepository.AddPetType(petType3);

            pets = new List<Pet> {
                new Pet
                {
                    Name = "Jerry",
                    PetType = petType1,
                    BirthDate = DateTime.Now.AddYears(-12),
                    Color = "Blue",
                    Owner = owner1,
                    Price = 50,
                    SoldDate = DateTime.Now.AddYears(-2),

                },
                new Pet
                {
                    Name = "Tom",
                    PetType =  petType2,
                    BirthDate = DateTime.Now.AddYears(-22),
                    Color = "Red",
                    Owner = owner2,
                    Price = 10,
                    SoldDate = DateTime.Now.AddYears(-5),
                },
                new Pet
                {
                    Name = "Cinc",
                    PetType =  petType3,
                    BirthDate = DateTime.Now.AddYears(-1),
                    Color = "Purple",
                    Owner = owner3,
                    Price = 100,
                    SoldDate = DateTime.Now.AddYears(-4),
                }
            };

            foreach (Pet pet in pets)
            {
                _petRepository.AddPet(pet);
            }


        }
    }
}