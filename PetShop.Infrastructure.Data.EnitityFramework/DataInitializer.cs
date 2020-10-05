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
        private readonly IColorRepository _colorRepository;
        private readonly IPetColorRepository _petColorRepository;
        private readonly IUserRepository _userRepository;

        public DataInitializer(IPetRepository petRepository, IPetTypeRepository petTypeRepository, IOwnerRepository ownerRepository, IColorRepository colorRepository, IPetColorRepository petColorRepository, IUserRepository userRepository)
        {
            _petRepository = petRepository;
            _petTypeRepository = petTypeRepository;
            _ownerRepository = ownerRepository;
            _colorRepository = colorRepository;
            _petColorRepository = petColorRepository;
            _userRepository = userRepository;
        }


        public void InitData()
        {
            List<PetColor> petColors1 = new List<PetColor>();
            List<PetColor> petColors2 = new List<PetColor>();
            List<PetColor> petColors3 = new List<PetColor>();
            List<PetColor> petColors4 = new List<PetColor>();

            List<Pet> pets = new List<Pet>();

            

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
            
                Color color1 = new Color
                {
                Name = "Red"
                };

                Color color2 = new Color
                {
                    Name = "Blue"
                };

                Color color3 = new Color
                {
                    Name = "Yellow"
                };

                Color color4 = new Color
                {
                    Name = "Green"
                };

                _colorRepository.AddColor(color1);
                _colorRepository.AddColor(color2);
                _colorRepository.AddColor(color3);
                _colorRepository.AddColor(color4);


                Pet pet1 = new Pet
                {
                    Name = "Jerry",
                    PetType = petType1,
                    BirthDate = DateTime.Now.AddYears(-12),
                    Owner = owner1,
                    Price = 50,
                    SoldDate = DateTime.Now.AddYears(-2),

                };

                Pet pet2 = new Pet
                {
                    Name = "Tom",
                    PetType = petType2,
                    BirthDate = DateTime.Now.AddYears(-22),
                    Owner = owner2,
                    Price = 10,
                    SoldDate = DateTime.Now.AddYears(-5),
                };

               Pet pet3 = new Pet
               {
                    Name = "Cinc",
                    PetType = petType3,
                    BirthDate = DateTime.Now.AddYears(-1),
                    Owner = owner3,
                    Price = 100,
                    SoldDate = DateTime.Now.AddYears(-4),
               };


               pet1.PetColors = new List<PetColor>
               {
                   new PetColor
                   {
                       Pet = pet1,
                       Color = color1,
                   } 
               };

               pet2.PetColors = new List<PetColor>
               {
                   new PetColor 
                   {
                       Pet = pet2, 
                       Color = color1

                   },
                   new PetColor
                   {
                       Pet = pet2,
                       Color = color2

                   }
               };

               pet3.PetColors = new List<PetColor>
               {
                   new PetColor
                   {
                       Pet = pet3,
                       Color = color1

                   },
                   new PetColor
                   {
                       Pet = pet3,
                       Color = color2

                   },

                   new PetColor
                   {
                       Pet = pet3,
                       Color = color3

                   }
               };

            //pet3.PetColors = new List<PetColor>
            //{
            //    petColor1,
            //    petColor2,
            //    petColor3
            //};

            //pets.Add(pet1);
            //pets.Add(pet2);
            //pets.Add(pet3);

            //_petRepository.AddPets(pets);

            _petRepository.AddPet(pet1);
            _petRepository.AddPet(pet2);
            _petRepository.AddPet(pet3);


            User user1 = new User
            {
                Username = "UserJoe",
                Password = "1234",
                IsAdmin = false
            };

            User user2 = new User
            {
                Username = "AdminAnn",
                Password = "1234", 
                IsAdmin = true
            };


            _userRepository.AddUser(user1);
            _userRepository.AddUser(user2);


            //_petColorRepository.AddPetColor(petColor1);
            //_petColorRepository.AddPetColor(petColor2);
            //_petColorRepository.AddPetColor(petColor3);
            //_petColorRepository.AddPetColor(petColor4);
            //_petColorRepository.AddPetColor(petColor5);
            //    //_petColorRepository.AddPetColor(petColor6);

            //petColors1.Add(petColor1);

            //    petColors2.Add(petColor1);
            //    petColors2.Add(petColor2);

            //    petColors3.Add(petColor1);
            //    petColors3.Add(petColor2);
            //    petColors3.Add(petColor3);

            //    petColors4.Add(petColor1);
            //    petColors4.Add(petColor2);
            //    petColors4.Add(petColor3);
            //    petColors4.Add(petColor4);







        }
    }
}