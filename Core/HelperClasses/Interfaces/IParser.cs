using System;
using System.Collections.Generic;
using System.Text;

namespace PetShop.Core.HelperClasses.Interfaces
{
    public interface IParser
    {
        
        public bool IsIntParsable(string stringToParse, out int parsedInt);

        public bool IsDoubleParsable(string stringToParse, out double parsedDouble);

        public bool IsDateParsable(string stringToParse, out DateTime parsedDate);
    }
}
