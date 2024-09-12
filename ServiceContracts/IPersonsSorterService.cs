using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceContracts.DTO;
using ServiceContracts.Enums;

namespace ServiceContracts
{
    public interface IPersonsSorterService
    {
       
        /// <summary>
        /// Returns SortedList of persons
        /// </summary>
        /// <param name="allpersons"></param>
        /// <param name="sortBy"></param>
        /// <param name="sortOrder"></param>
        /// <returns></returns>
        Task<List<PersonResponse>> GetSortedPersons(List<PersonResponse> allpersons, string sortBy, SortOrderOptions sortOrder);
        
       

    }
}
