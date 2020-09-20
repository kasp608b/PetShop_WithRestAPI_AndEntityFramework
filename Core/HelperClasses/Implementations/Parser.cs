
using PetShop.Core.HelperClasses.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PetShop.Core.HelperClasses.Implementations
{
    public class Parser : IParser
    {
        
        public bool IsDateParsable(string stringToParse, out DateTime parsedDate)
        {
            DateTime searchDateParsed;
            if (!DateTime.TryParse(stringToParse, out searchDateParsed))
            {
                parsedDate = searchDateParsed;
                return false;
            }
            else
            {
                parsedDate = searchDateParsed;
                return true;
            }
        }

        public bool IsDoubleParsable(string stringToParse, out double parsedDouble)
        {
            double searchDoubleParsed;

            if (!Double.TryParse(stringToParse, out searchDoubleParsed))
            {

                parsedDouble = searchDoubleParsed;
                return false;
            }
            else
            {
                parsedDouble = searchDoubleParsed;
                return true;
            }
        }

        public bool IsIntParsable(string stringToParse, out int parsedInt)
        {
            int searchintParsed;

            if (!int.TryParse(stringToParse, out searchintParsed))
            {

                parsedInt = searchintParsed;
                return false;
            }
            else
            {
                parsedInt = searchintParsed;
                return true;
            }
        }
    }
}
