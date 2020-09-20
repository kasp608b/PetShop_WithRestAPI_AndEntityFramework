using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace PetShop.Core.Entities.Entities.Filter
{
    
    public class Filter
    {
        
        public string OrderDirection { get; set; }

        public string OrderProperty { get; set; }
        public string SearchText { get; set; }
        public string SearchField { get; set; }
        public int ItemsPrPage { get; set; }
        public int CurrentPage { get; set; }

    }
}
