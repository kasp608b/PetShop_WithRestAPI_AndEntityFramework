﻿namespace PetShop.Core.Entities.Entities.Business
{
    public class PetColor
    {
        public int PetId { get; set; }
        public Pet Pet { get; set; }

        public int ColorId { get; set; }
        public Color Color { get; set; }

    }
}