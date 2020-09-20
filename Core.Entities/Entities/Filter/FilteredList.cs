using System.Collections.Generic;

namespace PetShop.Core.Entities.Entities.Filter
{
    public class FilteredList<T>
    {
        public Entities.Filter.Filter FilterUsed { get; set; }
        public int TotalCount { get; set; }
        public List<T> List { get; set; }
    }
}